using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;            // Normal walking speed
    public float crouchSpeed = 2f;      // Speed when crouching
    public float runSpeed = 10f;        // Running speed
    public float rotationSpeed = 10f;   // Speed at which character rotates

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

        // Create a new movement vector
        Vector3 velocity = Vector3.zero;

        // Movement states: Crouching, Running, or Walking
        if (Input.GetKey(KeyCode.LeftControl)) // Crouching
        {
            velocity = HandleCrouchMovement(horizontalInput, verticalInput);
        }
        else if (Input.GetKey(KeyCode.LeftShift)) // Running
        {
            velocity = HandleRunMovement(horizontalInput, verticalInput);
        }
        else // Walking
        {
            velocity = HandleWalkMovement(horizontalInput, verticalInput);
        }

        // Apply gravity
        velocity.y = ySpeed;

        // Move the character controller
        characterController.Move(velocity * Time.deltaTime);

        // Rotate the character to face movement direction
        RotateCharacter(movementDirection);
    }

    // Handles all crouch-related movement and animations
    private Vector3 HandleCrouchMovement(float horizontalInput, float verticalInput)
    {
        Vector3 velocity = Vector3.zero;

        if (verticalInput > 0) // Crouch walk forward
        {
            velocity = transform.forward * crouchSpeed;
            animator.SetBool("IsCrouchWalking", true);
        }
        else if (horizontalInput < 0) // Crouch walk left
        {
            velocity = -transform.right * crouchSpeed;
            animator.SetBool("IsCrouchWalkingLeft", true);
        }
        else if (horizontalInput > 0) // Crouch walk right
        {
            velocity = transform.right * crouchSpeed;
            animator.SetBool("IsCrouchWalkingRight", true);
        }
        else // Crouch in place
        {
            ResetMovementBools();
            animator.SetBool("IsCrouching", true);
        }

        return velocity;
    }

    // Handles all running-related movement and animations
    private Vector3 HandleRunMovement(float horizontalInput, float verticalInput)
    {
        Vector3 velocity = Vector3.zero;

        if (verticalInput > 0) // Run forward
        {
            velocity = transform.forward * runSpeed;
            animator.SetBool("IsRunning", true);
        }
        else if (horizontalInput < 0) // Run left
        {
            velocity = -transform.right * runSpeed;
            animator.SetBool("IsRunningLeft", true);
        }
        else if (horizontalInput > 0) // Run right
        {
            velocity = transform.right * runSpeed;
            animator.SetBool("IsRunningRight", true);
        }

        return velocity;
    }

    // Handles all walking-related movement and animations
    private Vector3 HandleWalkMovement(float horizontalInput, float verticalInput)
    {
        Vector3 velocity = Vector3.zero;

        if (verticalInput > 0) // Walk forward
        {
            velocity = transform.forward * speed;
            animator.SetBool("IsWalking", true);
        }
        else if (horizontalInput < 0) // Walk left
        {
            velocity = -transform.right * speed;
            animator.SetBool("IsWalkingLeft", true);
        }
        else if (horizontalInput > 0) // Walk right
        {
            velocity = transform.right * speed;
            animator.SetBool("IsWalkingRight", true);
        }
        else // Idle
        {
            ResetMovementBools();
            animator.SetBool("IsIdle", true);
        }

        return velocity;
    }

    // Method to rotate the character to face the direction of movement
    private void RotateCharacter(Vector3 movementDirection)
    {
        if (movementDirection.magnitude > 0.1f) // Check if there's significant movement
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Utility method to reset all movement-related animation booleans
    private void ResetMovementBools()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsWalkingLeft", false);
        animator.SetBool("IsWalkingRight", false);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsRunningLeft", false);
        animator.SetBool("IsRunningRight", false);
        animator.SetBool("IsCrouchWalking", false);
        animator.SetBool("IsCrouchWalkingLeft", false);
        animator.SetBool("IsCrouchWalkingRight", false);
        animator.SetBool("IsCrouching", false);
        animator.SetBool("IsIdle", false);
    }
}
