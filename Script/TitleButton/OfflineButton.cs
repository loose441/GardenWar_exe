﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfflineButton : MonoBehaviour
{
    public void PlayOffLine()
    {
        SceneManager.LoadScene("GameScene");
    }
}
