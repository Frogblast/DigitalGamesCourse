using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform cameraTarget; // The target object to follow
    public float smoothTime = 0.1f; // Adjust for smoothness
    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        Debug.Log("CameraTarget Position: " + cameraTarget.position);

        if (cameraTarget == null) return;
        transform.position = Vector3.SmoothDamp(transform.position, cameraTarget.position, ref velocity, smoothTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraTarget.rotation, Time.deltaTime * 10f);
    }


}
