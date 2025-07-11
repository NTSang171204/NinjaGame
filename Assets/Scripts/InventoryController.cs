using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

    // Initialize the inventory controller and find the ItemDictionary
    void Start()
    {
        itemDictionary = FindFirstObjectByType<ItemDictionary>();
        if (itemDictionary == null)
        {
            Debug.LogError("ItemDictionary not found in the scene! Please ensure an ItemDictionary component exists.");
            return;
        }

        if (inventoryPanel == null)
        {
            Debug.LogError("InventoryPanel is not assigned in the Inspector!");
            return;
        }

        if (slotPrefab == null)
        {
            Debug.LogError("SlotPrefab is not assigned in the Inspector!");
            return;
        }
    }

    //Add items to the inventory
    public bool AddItem(GameObject itemPrefab)
    {   
        //Run through every slots's position (transform) in inventory
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            //Get the slot at that position to use their properties: currentItem, transform....
            Slot slot = slotTransform.GetComponent<Slot>();
            //If the slot is not null and the slot's currentItem is null = there is no item in the slot
            if (slot != null && slot.currentItem == null)
            {
                //Create a new item in the slot, which is the item we just picked up
                GameObject newItem = Instantiate(itemPrefab, slot.transform);
                slot.currentItem = newItem; //Set the slot's current item is the item we just picked up
                return true; //Return the function is true to show everything went fine
            }
        }
        //Else debug the situation that the inventory is full (there's no slots left).
        Debug.Log("Inventory's full!");
        return false;
    }

    // Retrieve all items currently in the inventory slots
    // Returns a list of InventoryItemSaveData for saving
    public List<InventoryItemSaveData> GetInventoryItems()
    {
        // Validate essential components
        //Which mean, crate a new Inventory to save the data of the current inventory
        //By run through every slots in the inventory
        List<InventoryItemSaveData> invData = new List<InventoryItemSaveData>();
        foreach (Transform slotTransform in inventoryPanel.transform)
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
                    invData.Add(new InventoryItemSaveData { itemID = item.itemID, slotIndexPosition = slotTransform.GetSiblingIndex() });
                }
            }
        }
        return invData;
    }

    // Populate the inventory slots with saved items
    // Takes a list of InventoryItemSaveData to restore the inventory state
    public void SetInventoryItems(List<InventoryItemSaveData> invData)
    {
        // Validate essential components
        if (inventoryPanel == null)
        {
            Debug.LogError("InventoryPanel is null!");
            return;
        }

        if (slotPrefab == null)
        {
            Debug.LogError("SlotPrefab is null!");
            return;
        }

        // Ensure itemDictionary is initialized
        if (itemDictionary == null)
        {
            Debug.LogWarning("ItemDictionary is null! Attempting to find it...");
            itemDictionary = FindFirstObjectByType<ItemDictionary>();
            if (itemDictionary == null)
            {
                Debug.LogError("ItemDictionary not found in the scene! Please ensure an ItemDictionary component exists.");
                return;
            }
        }

        // Clear existing slots to avoid duplicates
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Create new slots based on slotCount
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        // Populate slots with saved items
        foreach (InventoryItemSaveData data in invData)
        {
            if (data.slotIndexPosition < slotCount)
            {
                Transform slotTransform = inventoryPanel.transform.GetChild(data.slotIndexPosition);
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