using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPhysics : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float jumpForce = 280;

    private Vector2 velocity = Vector2.zero;
    private Rigidbody rb;
    private bool isGrounded;
    private GroundChecker groundChecker;
    private bool isAlive = true;
    public Vector3 LocalSpace { get; set; } = Vector3.zero;

    [Header ("Inventory")]
    public InventoryScript inventory;

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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundChecker = GetComponentInChildren<GroundChecker>();
    }

    private void Update()
    {
        ApplyMovement();
        CheckIfGrounded();
    }

    private void CheckIfGrounded()
    {
        isGrounded = groundChecker.IsGrounded;
    }

    private void ApplyMovement()
    {
        if (LocalSpace == Vector3.zero) return;

        Vector3 forward = LocalSpace;
        Vector3 right = Vector3.Cross(Vector3.up, forward);

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 direction = forward * velocity.y + right * velocity.x;

        transform.position += direction * movementSpeed * Time.deltaTime;
    }

    internal void ChangeVelocity(Vector2 vector2)
    {
        velocity = vector2;
    }

    internal void Jump(InputValue value)
    {
        if (rb == null) Debug.Log("No rigidbody found");
        if (isGrounded)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        IInventoryItem item = other.GetComponent<IInventoryItem>();
        if (item != null)
        {
            inventory.AddItem(item);
        }
    }

}
