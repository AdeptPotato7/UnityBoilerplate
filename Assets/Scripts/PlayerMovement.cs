using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(StaminaManager))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    public Camera playerCamera;
    public float setWalkSpeed = 3f;
    public float setRunSpeed = 5f;
    public float jumpPower = 4f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 2f;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private StaminaManager stamina;
    private bool canMove = true;
    private bool isRunning;
    private float walkSpeed;
    private float runSpeed;
    
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
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (gameManager.gamePaused)
        {
            return;
        }

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
                walkSpeed = setWalkSpeed;
                runSpeed = setRunSpeed;
            }

            characterController.Move(moveDirection * Time.deltaTime);
        }
    }
    private void LateUpdate() 
    {
        if (gameManager.gamePaused)
        {
            return;
        }

        if (canMove && !gameManager.gamePaused)
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