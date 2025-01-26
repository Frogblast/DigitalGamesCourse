using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private Vector2 rawInput = Vector2.zero;
    [SerializeField] private float movementSpeed = 10f;

    private Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        Vector3 movement3d = new Vector3(rawInput.x, 0, rawInput.y);
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
