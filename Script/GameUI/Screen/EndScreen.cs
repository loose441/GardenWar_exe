using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public void AgainButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitButton()
    {
        SceneManager.LoadScene("StartScene");
    }

}
