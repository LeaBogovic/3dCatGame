using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float crouchSpeed = 2f;
    public float runSpeed = 10f;

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Gather inputs
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // Update Y speed for gravity
        ySpeed += Physics.gravity.y * Time.deltaTime;
        if (characterController.isGrounded)
        {
            ySpeed = -0.5f; // Keep the player grounded
        }

        Vector3 velocity = Vector3.zero;

        // Determine movement state
        if (Input.GetKey(KeyCode.LeftControl)) // Crouching
        {
            HandleCrouchAnimations();
            if (verticalInput > 0) // Crouch walk forward
            {
                velocity = transform.forward * crouchSpeed;
            }
            else if (Input.GetKey(KeyCode.A)) // Crouch walk left
            {
                velocity = -transform.right * crouchSpeed; // Move left
                HandleCrouchWalkLeftAnimation();
            }
            else if (Input.GetKey(KeyCode.D)) // Crouch walk right
            {
                velocity = transform.right * crouchSpeed; // Move right
                HandleCrouchWalkRightAnimation();
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift)) // Running
        {
            if (Input.GetKey(KeyCode.A)) // Run left
            {
                velocity = -transform.right * runSpeed; // Move left
                HandleRunWalkLeftAnimation();
            }
            else if (Input.GetKey(KeyCode.D)) // Run right
            {
                velocity = transform.right * runSpeed; // Move right
                HandleRunWalkRightAnimation();
            }
            else if (verticalInput > 0) // Run forward
            {
                velocity = transform.forward * runSpeed;
                HandleRunAnimations();
            }
        }
        else // Walking
        {
            if (Input.GetKey(KeyCode.A)) // Walk left
            {
                velocity = -transform.right * speed; // Move left
                HandleWalkLeftAnimations();
            }
            else if (Input.GetKey(KeyCode.D)) // Walk right
            {
                velocity = transform.right * speed; // Move right
                HandleWalkRightAnimations();
            }
            else if (verticalInput > 0) // Walk forward
            {
                velocity = transform.forward * speed;
                HandleWalkAnimations();
            }
            else // Reset to idle
            {
                HandleIdleAnimations();
            }
        }

        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleWalkAnimations()
    {
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsCrouching", false);
        animator.SetBool("IsRunning", false);
    }

    private void HandleWalkLeftAnimations()
    {
        animator.SetBool("IsWalkingLeft", true);
        animator.SetBool("IsWalkingRight", false);
        HandleWalkAnimations();
    }

    private void HandleWalkRightAnimations()
    {
        animator.SetBool("IsWalkingRight", true);
        animator.SetBool("IsWalkingLeft", false);
        HandleWalkAnimations();
    }

    private void HandleRunAnimations()
    {
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsCrouching", false);
        animator.SetBool("IsWalkingLeft", false);
        animator.SetBool("IsWalkingRight", false);
    }

    private void HandleRunWalkLeftAnimation()
    {
        animator.SetBool("IsWalkingLeft", true);
        animator.SetBool("IsWalkingRight", false);
        HandleRunAnimations();
    }

    private void HandleRunWalkRightAnimation()
    {
        animator.SetBool("IsWalkingRight", true);
        animator.SetBool("IsWalkingLeft", false);
        HandleRunAnimations();
    }

    private void HandleCrouchAnimations()
    {
        animator.SetBool("IsCrouching", true);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsWalkingLeft", false);
        animator.SetBool("IsWalkingRight", false);
    }

    private void HandleCrouchWalkLeftAnimation()
    {
        animator.SetBool("IsWalkingLeft", true);
        animator.SetBool("IsWalkingRight", false);
        HandleCrouchAnimations();
    }

    private void HandleCrouchWalkRightAnimation()
    {
        animator.SetBool("IsWalkingRight", true);
        animator.SetBool("IsWalkingLeft", false);
        HandleCrouchAnimations();
    }

    private void HandleIdleAnimations()
    {
        animator.SetBool("IsIdle", true);
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsCrouching", false);
        animator.SetBool("IsWalkingLeft", false);
        animator.SetBool("IsWalkingRight", false);
    }
}
