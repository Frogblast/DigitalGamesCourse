using UnityEngine;

[CreateAssetMenu(fileName = "NewItem")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;

    override
    public string ToString()
    {
        return "Item name: " + itemName + ", description: " + description;
    }
}
