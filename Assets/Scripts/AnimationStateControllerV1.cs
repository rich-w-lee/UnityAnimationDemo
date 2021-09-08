using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateControllerV1 : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;

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
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        bool fowardPressed = Input.GetKey(KeyCode.W);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        if(!isWalking && fowardPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (isWalking && !fowardPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if(!isRunning && fowardPressed && runPressed)
        {
            animator.SetBool(isRunningHash, true);
        }

        if (isRunning && (!fowardPressed || !runPressed))
        {
            animator.SetBool(isRunningHash, false);
        }
    }
}
