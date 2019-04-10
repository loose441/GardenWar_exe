using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioButton : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject unitPanel;
    public GameObject levelUpPanel;
    public GameObject materialPanel;
    private static GameObject currentPanel;

    private void Start()
    {
        currentPanel = mainPanel;
    }

    public void OpenMain()
    {
        currentPanel.SetActive(false);
        mainPanel.SetActive(true);
        currentPanel = mainPanel;
    }

    public void OpenUnit()
    {
        currentPanel.SetActive(false);
        unitPanel.SetActive(true);
        currentPanel = unitPanel;
    }

    public void OpenLevelUp()
    {
        currentPanel.SetActive(false);
        levelUpPanel.SetActive(true);
        currentPanel = levelUpPanel;
    }

    public void OpenMaterialPanel()
    {
        currentPanel.SetActive(false);
        materialPanel.SetActive(true);
        currentPanel = materialPanel;
    }
}
