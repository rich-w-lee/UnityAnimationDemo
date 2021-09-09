using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;

    PlayerInput input;

    Vector2 currentMovement;
    bool movementPressed;
    bool runPressed;

    void Awake()
    {
        input = new PlayerInput();

        input.CharacterControls.Movement.performed += Movement_performed;
        input.CharacterControls.Run.performed += Run_performed;
    }

    private void Run_performed(InputAction.CallbackContext ctx)
    {
        runPressed = ctx.ReadValueAsButton();
    }

    private void Movement_performed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Movement_performed");
        currentMovement = ctx.ReadValue<Vector2>();
        movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleRotation()
    {
        Vector3 currentPosition = transform.position;
        Vector3 newPosition = new Vector3(currentMovement.x, 0, currentMovement.y);
        Vector3 positionToLookAt = currentPosition + newPosition;

        transform.LookAt(positionToLookAt);
    }

    void HandleMovement()
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

    void OnEnable()
    {
        input.CharacterControls.Enable();
    }

    void OnDisable()
    {
        input.CharacterControls.Disable();
    }
}
