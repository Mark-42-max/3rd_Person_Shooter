using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;


    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float sprintSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float sprintJumpHeight = 3.0f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private float rotateSpeed = 0.8f;

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction shootAction;

    private Transform cameraTransform;

    private SwitchAimCam camScript;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        sprintAction = playerInput.actions["Sprint"];
        shootAction = playerInput.actions["Shoot"];

        camScript = GameObject.Find("Aim Cinemachine").GetComponent<SwitchAimCam>();
    }

    private void OnEnable()
    {
        shootAction.performed += _ => ShootBullet();
    }

    private void OnDisable()
    {
        shootAction.performed -= _ => ShootBullet();
    }

    private void ShootBullet()
    {
        RaycastHit hit;
        if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }

    void Update()
    {
        //check if grounded
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //movement
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0;

        if(input != Vector2.zero && sprintAction.IsPressed())
        {
            //rotate

            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, cameraTransform.eulerAngles.y, 0));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed);
            controller.Move(playerSpeed * sprintSpeed * Time.deltaTime * move);
        }
        else if(input != Vector2.zero)
        {
            //rotate

            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, cameraTransform.eulerAngles.y, 0));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed);
            controller.Move(playerSpeed * Time.deltaTime * move);
        }
        else if (camScript.isAimed)
        {
            //rotate

            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, cameraTransform.eulerAngles.y, 0));
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed);
            controller.Move(playerSpeed * Time.deltaTime * move);
        }


        // jump
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);

        }
        
        if(jumpAction.triggered && groundedPlayer && sprintAction.IsPressed())
        {
            playerVelocity.y += Mathf.Sqrt(sprintJumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}