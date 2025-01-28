using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float movementSpeed = 6f;
    private Vector2 velocity = Vector2.zero;

    public Vector3 LocalSpace { get; set; } = Vector3.zero;

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
}
