using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playertest : MonoBehaviour
{

    public Rigidbody rb;

    public Transform playerBody;

    public new Transform camera;

    private Vector3 direction;


    public float groundDrag;

    [Header ("Ground Check")]
    public float playerHeight;
    private bool grounded;
    public LayerMask whatIsGrounded;
    







    public float mouseSensitivity = 0.5f;

    public float moveSpeed = 6f;

    public float jumpForce = 10f;


    private float xRotation = 0f;



    public Kontroll kontroll;

    private InputAction move;
    private Vector2 moveInput
        ;
    private InputAction mouse;
    private Vector2 mouseInput;

    private InputAction jump;
  



    // Start is called before the first frame update
    void Awake()
    {
        kontroll = new Kontroll();

        mouse = kontroll.Player.Look;
        move  = kontroll.Player.PlayerMovement;
        jump  = kontroll.Player.Jump;
    }

    private void OnEnable()
    {
        mouse.Enable();
        move.Enable();
        jump.Enable();
    }
    private void OnDisable()
    {
        mouse.Disable();
        move.Disable();
        jump.Disable();
    }


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }



    private void moveThePlayer()
    {
        moveInput = move.ReadValue<Vector2>();

        direction = transform.right * moveInput.x + transform.forward * moveInput.y;


        rb.AddForce(direction * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
    }


    private void rotateThePlayer()
    {
        mouseInput = mouse.ReadValue<Vector2>();

        // xRotation lets the player look up and down
        xRotation -= mouseInput.y * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90); // We restrict the "look up/down" to 90 degrees

        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotate camera around the x-axis
        playerBody.Rotate(Vector3.up * mouseInput.x * mouseSensitivity); // Rotate body around the z-axis
    }


    private void Update()
    {
        // groundcheck
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGrounded);

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rotateThePlayer();

        moveThePlayer();

 


    }
}
