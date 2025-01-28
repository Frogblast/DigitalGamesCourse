using UnityEngine;
using UnityEngine.InputSystem;

public class CameraAiming
{
    private Camera camera;
    private float mouseSensitivity;

    public CameraAiming(Camera camera, float mouseSensitivity)
    {
        this.camera = camera;
        this.mouseSensitivity = mouseSensitivity;
    }

    private Vector2 mousePosition = Vector2.zero;
    private float xRotation, yRotation;

    public void Aim()
    {
        camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    public void UpdateMousePosition()
    {
        mousePosition = Mouse.current.delta.ReadValue();
        xRotation -= mousePosition.y * Time.deltaTime * mouseSensitivity;
        yRotation += mousePosition.x * Time.deltaTime * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
    }
}
