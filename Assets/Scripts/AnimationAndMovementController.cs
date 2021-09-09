using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    PlayerInput input;
    CharacterController characterController;
    Animator animator;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;

    bool movementPressed;
    bool runPressed;

    float rotationFactorPerFrame = 15f;
    float runMultiplier = 3f;

    int isWalkingHash;
    int isRunningHash;

    void Awake()
    {
        input = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

        input.CharacterControls.Movement.started += Movement_performed;
        input.CharacterControls.Movement.canceled += Movement_performed;
        input.CharacterControls.Movement.performed += Movement_performed;
        input.CharacterControls.Run.performed += Run_performed;
    }

    private void Run_performed(InputAction.CallbackContext ctx)
    {
        runPressed = ctx.ReadValueAsButton();
    }

    private void Movement_performed(InputAction.CallbackContext ctx)
    {
        currentMovementInput = ctx.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;
        movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
    }

    void HandleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if (movementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if (!movementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if (movementPressed && runPressed && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }

        if ((!movementPressed || !runPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (movementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    void HandleGravity()
    {
        if (characterController.isGrounded)
        {
            float groundedGravity = -0.5f;
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        } else
        {
            float groundedGravity = -9.8f;
            currentMovement.y += groundedGravity;
            currentRunMovement.y += groundedGravity;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleGravity();
        HandleRotation();
        HandleAnimation();
        if (runPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime);
        }
    }

    void OnEnable()
    {
        input.CharacterControls.Enable();
    }

    void OnDisable()
    {
        input.CharacterControls.Disable();
    }
}
