using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation
{
    public const float deathTime = 1f;
    private GameObject instance;

    public UnitAnimation(GameObject _instance)
    {
        instance = _instance;
    }


    public IEnumerator DeathAnim()
    {
        float time = 0;

        EffectGenerator.DeathEff(instance);
        
        while (time < deathTime)
        {
            UnitColor.FadeOutUnit(instance, Time.deltaTime / deathTime);

            yield return null;

            time += Time.deltaTime;
        }
    }
}
