using UnityEngine;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public Transform inventoryUIParent;
    public GameObject inventorySlotPrefab;

    private void Awake()
    {
        Instance = this;
    }

    /*
    public void AddItem(itemdata item)
    {
        GameObject slot = Instantiate(inventorySlotPrefab, inventoryUIParent);
        slot.GetComponent<InventorySlot>().Set(item);
    }
    */
}

