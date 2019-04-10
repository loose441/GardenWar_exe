using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectGenerator : MonoBehaviour
{
    const string normalHitEff_pass="NormalHit";
    const string fireHitEff_pass = "FireHit";
    const string healEff_pass = "HealEff";
    const string powerUpEff_pass = "PowerUpAura";
    const string deathEff_pass = "DeathEffect";

    private static GameObject Load(string pass)
    {
        return Resources.Load(pass) as GameObject;
    }

    public static void NormalHit(GameObject parent)
    {
        Instantiate(Load(normalHitEff_pass), parent.transform);
    }

    public static void FireHit(GameObject parent)
    {
        Instantiate(Load(fireHitEff_pass), parent.transform);
    }

    public static void HealEff(GameObject parent)
    {
        Instantiate(Load(healEff_pass), parent.transform);
    }

    public static void DeathEff(GameObject parent)
    {
        Instantiate(Load(deathEff_pass), parent.transform);
    }

    public static void PowerUpEff(GameObject parent)
    {
        Instantiate(Load(powerUpEff_pass), parent.transform);
    }
}
