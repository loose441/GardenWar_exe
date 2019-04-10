using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public void StartButton()
    {
        GameManager.GameStart();
        gameObject.SetActive(false);
    }

    public void QuitButton()
    {
        SceneManager.LoadScene("StartScene");
    }
}
