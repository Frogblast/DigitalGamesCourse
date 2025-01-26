using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraAiming : MonoBehaviour
{
    private Vector2 mousePosition = Vector2.zero;
    private Transform player;
    [SerializeField]
    float mouseSensitivity = 50f;

    private float xRotation, yRotation;

    private void Start()
    {
        player = GetComponentInParent<Transform>();
    }

    void Update()
    {
        UpdateMousePosition();
        Aim();
        player.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }


    private void Aim()
    {
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    private void UpdateMousePosition()
    {
        mousePosition = Mouse.current.delta.ReadValue();
        xRotation += mousePosition.y * Time.deltaTime*mouseSensitivity;
        yRotation += mousePosition.x * Time.deltaTime*mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
    }
}
