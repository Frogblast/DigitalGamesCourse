using UnityEngine;

/*
 * Holds all data about the item but not the actual gameobject itself. This way the data of an item can be stored in the inventory
 * and be used as a recipe when "respawning" the picked up gameobject which was destroyed on pick up.
 * Should be extended with a lot more fields for Renderer, MeshCollider, Material etc. so that the newly spawned item gameobject gets all of
 * the components it originally had.
 */

[CreateAssetMenu(fileName = "NewItem")]
public class Item : ScriptableObject
{
    public string itemName;
    public string description;

    override
    public string ToString()
    {
        return "Item name: " + itemName + ", description: " + description;
    }
}
