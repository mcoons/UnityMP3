using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject[] menuPanels;
    public GameObject[] menuButtons;
    public GameObject[] menuText;
    public GameObject menuButton;

    private Stack<int> menuHistory = new Stack<int>();

    private AudioSource audioSource;

    private UIModeResolver _modeResolver;


    // Start is called before the first frame update
    void Start()
    {
        _modeResolver = FindObjectOfType<UIModeResolver>();

        DisableMenu();
        //EnablePanel(1);
    }

    public void EnablePanel(int panelIndex)
    {
        Debug.Log("Inside EnablePanel");
        //menuButton.SetActive(false);
        menuPanels[0].SetActive(true);
        menuHistory.Push(panelIndex);

        for (int i = 1; i < menuPanels.Length; i++)
            menuPanels[i].SetActive(panelIndex == i);

        LogHistory();
    }

    public void DisableMenu()
    {
        menuPanels[0].SetActive(false);
        //menuButton.SetActive(true);
        menuHistory.Clear();
    }

    public void Back()
    {
        menuPanels[menuHistory.Pop()].SetActive(false);
        EnablePanel(menuHistory.Pop());
    }

    public void LogHistory()
    {
        string list = "";
        foreach (var item in menuHistory)
            list += item.ToString() + " ";

        //Debug.Log("Inside Log History");
    }


    public void ToggleUIMode()
    {
        _modeResolver.ResolveMode();
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}