using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIModeResolver : MonoBehaviour
{
    private const float DarkGrey = 0.30f;
    private const float MedGrey = 0.70f;
    private readonly Color _darkGrey = new Color(DarkGrey, DarkGrey, DarkGrey, 1);
    private readonly Color _medGrey = new Color(MedGrey, MedGrey, MedGrey, 1);

    public GameObject modeText;

    MenuManager menuManager;

    private void Start()
    {
        menuManager = GetComponent<MenuManager>();
    }

    public void ResolveMode()
    {
        modeText.GetComponent<TextMeshProUGUI>().text = modeText.GetComponent<TextMeshProUGUI>().text == "Dark Mode" ? "Light Mode" : "Dark Mode";
        

        foreach (var go in GameObject.FindGameObjectsWithTag("AudioPanel"))
        {
            go.GetComponent<Image>().color = go.GetComponent<Image>().color == Color.white ? _darkGrey : Color.white;
        }

        foreach (var go in GameObject.FindGameObjectsWithTag("AudioButton"))
        {
            go.GetComponent<Image>().color = go.GetComponent<Image>().color == Color.white ? _medGrey : Color.white;
        }


        foreach (var go in menuManager.menuPanels)
        {
            if (go.name != "MenuContainer")
            {
                go.GetComponent<Image>().color = go.GetComponent<Image>().color == Color.white ? _darkGrey : Color.white;
            }
        }

        foreach (var go in menuManager.menuButtons)
        {
            go.GetComponent<Image>().color = go.GetComponent<Image>().color == Color.white ? _medGrey : Color.white;
        }

        foreach (var go in menuManager.menuText)
        {
            Debug.Log(go.name);
            go.GetComponent<TextMeshProUGUI>().color = go.GetComponent<TextMeshProUGUI>().color == Color.white ? Color.black : Color.white;
        }

    }
}
