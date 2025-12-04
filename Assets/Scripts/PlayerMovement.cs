using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(StaminaManager))]
public class PlayerMovement : MonoBehaviour
{
    private GameManager gameManager;
    public Camera playerCamera;
    public float setWalkSpeed = 1.5f;
    public float setRunSpeed = 2.2f;
    public float jumpPower = 4f;
    public float gravity = 10f;
    public float lookSpeed = 1.5f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 1f;

    [Header("Headbob Settings")]
    public bool enableHeadbob = true;
    public float walkBobSpeed = 12f;
    public float walkBobAmount = 0.03f;
    public float runBobSpeed = 16f;
    public float runBobAmount = 0.08f;
    public float crouchBobSpeed = 8f;
    public float crouchBobAmount = 0.02f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private StaminaManager stamina;
    private bool canMove = true;
    private bool isRunning;
    private float walkSpeed;
    private float runSpeed;

    // Headbob variables
    private float bobTimer = 0f;
    private Vector3 cameraDefaultPos;

    private void Awake()
    {
        stamina = GetComponent<StaminaManager>();
    }

    void Start()
    {
        walkSpeed = setWalkSpeed;
        runSpeed = setRunSpeed;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Find GameManager at runtime (works with prefabs)
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        // Save camera default position for headbob
        if (playerCamera != null)
        {
            cameraDefaultPos = playerCamera.transform.localPosition;
        }
    }

    void Update()
    {
        if (gameManager != null && gameManager.gamePaused)
        {
            return;
        }

        HandleMovement();

        if (gameManager == null || !gameManager.gamePaused)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpPower;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.R) && canMove)
            {
                characterController.height = crouchHeight;
                walkSpeed = crouchSpeed;
                runSpeed = crouchSpeed;
            }
            else
            {
                characterController.height = defaultHeight;
                walkSpeed = setWalkSpeed;
                runSpeed = setRunSpeed;
            }

            characterController.Move(moveDirection * Time.deltaTime);

            // Headbob effect
            HandleHeadbob();
        }
    }

    private void LateUpdate()
    {
        if (gameManager != null && gameManager.gamePaused)
        {
            return;
        }

        if (canMove && (gameManager == null || !gameManager.gamePaused))
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void HandleMovement()
    {
        bool wantsToSprint = Input.GetKey(KeyCode.LeftShift);
        isRunning = wantsToSprint && !stamina.IsDepleted();

        if (wantsToSprint && moveDirection.magnitude > 0 && !stamina.IsDepleted())
        {
            if (!stamina.CanUseStamina(stamina.depletionRate * Time.deltaTime))
            {
                isRunning = false;
                characterController.Move(moveDirection * walkSpeed * Time.deltaTime);
            }
            else
            {
                stamina.UseStaminaContinuous(Time.deltaTime);
                characterController.Move(moveDirection * runSpeed * Time.deltaTime);
                isRunning = true;
            }
        }
        else
        {
            isRunning = false;
            characterController.Move(moveDirection * walkSpeed * Time.deltaTime);
        }
    }

    // HEADBOB EFFECT
    private void HandleHeadbob()
    {
        if (!enableHeadbob || playerCamera == null)
            return;

        // Check if moving
        bool isMoving = (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0);

        if (characterController.isGrounded && isMoving)
        {
            // Determine bob speed and amount based on movement state
            float currentBobSpeed;
            float currentBobAmount;

            if (Input.GetKey(KeyCode.R)) // Crouching
            {
                currentBobSpeed = crouchBobSpeed;
                currentBobAmount = crouchBobAmount;
            }
            else if (isRunning)
            {
                currentBobSpeed = runBobSpeed;
                currentBobAmount = runBobAmount;
            }
            else // Walking
            {
                currentBobSpeed = walkBobSpeed;
                currentBobAmount = walkBobAmount;
            }

            // Increment bob timer
            bobTimer += Time.deltaTime * currentBobSpeed;

            // Calculate headbob using sine wave
            float bobOffsetY = Mathf.Sin(bobTimer) * currentBobAmount;
            float bobOffsetX = Mathf.Cos(bobTimer / 2) * currentBobAmount * 0.5f; // Subtle side-to-side

            // Apply headbob to camera
            playerCamera.transform.localPosition = new Vector3(
                cameraDefaultPos.x + bobOffsetX,
                cameraDefaultPos.y + bobOffsetY,
                cameraDefaultPos.z
            );
        }
        else
        {
            // Smoothly return to default position when not moving
            bobTimer = 0f;
            playerCamera.transform.localPosition = Vector3.Lerp(
                playerCamera.transform.localPosition,
                cameraDefaultPos,
                Time.deltaTime * 5f
            );
        }
    }
}