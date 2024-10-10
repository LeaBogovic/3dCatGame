using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour

    

{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float jumpForce;
    [SerializeField] private BoxCollider ground;//ground detection
    [SerializeField] private LayerMask groundMask;
    private Rigidbody rb;
    private Animator animator;
    private string currentAnimation = "";
    private Vector2 movement = Vector2.zero;//stores wasd 
    private int currentidle = 0;

    private bool grounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(ChangeIdle());

        IEnumerator ChangeIdle()
        {
            while (true)
            {
                yield return new WaitForSeconds(2);
                ++currentidle;
                if(currentidle >= 5)
                    currentidle = 0;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        //getting wasda movemment
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //getting mouse movement
        Vector2 mouse = mouseSensitivity * Time.deltaTime * new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        //rotato potato
        transform.Rotate(mouse.x * Vector3.up);

        if (grounded && Input.GetKeyDown(KeyCode.Mouse0))
            ChangeAnimation("Bite");
        //check for jump
        if(grounded && Input.GetKeyDown(KeyCode.Space))
        rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);

        CheckAnimation();
    }
    private void ChangeAnimation(string animation, float crossfade = 0.2f)
    {
        if(currentAnimation != animation)
        {
            animator.CrossFade(animation, crossfade);
        }
    }

    private void CheckAnimation()
    {
        if (currentAnimation == "Bite")
            return;
        if (movement.y == 1)
            ChangeAnimation("Walk_Foward");
        else if (movement.x == 1)
            ChangeAnimation("Walk_Right");
        else if (movement.x == -1)
            ChangeAnimation("Walk_Left");
        else
            CheckIdle();
            
        void CheckIdle()
            {
                switch (currentidle)
                {
                case 0:
                    ChangeAnimation("idle1");
                    break;
                case 1:
                    ChangeAnimation("idle2");
                    break;
                case 2:
                    ChangeAnimation("idle3");
                    break;
                case 3:
                    ChangeAnimation("idle4");
                    break;
                case 4:
                    ChangeAnimation("idle5");
                    break;
                }
            }

        
    }

    private void FixedUpdate()
    {
        //get velocity dire
        Vector3 velocity = movementSpeed * (movement.x * transform.right + movement.y * transform.forward);
        //apply velocity
        rb.velocity = new Vector3(velocity.x, velocity.y, velocity.z);
        //update ground
        grounded = Physics.CheckBox(ground.transform.position + ground.center, 0.5f * ground.size, ground.transform.rotation, groundMask);
    }
}
