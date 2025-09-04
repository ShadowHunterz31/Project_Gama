using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float gravity = -9.81f;

    [Header("Configurações de Câmera")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;
    private float cameraPitch = 0f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // trava o mouse no centro
        Cursor.visible = false;
    }

    void Update()
    {
        Movimento();
        CameraLook();
    }

    void Movimento()
    {
        float moveX = Input.GetAxis("Horizontal");  // A e D
        float moveZ = Input.GetAxis("Vertical");    // W e S

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float speed = isRunning ? runSpeed : walkSpeed;

        controller.Move(move * speed * Time.deltaTime);

        // Gravidade
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void CameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -75f, 75f);

        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
