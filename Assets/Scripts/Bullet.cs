using UnityEngine;

public class Bullet : DamageBase
{
    [SerializeField] private int damageNrValue = 20; // Allows damageNr to be viewd through the inspector
    [SerializeField]public override int damageNr => damageNrValue; // Damage towards player

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lifetimeSeconds = 5;

    private Vector3 direction;
    private bool isMoving = false;

    public void Move(Vector3 newDirection)
    {
        direction = newDirection.normalized;
        isMoving = true;
        Destroy(gameObject, lifetimeSeconds);
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;
        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other) // In collision with player
    {
        ApplyDamage(other.gameObject);
    }

}
