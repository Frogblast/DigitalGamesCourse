using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


// This script is the only script that needs to be on a player for inventory
public class PlayerInventoryScript : MonoBehaviour
{


    private int hotbarSelected = 0;
    private float distance = 3f;
    public GameObject pickupText;
    [SerializeField] private GameObject[] hotbarslots = new GameObject[5]; // Inventory size and contains all UI slot gameobjects

    [SerializeField] private InventoryScript inventoryscript; // For inventory logic
    [SerializeField] private HUD toolbar; // For UI

    private AudioManager audiomanager => AudioManager.Instance;


    public void OnHotbar_1(InputAction.CallbackContext context)
    {
        hotbarSelected = 0; // Update what slot is selected
        hotbarChangeItem(); // Update UI
    }

    public void OnHotbar_2(InputAction.CallbackContext context)
    {
        hotbarSelected = 1;
        hotbarChangeItem();
    }

    public void OnHotbar_3(InputAction.CallbackContext context)
    {
        hotbarSelected = 2;
        hotbarChangeItem();
    }

    public void OnHotbar_4(InputAction.CallbackContext context)
    {
        hotbarSelected = 3;
        hotbarChangeItem();
    }

    public void OnHotbar_5(InputAction.CallbackContext context)
    {
        hotbarSelected = 4;
        hotbarChangeItem();
    }


    private void hotbarChangeItem() // Changes the UI to convey what item the player is "holding"
    {
        inventoryscript.hotbarSelected = hotbarSelected;

        foreach (GameObject slot in hotbarslots) // Goes through all slots (Actually borders)
        {
            Vector3 scale;
            Color color;

            if (slot == hotbarslots[hotbarSelected])
            {
                scale = new Vector3(1.1f, 1.1f, 1.1f);
                color = new Color(75f/255, 204f/255, 110f/255,1f); // show border with a green color
            }
            else
            {
                scale = new Vector3(1f, 1f, 1f);
                color = new Color(1f,1f,1f,0f); // Make border fully transparent
            }
            slot.transform.localScale = scale; //apply
            slot.GetComponent<Image>().color = color; //apply

        }

    }

    public void OnDropItem(InputAction.CallbackContext context) // Drops item in inventory with "Q"
    {
        if (context.started && !inventoryscript.slotIsEmpty(hotbarSelected)) // Checks that the inventoryslot is non-empty
        {
            // Updates what slot is selected to the other scripts
            toolbar.selectedSlot = hotbarSelected;
            inventoryscript.hotbarSelected = hotbarSelected;

            inventoryscript.DropItem(hotbarSelected); // Calls the for the inventory to drop the item in the equipped slot
        }

    }

    public void OnPickUp(InputAction.CallbackContext context) // Pick up object on "E"
    {
        rayItemPickup();
    }


    private void rayItemPickup() // Spawns one ray and if it hits a pickupable object, it adds it to the inventory
    {
        Vector3 rpos = Camera.main.transform.position;
        Vector3 rdir = Camera.main.transform.forward;
        RaycastHit hit;

        Ray ray = new Ray(rpos, rdir);

        if (Physics.Raycast(ray, out hit, distance))
        {
            
            IInventoryItem item = hit.collider.GetComponent<IInventoryItem>();
            if (item != null)
            {
                inventoryscript.AddItem(item);
                pickupText.SetActive(false);
                audiomanager.Play("PickupSound");
            }

        }

    }


    private void DebugRay() // Make the ray visible for debugging
    {
        Vector3 rpos = Camera.main.transform.position;
        Vector3 rdir = Camera.main.transform.forward;
        Ray ray = new Ray(rpos, rdir);

        Debug.DrawRay(rpos, rdir * distance, Color.yellow, 0.1f);
    }

    private void DisplayPickUptxt() // Displays a text, only if the ray hits a pickupable object
    {
        Vector3 rpos = Camera.main.transform.position;
        Vector3 rdir = Camera.main.transform.forward;
        RaycastHit hit = new RaycastHit();
        Ray ray = new Ray(rpos, rdir);

        if (Physics.Raycast(ray, out hit, distance))
        {
            IInventoryItem item = hit.collider.GetComponent<IInventoryItem>();
            if (item != null)
            {
                pickupText.SetActive(true);
            }
            else
            {
                pickupText.SetActive(false);
            }
        }
        else
        {
            pickupText.SetActive(false);
        }
    }



    private void Update()
    {
        //DebugRay;
        DisplayPickUptxt();
    }
   


}
