using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;
    public Dictionary<int, GameObject> itemDictionary;

    // Initialize the item dictionary with prefabs on awake
    void Awake()
    {
        // Check if itemPrefabs is null to avoid errors
        if (itemPrefabs == null)
        {
            Debug.LogError("ItemPrefabs list is null in ItemDictionary!");
            itemPrefabs = new List<Item>();
        }

        itemDictionary = new Dictionary<int, GameObject>();

        // Assign incremental IDs to items in the list
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                itemPrefabs[i].itemID = i + 1;
            }
            else
            {
                Debug.LogWarning($"Item at index {i} in itemPrefabs is null!");
            }
        }

        // Populate the dictionary with item IDs and their corresponding GameObjects
        foreach (Item item in itemPrefabs)
        {
            if (item != null)
            {
                itemDictionary[item.itemID] = item.gameObject;
            }
            else
            {
                Debug.LogWarning("Skipping null item in itemPrefabs");
            }
        }
    }

    // Retrieve a prefab from the dictionary based on its itemID
    // Returns null if the itemID is not found
    public GameObject GetItemPrefab(int itemID)
    {
        if (itemDictionary == null)
        {
            Debug.LogError("ItemDictionary is null in GetItemPrefab!");
            return null;
        }

        if (itemDictionary.TryGetValue(itemID, out GameObject itemPrefab))
        {
            return itemPrefab;
        }
        else
        {
            Debug.LogError($"Item with id {itemID} is not found in the dictionary, please check.");
            return null;
        }
    }
}