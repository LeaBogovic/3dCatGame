using System.Collections;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float jumpForce;
    [SerializeField] private BoxCollider ground; // Ground detection
    [SerializeField] private LayerMask groundMask;

    private Rigidbody rb;
    private Animator animator;
    private int currentAnimationHash;
    private Vector2 movement = Vector2.zero; // Stores WASD input
    private int currentIdle = 0;
    private bool grounded = false;
    private bool isTurning = false; // Flag for turning animation
    private bool isTurningBackwards = false; // Flag for handling backward turn animation

    // Animator state hashes
    readonly int IDLE = Animator.StringToHash("idle1");
    readonly int WALKFORWARD = Animator.StringToHash("Walk_Foward");
    readonly int WALKRIGHT = Animator.StringToHash("Walk_Right");
    readonly int WALKLEFT = Animator.StringToHash("Walk_Left");
    readonly int RUNFORWARD = Animator.StringToHash("Run_Forward");
    readonly int RUNLEFT = Animator.StringToHash("Run_Left");
    readonly int RUNRIGHT = Animator.StringToHash("Run_Right");
    readonly int CROUCH = Animator.StringToHash("Crouch");
    readonly int CROUCHFORWARD = Animator.StringToHash("Crouch_Forward");
    readonly int CROUCHLEFT = Animator.StringToHash("Crouch_Left");
    readonly int CROUCHRIGHT = Animator.StringToHash("Crouch_Right");
    readonly int TURN_LEFT = Animator.StringToHash("TurnLeft");
    readonly int TURN_RIGHT = Animator.StringToHash("TurnRight");

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        StartCoroutine(ChangeIdle());
        ChangeAnimation(IDLE); // Initial animation
    }

    private IEnumerator ChangeIdle()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            currentIdle = (currentIdle + 1) % 5; // Cycle through idle animations
        }
    }

    void Update()
    {
        // Getting WASD movement
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Check for jump
        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            ChangeAnimation(Animator.StringToHash("InPlace"));
        }

        // Check for bite action
        if (grounded && Input.GetKeyDown(KeyCode.Mouse0))
            ChangeAnimation(Animator.StringToHash("Bite"));

        // Check for crouch
        if (Input.GetKey(KeyCode.LeftControl)) // Assuming Left Control is the crouch key
        {
            CheckCrouchAnimation();
        }
        else
        {
            CheckAnimation();
        }
    }

    private void CheckCrouchAnimation()
    {
        if (movement.y > 0) // W pressed
        {
            if (movement.x > 0) // D pressed
                ChangeAnimation(CROUCHRIGHT); // Crouch right while moving forward
            else if (movement.x < 0) // A pressed
                ChangeAnimation(CROUCHLEFT); // Crouch left while moving forward
            else
                ChangeAnimation(CROUCHFORWARD); // Just W pressed
        }
        else if (movement.x > 0) // D pressed
        {
            ChangeAnimation(CROUCHRIGHT); // Only D pressed
        }
        else if (movement.x < 0) // A pressed
        {
            ChangeAnimation(CROUCHLEFT); // Only A pressed
        }
        else
        {
            ChangeAnimation(CROUCH); // No directional input, just crouching
        }
    }

    public void ChangeAnimation(int animationHash, float crossfade = 0.2f, float time = 0)
    {
        if (time > 0) StartCoroutine(Wait());
        else Validate(animationHash);

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(time - crossfade);
            Validate(animationHash);
        }

        void Validate(int animationHash)
        {
            if (currentAnimationHash != animationHash)
            {
                currentAnimationHash = animationHash;
                animator.CrossFade(animationHash, crossfade);
            }
        }
    }

    private void CheckAnimation()
    {
        if (currentAnimationHash == Animator.StringToHash("Land_stop") ||
            currentAnimationHash == Animator.StringToHash("InPlace") ||
            currentAnimationHash == Animator.StringToHash("Bite"))
            return;

        if (currentAnimationHash == Animator.StringToHash("Fall_low") && grounded)
        {
            ChangeAnimation(Animator.StringToHash("Land_stop"));
            return;
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isMovingForward = movement.y > 0; // W pressed
        bool isMovingBackward = movement.y < 0; // S pressed
        bool isMovingRight = movement.x > 0; // D pressed
        bool isMovingLeft = movement.x < 0; // A pressed

        // Handle backward movement with turning animation
        if (isMovingBackward && !isTurningBackwards)
        {
            StartCoroutine(PlayTurnAnimationTwice());
            return; // Exit this function while turning backward
        }

        // Prioritize diagonal movement with W + A or W + D
        if (isMovingForward && isMovingRight)
        {
            ChangeAnimation(isRunning ? RUNRIGHT : WALKRIGHT); // Running or walking diagonally to the right
        }
        else if (isMovingForward && isMovingLeft)
        {
            ChangeAnimation(isRunning ? RUNLEFT : WALKLEFT); // Running or walking diagonally to the left
        }
        else if (isMovingForward)
        {
            // Forward movement without diagonal
            ChangeAnimation(isRunning ? RUNFORWARD : WALKFORWARD); // Just W pressed
        }
        else if (isMovingLeft && !isMovingForward && !isTurning)
        {
            // Turning left (A pressed without forward movement)
            if (Input.GetKey(KeyCode.A))
            {
                isTurning = true;
                ChangeAnimation(TURN_LEFT); // Play turn left animation
                StartCoroutine(ResetTurningAnimation(TURN_LEFT));
            }
            else
            {
                ChangeAnimation(WALKLEFT); // Walking left
            }
        }
        else if (isMovingRight && !isMovingForward && !isTurning)
        {
            // Turning right (D pressed without forward movement)
            if (Input.GetKey(KeyCode.D))
            {
                isTurning = true;
                ChangeAnimation(TURN_RIGHT); // Play turn right animation
                StartCoroutine(ResetTurningAnimation(TURN_RIGHT));
            }
            else
            {
                ChangeAnimation(WALKRIGHT); // Walking right
            }
        }
        else
        {
            // No movement, check for idle animations
            CheckIdle();
        }
    }

    private IEnumerator PlayTurnAnimationTwice()
    {
        isTurningBackwards = true; // Lock backward movement until turn completes

        // Play turn animation to the right
        ChangeAnimation(TURN_RIGHT);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[TURN_RIGHT].length);

        // Play turn animation to the right again (total of 180 degrees turn)
        ChangeAnimation(TURN_RIGHT);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[TURN_RIGHT].length);

        isTurningBackwards = false; // Unlock backward movement after turn
    }

    private IEnumerator ResetTurningAnimation(int animationHash)
    {
        // Get the animation clip length using the animation name or hash
        AnimationClip clip = GetAnimationClipByHash(animationHash);
        if (clip != null)
        {
            yield return new WaitForSeconds(clip.length); // Wait for the animation to complete
        }
        isTurning = false; // Reset turning flag
    }

    // Helper function to retrieve animation clip by hash
    private AnimationClip GetAnimationClipByHash(int hash)
    {
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (Animator.StringToHash(clip.name) == hash)
            {
                return clip;
            }
        }
        return null; // Return null if the clip isn't found
    }


    private void CheckIdle()
    {
        switch (currentIdle)
        {
            case 0:
                ChangeAnimation(IDLE);
                break;
            case 1:
                ChangeAnimation(Animator.StringToHash("idle2"));
                break;
            case 2:
                ChangeAnimation(Animator.StringToHash("idle3"));
                break;
            case 3:
                ChangeAnimation(Animator.StringToHash("idle4"));
                break;
            case 4:
                ChangeAnimation(Animator.StringToHash("idle5"));
                break;
        }
    }

    private void FixedUpdate()
    {
        // Get velocity direction based on input
        Vector3 velocity = movementSpeed * (movement.x * transform.right + movement.y * transform.forward);
        // Apply velocity while maintaining the current y velocity for jumping
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
        // Update grounded state
        grounded = Physics.CheckBox(ground.transform.position + ground.center, 0.5f * ground.size, ground.transform.rotation, groundMask);
    }
}
