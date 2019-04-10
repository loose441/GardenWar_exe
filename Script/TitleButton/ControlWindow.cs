using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlWindow : MonoBehaviour
{
    public GameObject basePanel;
    public GameObject nextPanel;


    //UIパネルを開く
    public void OpenNextPanel()
    {
        nextPanel.SetActive(true);
        basePanel.SetActive(false);
    }
    
}