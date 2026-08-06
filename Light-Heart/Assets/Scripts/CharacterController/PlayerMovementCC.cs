using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Header("Stamina")]
    public float maxStamina = 100f;
    public float staminaDrainRate = 20f;
    public float staminaRegenRate = 15f;
    public float staminaRegenDelay = 1.5f;
    public float staminaPercent => currentStamina / maxStamina;

    private float currentStamina;
    private float regenDelayTimer;

    public float walkspeed = 6f;
    public float sprintspeed = 10f;
    public float mouseSensitivity = 150f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    public Camera playerCamera;
    public float normalFOV = 60f;
    public float sprintFOV = 80f;
    public float fovSpeed = 8f;

    public Transform cameraTransform;
    public Transform groundCheck;
    public LayerMask groundMask;

    public float groundDistance = 0.4f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;

        currentStamina = maxStamina;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Check if sprint key is held
        bool isMovingForward = z > 0;
        bool wantsToSprint = Input.GetKey(KeyCode.LeftShift) && isMovingForward && isGrounded;
        bool isSprinting = wantsToSprint && currentStamina > 0f;
        HandleStamina(isSprinting);
        HandleFOV(isSprinting);

        float currentSpeed = isSprinting ? sprintspeed : walkspeed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Change FOV while sprinting

    void HandleFOV(bool isSprinting)
    {
        float targetFOV = isSprinting ? sprintFOV : normalFOV;

        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, fovSpeed * Time.deltaTime);
    }

    // Handle Stamina while sprinting

    void HandleStamina(bool isSprinting)
    {
        if (isSprinting)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0f);
            regenDelayTimer = staminaRegenDelay;
        }
        else
        {
            if (regenDelayTimer > 0f)
            {
                regenDelayTimer -= Time.deltaTime;
            }
            else
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Min(currentStamina, maxStamina);
            }
        }
    }
}

