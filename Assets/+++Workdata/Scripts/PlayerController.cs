using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Inspector

    [Header("Movement")]
    
    [Min(0)]
    [Tooltip("The maximum speed of the player in uu/s")]
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float crouchSpeed = 2f;

    [Min(0)]
    [Tooltip("How fast the movement speed is in-/decreasing")]
    [SerializeField] private float speedChangeRate = 10f;

    [SerializeField] private float rotationSpeed = 10f;

    [Header("Slope Movement")] 
    
    [SerializeField] private float pullDownForce = 5f;

    [SerializeField] private LayerMask raycastMask;

    [SerializeField] private float raycastLength = 0.5f;
    
    [Header("Camera")] 
    
    [SerializeField] private Transform cameraTarget;

    [SerializeField] private float verticalCameraRotationMin = -30f;

    [SerializeField] private float verticalCameraRotationMax = 70f;

    [SerializeField] private float cameraHorizontalSpeed = 200f;

    [SerializeField] private float cameraVerticalSpeed = 130f;

    [Header("Mouse Settings")] 
    
    [SerializeField] private float mouseCameraSensitivity = 1f;
    
    [Header("Controller Settings")] 
    [SerializeField] private float controllerCameraSensitivity = 1f;

    [SerializeField] private bool invertY = true;

    [Header("Animation")] 
    [SerializeField] private Animator anim;

    [SerializeField] private float coyoteTime;

    #endregion
    
    #region Private Variables

    private static readonly int Hash_MovementSpeed = Animator.StringToHash("MovementSpeed");
    private static readonly int Hash_Grounded = Animator.StringToHash("Grounded");
    private static readonly int Hash_Crouching = Animator.StringToHash("Crouching");
    
    private CharacterController characterController;
    
    private Player_InputActions inputActions;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction runAction;
    private InputAction crouchAction;
    
    private Vector2 moveInput;
    private Vector2 lookInput;

    private Quaternion characterTargetRotation = Quaternion.identity;
    
    private Vector2 cameraRotation;
    
    private Vector3 lastMovement;

    private float movementSpeed;
    private float currentSpeed;
    private bool isGrounded = true;
    private float airTime;
    private bool isCrouching;
    #endregion
    
    #region Event Functions
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        
        inputActions = new Player_InputActions();
        moveAction = inputActions.Player.Move;
        lookAction = inputActions.Player.Look;
        runAction = inputActions.Player.Run;
        crouchAction = inputActions.Player.Crouch;

        characterTargetRotation = transform.rotation;
        cameraRotation = cameraTarget.rotation.eulerAngles;
        movementSpeed = walkSpeed;

    }

    private void OnEnable()
    {
        inputActions.Enable();
        runAction.performed += Run;
        runAction.canceled += Run;
        crouchAction.performed += Crouch;
        crouchAction.canceled += Crouch;
    }

    private void Update()
    {
        ReadInput();

        Rotate(moveInput);
        Move(moveInput);
        GroundCheck();
        Animate();
    }

    private void LateUpdate()
    {
        RotateCamera(lookInput);
    }

    private void OnDisable()
    {
        inputActions.Disable();
        runAction.performed -= Run;
        runAction.canceled -= Run;
        crouchAction.performed -= Crouch;
        crouchAction.canceled -= Crouch;
    }
    
    #endregion

    #region Input

    private void ReadInput()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();
    }
    
    private void Run(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            movementSpeed = runSpeed;
        }

        if (context.canceled)
        {
            movementSpeed = walkSpeed;
        }
    }

    private void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouching = true;
            movementSpeed = crouchSpeed;
        }

        if (context.canceled)
        {
            isCrouching = false;
            movementSpeed = walkSpeed;
        }
    }
    #endregion
    
    #region Movement

    private void Rotate(Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            Vector3 inputDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

            Vector3 worldInputDirection = cameraTarget.TransformDirection(inputDirection);
            worldInputDirection.y = 0;
            
            characterTargetRotation = Quaternion.LookRotation(worldInputDirection);
        }

        if (Quaternion.Angle(transform.rotation, characterTargetRotation) > 0.1f)
        {
            transform.rotation =
                Quaternion.Slerp(transform.rotation, characterTargetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = characterTargetRotation;
        }
    }
    
    private void Move(Vector2 moveInput)
    {
        float targetSpeed = moveInput == Vector2.zero ? 0 : movementSpeed * moveInput.magnitude;
        Vector3 currentVelocity = lastMovement;
        currentVelocity.y = 0;
        currentSpeed = currentVelocity.magnitude;

        if (Mathf.Abs(currentSpeed - targetSpeed) > 0.01f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, speedChangeRate * Time.deltaTime);
        }
        else
        {
            currentSpeed = targetSpeed;
        }

        Vector3 targetDirection = characterTargetRotation * Vector3.forward;
        
        Vector3 movement = targetDirection * currentSpeed;
        characterController.SimpleMove(movement);

        if (Physics.Raycast(transform.position + Vector3.up * 0.01f, Vector3.down,
                out RaycastHit hit, raycastLength, raycastMask, QueryTriggerInteraction.Ignore))
        {
            //isGrounded = true;
            if (Vector3.ProjectOnPlane(movement, hit.normal).y < 0)
            {
                characterController.Move(Vector3.down * (pullDownForce * Time.deltaTime));
            }
        }
        //else
        //{
        //    isGrounded = false;
        //}
        
        lastMovement = movement;
    }
    
    #endregion

    #region Animation
    
    private void Animate()
    {
        anim.SetFloat(Hash_MovementSpeed, currentSpeed);
        anim.SetBool(Hash_Grounded, isGrounded);
        anim.SetBool(Hash_Crouching, isCrouching);
    }

    #endregion

    #region GroundCheck

    private void GroundCheck()
    {
        if (characterController.isGrounded)
        {
            airTime = 0;
        }
        else
        {
            airTime += Time.deltaTime;
        }

        isGrounded = airTime < coyoteTime;
    }

    #endregion
    
    #region Camera

    private void RotateCamera(Vector2 lookInput)
    {
        if (lookInput != Vector2.zero)
        {
            bool isMouseLook = IsMouseLook(); 
            
            float deltaTimeMultiplier = isMouseLook ? 1 : Time.deltaTime;

            float sensitivity = isMouseLook ? mouseCameraSensitivity : controllerCameraSensitivity;
            
            /*       
            if (isMouseLook)
            {
                sensitivity = mouseCameraSensitivity;
            }
            else
            {
                sensitivity = controllerCameraSensitivity;
            }
            */

            lookInput *= deltaTimeMultiplier * sensitivity;
            
            cameraRotation.x += lookInput.y * cameraVerticalSpeed * (!isMouseLook && invertY ? -1 : 1);
            cameraRotation.y += lookInput.x * cameraHorizontalSpeed;

            cameraRotation.x = NormalizeAngle(cameraRotation.x);
            cameraRotation.y = NormalizeAngle(cameraRotation.y);

            cameraRotation.x = Mathf.Clamp(cameraRotation.x, verticalCameraRotationMin, verticalCameraRotationMax);
        }
        
        cameraTarget.rotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0);
    }
    
    private float NormalizeAngle(float angle)
    {
        angle %= 360;

        if (angle < 0)
        {
            angle += 360;
        }

        if (angle > 180)
        {
            angle -= 360;
        }

        return angle;
    }

    private bool IsMouseLook()
    {
        if (lookAction.activeControl == null)
        {
            return true;
        }

        return lookAction.activeControl.device.name == "Mouse";
    }

    #endregion
}