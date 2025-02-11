using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController1 : MonoBehaviour
{
    public Rigidbody rb;

    public Transform orientation; // where the player is currently looking

    public new Transform camera;

    private Vector3 playerDirection;

    [Header ("Mouse movement")]
    public float mouseSensitivity = 0.5f;

    [Header("Movement")]
    public float moveSpeed;
    public float moveSpeedMutliplier;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump = true;

    [Header("Ground Check")]
    public float playerHeight;
    public bool grounded;
    public LayerMask whatIsGrounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;

    public float groundDrag;

    private float xRotation = 0f;
    private float yRotation = 0f;


    public Kontroll kontroll;

    private InputAction move;
    private Vector2 moveInput
        ;
    private InputAction mouse;
    private Vector2 mouseInput;

    private InputAction jump;



    void Awake()
    {
        kontroll = new Kontroll();

        mouse = kontroll.Player.Look;
        move = kontroll.Player.PlayerMovement;
        jump = kontroll.Player.Jump;
    }

    private void OnEnable()
    {
        mouse.Enable();
        move.Enable();
        jump.Enable();

        jump.performed += Jump;
    }
    private void OnDisable()
    {
        mouse.Disable();
        move.Disable();
        jump.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true;
        
    }

    private void rotateThePlayer()
    {
        mouseInput = mouse.ReadValue<Vector2>();

        // xRotation lets the player look up and down
        xRotation -= mouseInput.y * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90); // We restrict the "look up/down" to 90 degrees
        yRotation += mouseInput.x * mouseSensitivity;


        camera.rotation = Quaternion.Euler(xRotation,yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        /*
        camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotate camera around the x-axis

        camera.rotation = Quaternion.Euler(0f, mouseInput.x * mouseSensitivity, 0f);
        playerBody.Rotate(Vector3.up * mouseInput.x * mouseSensitivity); // Rotate body around the y-axis
        */
    }

    private void moveThePlayer()
    {
        moveInput = move.ReadValue<Vector2>();
        playerDirection = orientation.right * moveInput.x + orientation.forward * moveInput.y;


        // Let player walk smoothly on a slope
        if (OnSlope())
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * moveSpeedMutliplier * Time.deltaTime, ForceMode.VelocityChange);

          /*  if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }*/
        }


        // Move player depending if player is on ground or in air
        if (grounded)
        {
            rb.AddForce(playerDirection * moveSpeed * moveSpeedMutliplier * Time.deltaTime, ForceMode.VelocityChange);
        }
        else if (!grounded)
        {
            rb.AddForce(playerDirection * moveSpeed * moveSpeedMutliplier * Time.deltaTime, ForceMode.VelocityChange);
        }

        // Turn off gravity while on slopes (prevents sliding)
        //rb.useGravity = !OnSlope();
        
    }

    // Check if player is standing on a walkable slope (not too steep)
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, 100f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal); // Calculate the angle of the slope we're standing on
            Debug.Log(angle);
            Debug.DrawLine(transform.position, Vector3.down * 100f, Color.yellow);
            return angle < maxSlopeAngle && angle != 0; // Return true if the angle were standing on is less than maxSlopeAngle and not 0
        }
        return false;
    }

    // Retrieve direction perpendicular to the slope
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(playerDirection, slopeHit.normal).normalized;
    }




    private void setDragIfGrounded()
    {
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

    private void SpeedControl()
    {

            Vector3 flatvelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit veolocity if needed
            if (flatvelocity.magnitude > moveSpeed)
            {
                Vector3 limitedvelocity = flatvelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedvelocity.x, rb.velocity.y, limitedvelocity.z);
            }
    }
    
    
    private void Jump(InputAction.CallbackContext context)
    {
        if (readyToJump && grounded)
        {
            readyToJump = false;

            // Reset y velocity so the jump is always the same height
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            Invoke("ResetJump", jumpCooldown); // Activates the jumpcooldown

        }
    }
    private void ResetJump()
    {
        readyToJump = true;
    }


    private void Update()
    {
        setDragIfGrounded();
        rotateThePlayer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveThePlayer();
        SpeedControl();
    }
    
}
