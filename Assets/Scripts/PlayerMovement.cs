using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(StaminaManager))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private StaminaManager stamina;
    private bool canMove = true;
    private bool isRunning;
    private void Awake()
    {
        stamina = GetComponent<StaminaManager>();
    }
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();

        if (!gameManager.gamePaused)
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
                walkSpeed = 6f;
                runSpeed = 12f;
            }

            characterController.Move(moveDirection * Time.deltaTime);

            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
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
                // Not enough stamina, go back to walking
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
}