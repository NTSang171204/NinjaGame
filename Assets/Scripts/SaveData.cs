using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    //Saving data: Player's position, the mapBoundary that user are in.
    public Vector3 playerPosition;
    //Saving the map's name, because PolygonCollider2D is not serializable
    public string mapBoundary;

    public List<InventoryItemSaveData> inventorySaveData;
    public List<InventoryItemSaveData> hotBarSaveData;
}
