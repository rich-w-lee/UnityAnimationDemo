using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateControllerV3 : MonoBehaviour
{
    Animator animator;
    float velocityX = 0.0f;
    float velocityZ = 0.0f;
    int velocityXHash;
    int velocityZHash;
    float offset = 0.1f;

    public float acceleration = 2.0f;
    public float decelaration = 2.0f;
    public float maxRunV = 2.0f;
    public float maxWalkV = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        velocityXHash = Animator.StringToHash("VelocityX");
        velocityZHash = Animator.StringToHash("VelocityZ");
    }

    void ChangeVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxV)
    {
        // Forward
        if (forwardPressed && velocityZ < currentMaxV)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * decelaration;
        }

        if (!forwardPressed && velocityZ < 0.0f)
        {
            velocityZ = 0.0f;
        }

        // Left
        if (leftPressed && velocityX > -currentMaxV)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * decelaration;
        }

        // Right
        if (rightPressed && velocityX < currentMaxV)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * decelaration;
        }
    }

    void  LockOrResetVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxV)
    {
        // Left Right Reset
        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX < offset && velocityX > -offset))
        {
            velocityX = 0.0f;
        }

        // Lock Forward on Run
        if (forwardPressed && runPressed && velocityZ > currentMaxV)
        {
            velocityZ = currentMaxV;
        }
        // Decelarate from run to walk
        else if (forwardPressed && velocityZ > currentMaxV)
        {
            velocityZ -= Time.deltaTime * decelaration;

            // Round to current max velocity if within offset
            if (velocityZ > currentMaxV && velocityZ < (currentMaxV + offset))
            {
                velocityZ = currentMaxV;
            }
        }
        // Round to current max velocity if within offset
        else if (forwardPressed && velocityZ < currentMaxV && velocityZ > (currentMaxV - offset))
        {
            velocityZ = currentMaxV;
        }

        // Lock Left
        if (leftPressed && runPressed && velocityX < -currentMaxV)
        {
            velocityX = -currentMaxV;
        }
        // Decelarate from run to walk
        else if (leftPressed && velocityX < -currentMaxV)
        {
            velocityX += Time.deltaTime * decelaration;

            // Round to current max velocity if within offset
            if (velocityX < -currentMaxV && velocityX > (-currentMaxV - offset))
            {
                velocityX = -currentMaxV;
            }
        }
        // Round to current max velocity if within offset
        else if (leftPressed && velocityX > -currentMaxV && velocityX < (-currentMaxV + offset))
        {
            velocityX = -currentMaxV;
        }

        // Lock Right
        if (rightPressed && runPressed && velocityZ > currentMaxV)
        {
            velocityX = currentMaxV;
        }
        // Decelarate from run to walk
        else if (rightPressed && velocityX > currentMaxV)
        {
            velocityX -= Time.deltaTime * decelaration;

            // Round to current max velocity if within offset
            if (velocityX > currentMaxV && velocityX < (currentMaxV + offset))
            {
                velocityX = currentMaxV;
            }
        }
        // Round to current max velocity if within offset
        else if (rightPressed && velocityX < currentMaxV && velocityX > (currentMaxV - offset))
        {
            velocityX = currentMaxV;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        float currentMaxV = runPressed ? maxRunV : maxWalkV;

        ChangeVelocity(forwardPressed, leftPressed, rightPressed, runPressed, currentMaxV);

        LockOrResetVelocity(forwardPressed, leftPressed, rightPressed, runPressed, currentMaxV);

        animator.SetFloat(velocityXHash, velocityX);
        animator.SetFloat(velocityZHash, velocityZ);
    }
}
