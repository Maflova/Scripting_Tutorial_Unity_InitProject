using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class FirstPersonCharacterController : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField] private Camera cam;
    [SerializeField] private float lookSensitivity = 2f;
    [SerializeField] private bool smooth;
    [SerializeField] private float smoothTime = 3f;
    [SerializeField] private float movementSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 3.0f;
    [SerializeField] private float crouchHeight;

    // Looking Vars
    // private float xRotation = 0f;
    private Quaternion m_CharacterTargetRot;
    private Quaternion m_CameraTargetRot;

    // Movement Vars
    private Vector3 velocity;
    private float gravity = -9.81f;
    private bool grounded;

    // Crouch Vars
    private float initHeight;

    // Zoom Vars - Zoom code adapted from @torahhorse's First Person Drifter scripts.
    public float zoomFOV = 35.0f;
    public float zoomSpeed = 9f;
    private float targetFOV;
    private float baseFOV;

   
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        initHeight = controller.height;
        Cursor.lockState = CursorLockMode.Locked;
        SetBaseFOV(cam.fieldOfView);
        m_CharacterTargetRot = transform.localRotation;
        m_CameraTargetRot = cam.transform.localRotation;
        initHeight = controller.height;
    }

    private void Update()
    {
        DoLooking();
        DoMovement();
        DoJump();
        DoCrouch();
        DoZoom();
    }

    private void DoLooking()
    {
        float lookX = Input.GetAxis("Mouse X") * lookSensitivity;
        float lookY = Input.GetAxis("Mouse Y") * lookSensitivity;

        m_CharacterTargetRot *= Quaternion.Euler(0f, lookX, 0f);
        m_CameraTargetRot *= Quaternion.Euler(-lookY, 0f, 0f);

        if (smooth)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, m_CharacterTargetRot,
                smoothTime * Time.deltaTime);
            cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, m_CameraTargetRot,
                smoothTime * Time.deltaTime);
        }
        else
        {
            transform.localRotation = m_CharacterTargetRot;
            cam.transform.localRotation = m_CameraTargetRot;
        }
    }

    private void DoMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        grounded = controller.isGrounded;
        if (grounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = transform.right * moveX + transform.forward * moveY;

        // Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            move *= 2.0f;
        }

        controller.Move(move * movementSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void DoJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
    }

    private void DoCrouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            controller.height = crouchHeight;
        }
        else
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), 2.0f, -1))
            {
                controller.height = crouchHeight;
            }
            else
            {
                controller.height = initHeight;
            }
        }
    }

    private void DoZoom()
    {
        if (Input.GetMouseButton(1))
        {
            targetFOV = zoomFOV;
        }
        else
        {
            targetFOV = baseFOV;
        }
        UpdateZoom();
    }

    private void UpdateZoom()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
    }

    public void SetBaseFOV(float fov)
    {
        baseFOV = fov;
    }
}