using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRotation
{
    private const float speed = 3.0f;

    public static IEnumerator RotateUnit(GameObject instance, Vector2Int _lookDir)
    {
        //型変換
        Vector3 lookDir = new Vector3(_lookDir.x, 0, _lookDir.y);
       

        Vector3 rotateAxis = Vector3.Cross(instance.transform.forward, lookDir);
        float rotateAngle = Vector3.Angle(instance.transform.forward, lookDir);

        if (rotateAngle == 180)
            rotateAxis = Vector3.up;
        else if (rotateAngle == 0)
            yield break;


        Quaternion firstDir = instance.transform.rotation;
        Quaternion finalDir = firstDir * Quaternion.AngleAxis(rotateAngle, rotateAxis);

        float time = 0;
        while (time < 1)
        {
            yield return null;
            time += Time.deltaTime * 180 / rotateAngle * speed;

            instance.transform.rotation = Quaternion.Lerp(firstDir, finalDir, time);

        }
    } 
}
