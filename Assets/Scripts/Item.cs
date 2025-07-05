using UnityEngine;
using UnityEngine.UI;


public class Item : MonoBehaviour
{
    public int itemID;
    public string itemName;

    public virtual void OnPickUp()
    {
        Sprite itemIcon = GetComponent<Image>().sprite;
        if (ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowPopupNotification(itemName, itemIcon);
        }
    }
    public virtual void OnUseItem()
    {
        Debug.Log("Using" + itemName);
    }
}
