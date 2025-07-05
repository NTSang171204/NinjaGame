using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotBarController : MonoBehaviour
{
    public GameObject hotBarPanel;
    public GameObject slotPrefab;
    public int itemSlots = 10;
    private ItemDictionary itemDictionary;
    private Key[] hotBarKeys;


    private void Awake()
    {
        hotBarKeys = new Key[itemSlots];

        itemDictionary = FindFirstObjectByType<ItemDictionary>();

        for(int i = 0; i< itemSlots; i++)
        {
            hotBarKeys[i] = i < 9 ? (Key)((int)Key.Digit1 + i) : Key.Digit0;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        for ( int i = 0; i  < itemSlots; i++)
        {
            if (Keyboard.current[hotBarKeys[i]].wasPressedThisFrame)
            {
                //Use Item function
                UseItemInSlot(i);
            }
        }    
    }

    public void UseItemInSlot(int index)
    {
        Slot slot = hotBarPanel.transform.GetChild(index).GetComponent<Slot>();

        if (slot.currentItem != null)
        {
            Item item = slot.currentItem.GetComponent<Item>();
            item.OnUseItem();
        }
    }


    public List<InventoryItemSaveData> GetHotBarItems()
    {
        // Validate essential components
        //Which mean, crate a new Inventory to save the data of the current inventory
        //By run through every slots in the inventory
        List<InventoryItemSaveData> hotBarData = new List<InventoryItemSaveData>();
        foreach (Transform slotTransform in hotBarPanel.transform)
        {
            //Get the slot at that position to use their properties: currentItem, transform....
            //If the slot is not null and the slot's currentItem is not null = there is an item in the slot
            //Take the item in that slot and add to the new Inventory (which for to use saving the data)
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                if (item != null)
                {
                    hotBarData.Add(new InventoryItemSaveData { itemID = item.itemID, slotIndexPosition = slotTransform.GetSiblingIndex() });
                }
            }
        }
        return hotBarData;
    }

    // Populate the inventory slots with saved items
    // Takes a list of InventoryItemSaveData to restore the inventory state
    public void SetHotBarItems(List<InventoryItemSaveData> hotBarData)
    {
        // Ensure itemDictionary is initialized
            itemDictionary = FindFirstObjectByType<ItemDictionary>();

        // Clear existing slots to avoid duplicates
        foreach (Transform child in hotBarPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Create new slots based on slotCount
        for (int i = 0; i < itemSlots; i++)
        {
            Instantiate(slotPrefab, hotBarPanel.transform);
        }

        // Populate slots with saved items
        foreach (InventoryItemSaveData data in hotBarData)
        {
            if (data.slotIndexPosition < itemSlots)
            {
                Transform slotTransform = hotBarPanel.transform.GetChild(data.slotIndexPosition);
                Slot slot = slotTransform.GetComponent<Slot>();
                if (slot == null)
                {
                    Debug.LogError($"Slot at index {data.slotIndexPosition} does not have a Slot component!");
                    continue;
                }

                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
                else
                {
                    Debug.LogError($"Item with id {data.itemID} is not found in the dictionary, please check.");
                }
            }
            else
            {
                Debug.LogError($"Something got wrong with slot index {data.slotIndexPosition}, please check.");
            }
        }
    }
}
