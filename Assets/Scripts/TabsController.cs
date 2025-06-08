using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TabsController : MonoBehaviour
{

    public Image[] tabImages;
    public GameObject[] pageImages;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActivePage(0);

    }

    public void ActivePage(int pageNo)
    {
        for(int i = 0; i< pageImages.Length; i++)
        {
            pageImages[i].SetActive(false);
            tabImages[i].color = Color.grey;
        }

        pageImages[pageNo].SetActive(true);
        tabImages[pageNo].color = Color.white;
    }
}
