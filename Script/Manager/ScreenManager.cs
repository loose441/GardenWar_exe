using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    private static GameObject startScreen;
    private static GameObject endScreen;

    private void Awake()
    {
        startScreen = GameObject.Find("StartScreen");
        endScreen = GameObject.Find("EndScreen");

        startScreen.SetActive(false);
        endScreen.SetActive(false);
    }

    public static void AwakeStartScreen()
    {
        startScreen.SetActive(true);
    }

    public static void AwakeEndScreen(bool win)
    {
        endScreen.SetActive(true);

        if (win)
        {
            endScreen.transform.Find("WinText").gameObject.SetActive(true);
        }
        else
        {
            endScreen.transform.Find("LoseText").gameObject.SetActive(true);
        }
    }
}
