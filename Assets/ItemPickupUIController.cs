
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickupUIController : MonoBehaviour
{
    //This script using Instance so we can call it everywhere in the app
    public static ItemPickupUIController Instance { get; private set; }

    public GameObject popupPrefab;
    public int maxPopups = 5;
    public float popupDuration;

    private readonly Queue<GameObject> activePopups = new();

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        } else
        {
            Debug.LogError("Multiple Item pick up UI manager instances detected! Destroy the extra one");
            Destroy(gameObject);
        }
    }

    public void ShowPopupNotification(string itemName, Sprite itemIcon)
    {
        GameObject itemPopup = Instantiate(popupPrefab, this.transform);
        itemPopup.GetComponentInChildren<TMP_Text>().text = itemName;
        Image itemImage = itemPopup.transform.Find("ItemIcon")?.GetComponent<Image>();
        if (itemImage)
        {
            itemImage.sprite = itemIcon;
        }
        activePopups.Enqueue(itemPopup);
        if(activePopups.Count > maxPopups)
        {
            Destroy(activePopups.Dequeue());
        }

        //Fade out and destroy popup
        StartCoroutine(FadeOutAndDestroyPopup(itemPopup));
    }

    //Fade out and Destroy Popup, using CanvasGroup and Coroutine
    private IEnumerator FadeOutAndDestroyPopup(GameObject itemPopup)
    {
        yield return new WaitForSeconds(popupDuration);
        if (itemPopup == null) yield break;

        CanvasGroup canvasGroup = itemPopup.GetComponent<CanvasGroup>();
        for (float timePassed = 0f; timePassed < 1f; timePassed += Time.deltaTime)
        {
            if (itemPopup == null) yield break;

            canvasGroup.alpha = 1f - timePassed;
            yield return null;
        }
        Destroy(itemPopup);
    }
}
