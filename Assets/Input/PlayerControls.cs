using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private Vector2 rawInput = Vector2.zero;
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private Transform cameraTransform;

    private Inventory inventory;

    private void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
       // Vector3 movement3d = Vector3.Cross(cameraTransform.forward, new Vector3(rawInput.x, 0,0 )).normalized;
       // Vector3 movement3d = new Vector3(rawInput.x * cameraTransform.forward.x, 0, rawInput.y * cameraTransform.forward.z).normalized;
        Vector3 movement3d = new Vector3(rawInput.x, 0, rawInput.y).normalized;

        transform.position += movement3d * movementSpeed * Time.deltaTime;
        
    }

    public void OnMovement(InputValue value)
    {
        rawInput = value.Get<Vector2>();
    }

    public void OnPickUp(InputValue value)
    {
        if (inventory != null)
        {
            inventory.TryPickUpItem();
        }
    }

    public void OnDrop(InputValue value)
    {
        if (inventory != null)
        {
            inventory.DropItem(transform.position);
        }
    }
}
