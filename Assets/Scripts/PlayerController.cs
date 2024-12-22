using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class GroundLayer
    {
        public string layerName;
        public Texture2D[] groundTextures;
        public AudioClip[] footstepSounds;
    }

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runMultiplier = 1.5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Mouse Look")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float mouseVerticalClamp = 60f;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;

    [Header("Footsteps")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private float groundCheckDistance = 1.0f;
    [SerializeField] private float footstepRate = 1f;
    [SerializeField] private float runningFootstepRate = 1.5f;
    [SerializeField] private List<GroundLayer> groundLayers;

    private CharacterController _characterController;
    private Vector3 _moveDirection;
    private Vector3 _velocity;
    private float _verticalRotation;
    private bool _isRunning;
    private float _nextFootstep;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    private void HandleMovement()
    {
        if (_characterController.isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        if (Input.GetKey(jumpKey) && _characterController.isGrounded)
            _velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        _moveDirection = transform.forward * vertical + transform.right * horizontal;

        _isRunning = Input.GetKey(runKey);
        float currentSpeed = walkSpeed * (_isRunning ? runMultiplier : 1f);
        _characterController.Move(_moveDirection * currentSpeed * Time.deltaTime);

        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);

        HandleFootsteps(horizontal, vertical, currentSpeed);
    }

    private void HandleMouseLook()
    {
        float xAxis = Input.GetAxis("Mouse X");
        float yAxis = Input.GetAxis("Mouse Y");

        _verticalRotation -= yAxis * mouseSensitivity;
        _verticalRotation = Mathf.Clamp(_verticalRotation, -mouseVerticalClamp, mouseVerticalClamp);

        playerCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
        transform.Rotate(Vector3.up * xAxis * mouseSensitivity);
    }

    private void HandleFootsteps(float horizontal, float vertical, float currentSpeed)
    {
        if (_characterController.isGrounded && (horizontal != 0 || vertical != 0))
        {
            float rate = _isRunning ? runningFootstepRate : footstepRate;
            _nextFootstep += rate * currentSpeed;

            if (_nextFootstep >= 100f)
            {
                PlayFootstep();
                _nextFootstep = 0;
            }
        }
    }

    private void PlayFootstep()
    {
        foreach (var layer in groundLayers)
        {
            foreach (var texture in layer.groundTextures)
            {
                if (GetGroundTexture() == texture)
                {
                    footstepSource.PlayOneShot(RandomClip(layer.footstepSounds));
                    return;
                }
            }
        }
    }

    private Texture2D GetGroundTexture()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundCheckDistance))
        {
            if (hit.collider.TryGetComponent(out Terrain terrain))
            {
                return terrain.terrainData.terrainLayers[0].diffuseTexture; // Simplified for example
            }

            if (hit.collider.TryGetComponent(out Renderer renderer))
            {
                return renderer.material.mainTexture as Texture2D;
            }
        }
        return null;
    }

    private AudioClip RandomClip(AudioClip[] clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }
}
