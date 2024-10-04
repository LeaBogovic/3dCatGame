using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;  // Reference to the Character Controller component
    public Transform cam;                   // Reference to the camera transform
    public Animator animator;               // Reference to Animator component

    public float walkSpeed = 3f;            // Walking speed
    public float runSpeed = 6f;             // Running speed
    private float speed;                    // Speed variable to switch between walking and running

    public float turnSmoothTime = 0.1f;     // Time to smoothly rotate character
    float turnSmoothVelocity;               // Used for smooth rotation

    // Parameters to track running and walking states
    private bool isRunning = false;
    private bool isWalking = false;

    void Start()
    {
        // Start with walking speed
        speed = walkSpeed;

        // Get the Animator component attached to the same GameObject
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Capture Input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // 2. Determine the speed and state (walking or running)
        if (Input.GetKey(KeyCode.LeftShift) && direction.magnitude >= 0.1f)
        {
            speed = runSpeed;
            isRunning = true;
            isWalking = false;
        }
        else if (direction.magnitude >= 0.1f)
        {
            speed = walkSpeed;
            isRunning = false;
            isWalking = true;
        }
        else
        {
            isRunning = false;
            isWalking = false;
        }

        // 3. Set the Animator parameters based on states
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isWalking", isWalking);

        // 4. Handle Rotation and Movement
        if (direction.magnitude >= 0.1f)
        {
            // Calculate target angle based on camera's y rotation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            // Smoothly rotate the character towards the target angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Calculate the movement direction relative to camera and move the character
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // 5. Handle Attack Trigger
        if (Input.GetMouseButtonDown(0)) // Assuming left mouse button for attack
        {
            animator.SetTrigger("Attack");
        }
    }
}
