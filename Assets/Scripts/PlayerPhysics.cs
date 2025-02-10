using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPhysics : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float movementSpeed = 6f;
    [SerializeField] private float jumpForce = 280;
    [SerializeField] private float groundDetectionDistance = 1.3f;

    private Vector2 velocity = Vector2.zero;
    private Rigidbody rb;
    private bool isAlive = true;
    public Vector3 LocalSpace { get; set; } = Vector3.zero;

    private bool IsGrounded()
    { 
        return Physics.Raycast(transform.position, Vector3.down, groundDetectionDistance); ;
    }

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
    }

    private void Update()
    {
        ApplyMovement();
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

    internal void Jump()
    {
        if (IsGrounded())
            rb.AddForce(Vector3.up * jumpForce);
    }
}
