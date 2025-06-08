using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController InventoryController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InventoryController = FindFirstObjectByType<InventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            if ()
        }
    }
}
