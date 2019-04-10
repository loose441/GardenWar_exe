using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGenerator : MonoBehaviour
{
    public static GameObject InstantiatePrefab(string prefabName, Vector3 position)
    {
        GameObject prefab = Instantiate(Resources.Load(prefabName), position, Quaternion.identity) as GameObject;

        //回転調整
        //prefab.transform.Rotate(-90, 0, 0);

        return prefab;
    }
}
