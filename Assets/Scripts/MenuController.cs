using UnityEngine;

public class MenuController : MonoBehaviour
{
    //By default, the menu is hidden
    //Only appear and disappear when player press tab
    public GameObject menuCanvas;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }
}
