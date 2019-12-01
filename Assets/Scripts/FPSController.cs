using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public Transform    xRotationTarget;
    [Header("Ground")]
    public Transform    groundPos;
    public LayerMask    groundMask;
    public float        groundCheckSize = 0.2f;
    public float        gravity = -9.8f;
    [Header("Mouse Look")]
    public bool         mouseLookEnable = true;
    public float        mouseSensitivity = 100.0f;
    [Header("Movement")]
    public float        moveSpeed = 4.0f;
    public float        jumpSpeed = 20.0f;

    Vector3             currentRotation;
    CharacterController controller;
    Vector3             velocity;
    bool                isGrounded = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();

        currentRotation = transform.rotation.eulerAngles;
    }

    private void FixedUpdate()
    {
        if (Physics.CheckSphere(groundPos.position, groundCheckSize, groundMask))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        currentRotation.y += mouseX;
        currentRotation.x = Mathf.Clamp(currentRotation.x - mouseY, -70.0f, 70.0f);

        transform.rotation = Quaternion.Euler(0.0f, currentRotation.y, 0.0f);
        xRotationTarget.localRotation = Quaternion.Euler(currentRotation.x, 0.0f, currentRotation.z);

        Vector3 right_axis = transform.right; right_axis.y = 0.0f; right_axis.Normalize();
        Vector3 forward_axis = transform.forward; forward_axis.y = 0.0f; forward_axis.Normalize();

        Vector3 moveDir = (right_axis * Input.GetAxis("Horizontal") +
                           forward_axis * Input.GetAxis("Vertical")) * Time.deltaTime * moveSpeed;

        controller.Move(moveDir);

        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = jumpSpeed;
            }

            if (velocity.y < 0.0f)
            {
                velocity.y = 0.0f;
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(groundPos.position, groundCheckSize);
    }
}
