using System.Collections.Generic;
using System.IO;
using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;

    // Initialize save location and find InventoryController
    void Awake()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindFirstObjectByType<InventoryController>();
        if (inventoryController == null)
        {
            Debug.LogError("InventoryController is null in SaveController!");
        }
    }

    // Delay loading the game to ensure all scripts are initialized
    void Start()
    {
        StartCoroutine(LoadGameAfterDelay());
    }

    // Coroutine to delay the LoadGame call by one frame
    private IEnumerator LoadGameAfterDelay()
    {
        yield return null;
        LoadGame();
    }

    // Save the game state including player position, map boundary, and inventory
    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            mapBoundary = FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name,
            inventorySaveData = inventoryController.GetInventoryItems() ?? new List<InventoryItemSaveData>(),
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    // Load the game state from the save file
    // If no save file exists, create a new one with default values
    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;

            FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();

            if (inventoryController != null)
            {
                inventoryController.SetInventoryItems(saveData.inventorySaveData);
            }
            else
            {
                Debug.LogError("Cannot load inventory: InventoryController is null!");
            }
        }
        else
        {
            SaveGame();
            if (inventoryController != null)
            {
                inventoryController.SetInventoryItems(new List<InventoryItemSaveData>());
            }
            else
            {
                Debug.LogError("Cannot initialize inventory: InventoryController is null!");
            }
        }
    }
}