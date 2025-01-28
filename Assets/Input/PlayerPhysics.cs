using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPhysics : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    private Vector2 velocity = Vector2.zero;

    private void Update()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        Vector3 velocity3D = new Vector3(velocity.x, 0, velocity.y);

        transform.position += velocity3D.normalized * movementSpeed * Time.deltaTime;
    }

    internal void ChangeVelocity(Vector2 vector2)
    {
        velocity = vector2;
    }
}
