using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraAiming : MonoBehaviour
{
    private Vector2 mousePosition = Vector2.zero;
    [SerializeField]
    private Transform player;
    [SerializeField]
    float mouseSensitivity = 50f;

    private float xRotation, yRotation;

    void Update()
    {
        UpdateMousePosition();
        Aim();
        //player.Rotate(Vector3.up*xRotation);
    }


    private void Aim()
    {
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    private void UpdateMousePosition()
    {
        mousePosition = Mouse.current.delta.ReadValue();
        xRotation -= mousePosition.y * Time.deltaTime*mouseSensitivity;
        yRotation += mousePosition.x * Time.deltaTime*mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
    }
}
