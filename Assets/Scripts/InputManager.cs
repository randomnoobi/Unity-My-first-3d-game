using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float lookSpeed = 2f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private PlayerInput input;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;

    private float xRotation = 0f;
    private float verticalVelocity = 0f;
    private const float gravity = -9.81f;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter = 0f;

    // New: Track velocity
    private Vector3 lastPosition;
    public Vector3 Velocity { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();
        moveAction = input.actions["Move"];
        lookAction = input.actions["Look"];
        jumpAction = input.actions["Jump"];
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        jumpAction.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        jumpAction.performed -= OnJumpPerformed;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lastPosition = transform.position;  // initialize last position
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();

        // Update jump buffer timer
        if (jumpBufferCounter > 0)
            jumpBufferCounter -= Time.deltaTime;

        // Check if grounded and jump buffered, then jump
        if (controller.isGrounded && jumpBufferCounter > 0)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpBufferCounter = 0f;
        }

        // Update velocity based on position delta
        Velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }

    private void HandleMovement()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        if (controller.isGrounded && verticalVelocity < 0)
        {
            // Reset vertical velocity on ground
            verticalVelocity = -1f; // Small negative value to keep controller grounded properly
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void HandleLook()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>() * lookSpeed;

        xRotation -= lookInput.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * lookInput.x);
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        jumpBufferCounter = jumpBufferTime;
    }
}
