using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    private Vector3 Velocity;
    private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouseInput;
    private bool Sneaking = false;
    private float xRotation;

    [Header("Components Needed")]
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] private CharacterController Controller;
    [SerializeField] private Transform Player;

    [Space]
    [Header("Movement")]
    [SerializeField] private float Speed = 5f;
    [SerializeField] private float JumpForce = 5f;
    [SerializeField] private float Sensitivity = 2f;
    [SerializeField] private float Gravity = 9.81f;

    [Space]
    [Header("Sneaking")]
    [SerializeField] private float SneakSpeed = 2f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        PlayerMovementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        HandleMovement();
        HandleCamera();
    }

    private void HandleMovement()
    {
        Vector3 moveVector = transform.TransformDirection(PlayerMovementInput);

        if (Controller.isGrounded)
        {
            Velocity.y = -1f;

            if (Input.GetKeyDown(KeyCode.Space) && !Sneaking)
            {
                Velocity.y = JumpForce;
            }
        }
        else
        {
            Velocity.y -= Gravity * Time.deltaTime;
        }

        Vector3 move = moveVector * (Sneaking ? SneakSpeed : Speed);
        move += Velocity;

        Controller.Move(move * Time.deltaTime);
    }

    private void HandleCamera()
    {
        xRotation -= PlayerMouseInput.y * Sensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(0f, PlayerMouseInput.x * Sensitivity, 0f);
        PlayerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
