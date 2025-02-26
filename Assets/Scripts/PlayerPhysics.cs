using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPhysics : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float walkSpeed = 4.5f;
    [SerializeField] private float sprintSpeed = 6.5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float tapJumpForce = 3f;
    [SerializeField] private float groundDetectionDistance = 1.3f;
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float airborneSteeringDampening = 0.4f;


    private Vector2 velocity = Vector2.zero;
    private Vector3 airborneVelocity = Vector3.zero;
    private Rigidbody rb;
    private bool isAlive = true;
    private CameraAnimationHandler CameraAnimationHandler;

    public Vector3 LocalSpace { get; set; } = Vector3.zero;
    public bool IsSprinting { get; internal set; } = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        CameraAnimationHandler = GetComponentInChildren<CameraAnimationHandler>();
        rb.freezeRotation = true;
    }

    private bool IsGrounded()
    {
        Vector3 origin = transform.position;
        Vector3 endPoint = transform.position + Vector3.down * groundDetectionDistance;
        Debug.DrawLine(origin, endPoint, Color.blue);

        return Physics.Raycast(origin, Vector3.down, groundDetectionDistance);
    }


    /*[Header ("Inventory")]
    public InventoryScript inventory;
    */
    private void OnEnable()
    {
        EventManager.OnPlayerDeath += KillPlayer;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerDeath -= KillPlayer;
    }

    private void KillPlayer()
    {
        if (!isAlive) return;
        isAlive = false;
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void Update()
    {
        ApplyCameraAnimation();
    }


    private void ApplyCameraAnimation()
    {
        if (Mathf.Abs(rb.velocity.y) > float.Epsilon)
        {
            CameraAnimationHandler.SetAnimationState(CameraAnimationHandler.State.Airborne);
            return;
        }

        if (Mathf.Abs(rb.velocity.x) > float.Epsilon || Mathf.Abs(rb.velocity.z) > float.Epsilon)
        {
            CameraAnimationHandler.SetAnimationState(CameraAnimationHandler.State.Walking);
            return;
        }

        CameraAnimationHandler.SetAnimationState(CameraAnimationHandler.State.Idling);
    }

    private void ApplyMovement()
    {
        if (LocalSpace == Vector3.zero) return; // Don't apply movement if there is no updated direction

        float targetSpeed = IsSprinting ? sprintSpeed : walkSpeed; // Set the actual speed to be applied according to the IsSprinting bool
        float maxSpeed = targetSpeed;

        // Find what is "forward" (and sideways "right") from the player's perspective
        Vector3 forward = LocalSpace;
        Vector3 right = Vector3.Cross(Vector3.up, forward);
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * velocity.y + right * velocity.x).normalized;
        Vector3 targetVelocity = moveDirection * targetSpeed;

        Vector3 currentVelocity = rb.velocity;
    //    currentVelocity.y = 0; // Ignore y-velocity since we are moving horizontally

        // find the direction of the new input
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = Vector3.ClampMagnitude(velocityChange, acceleration * Time.fixedDeltaTime); // for smoother movement

        // Handle reduced effectivity of new input when airborne
        if (!IsGrounded() && targetVelocity != airborneVelocity)
        {
            velocityChange *= airborneSteeringDampening;
        }
        else if (IsGrounded())
        {
            airborneVelocity = targetVelocity;
        }

        // Apply the actual force (horizontal only)
        rb.AddForce(new Vector3(velocityChange.x, 0, velocityChange.z), ForceMode.VelocityChange);

        // Cap horizontal speed if larger than maxSpeed
        Vector3 horizontalVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            rb.velocity = horizontalVelocity.normalized * maxSpeed;
            rb.velocity += Vector3.up * rb.velocity.y; // restore the y velocity for gravity
        }
    }


    internal void ChangeVelocity(Vector2 vector2)
    {
        velocity = vector2;
    }

    internal void Jump(InputAction.CallbackContext context)
    {
        if (context.started && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // To always jump the same height
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        bool tapped = context.interaction is UnityEngine.InputSystem.Interactions.TapInteraction;

        if (context.performed && tapped) // If tapping - cut the velocity in y direction
        {
            float tapJumpMultiplier = 0.6f;
            if (rb.velocity.y > 0) 
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * tapJumpMultiplier, rb.velocity.z);
        }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        IInventoryItem item = other.GetComponent<IInventoryItem>();
        if (item != null)
        {
            inventory.AddItem(item);
        }
    }*/

}
