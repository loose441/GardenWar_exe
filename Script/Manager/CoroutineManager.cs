using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager singleton;

    private void Awake()
    {
        singleton = this;
    }
}
