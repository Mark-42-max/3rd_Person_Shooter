using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchAimCam : MonoBehaviour
{

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private InputAction aimAction;
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private int priorityBoost = 10;

    [SerializeField] private Canvas thirdPersonCanvas;
    [SerializeField] private Canvas aimCanvas;

    public bool isAimed { get; private set; }
    //private bool isPrioritized = false;

    private void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];
    }

    private void Start()
    {
        thirdPersonCanvas.enabled = true;
        aimCanvas.enabled = false;
        isAimed = false;

        Cursor.lockState = CursorLockMode.Locked;
    }

    //private void Update()
    //{
    //    if (aimAction.IsPressed())
    //    {
    //        if (isPrioritized)
    //        {
    //            CancelAim();
    //        }
    //        else
    //        {
    //            StartAim();
    //        }
    //    }
    //}

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }

    private void StartAim()
    {
        //isPrioritized = true;
        vcam.Priority += priorityBoost;
        thirdPersonCanvas.enabled = false;
        aimCanvas.enabled = true;
        isAimed = true;
    }

    private void CancelAim()
    {
        //isPrioritized = false;
        vcam.Priority -= priorityBoost;
        thirdPersonCanvas.enabled = true;
        aimCanvas.enabled = false;
        isAimed = false;
    }
}
