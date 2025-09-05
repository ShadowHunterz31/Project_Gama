using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]

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

    [Header("Sons de Passos")]
    public AudioClip walkStep;
    public AudioClip runStep;
    public float stepInterval = 0.5f; // intervalo entre passos andando
    public float runStepInterval = 0.3f; // intervalo entre passos correndo

    private CharacterController controller;
    private Vector3 velocity;
    private AudioSource audioSource;
    private float stepTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Movimento();
        CameraLook();
        Footsteps();
    }

    void Movimento()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

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

    void Footsteps()
    {
        // pega input do jogador
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        bool isMoving = new Vector3(moveX, 0, moveZ).magnitude > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        if (controller.isGrounded && isMoving)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                audioSource.clip = isRunning ? runStep : walkStep;
                audioSource.PlayOneShot(audioSource.clip);

                stepTimer = isRunning ? runStepInterval : stepInterval;
            }
        }
        else
        {
            stepTimer = 0f; // reseta quando para
        }
    }
}
