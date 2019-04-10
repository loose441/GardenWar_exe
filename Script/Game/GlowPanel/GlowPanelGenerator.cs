using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowPanelGenerator : MonoBehaviour
{
    private const string prefabPass = "GlowPanel";

    public static GameObject InstantiateGlowPanel(Vector3 setPos)
    {
        //高さ調整
        setPos.y = 0.96f;
        GameObject prefab = Instantiate(Resources.Load(prefabPass), setPos, Quaternion.identity) as GameObject;
        return prefab;
    }
}
