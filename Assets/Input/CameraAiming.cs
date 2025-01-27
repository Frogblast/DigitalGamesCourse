using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraAiming : MonoBehaviour
{
    private Vector2 mousePosition = Vector2.zero;

    [SerializeField]
    float mouseSensitivity = 50f;

    private float xRotation, yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        UpdateMousePosition();
        Aim();
    }


    private void Aim()
    {
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    private void UpdateMousePosition()
    {
        mousePosition = Mouse.current.delta.ReadValue();
        xRotation -= mousePosition.y * Time.deltaTime * mouseSensitivity;
        yRotation += mousePosition.x * Time.deltaTime * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
    }
}
