using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitColor
{
    public static readonly Color blackColor = Color.black;
    public static readonly Color whiteColor = new Color(0.8f, 0.8f, 0.8f, 1);
    
    //オブジェクトの色を設定
    public static void SetUnitColor(GameObject gameObject, int unitColor)
    {
        Renderer renderer = gameObject.GetComponentInChildren<Renderer>();
        renderer.material.SetColor("unitColor", GetColor(unitColor));
    }

    public static bool FadeOutUnit(GameObject gameObject, float alpha)
    {
        Renderer renderer = gameObject.GetComponentInChildren<Renderer>();
        Color newColor = renderer.material.GetColor("unitColor") - new Color(0, 0, 0, alpha);
        renderer.material.SetColor("unitColor", newColor);

        if (newColor.a <= 0)
            return true;

        return false;
    }

    private static Color GetColor(int colorNum)
    {
        switch (colorNum)
        {
            case (int)UnitTeam.ColorVariety.black:
                return blackColor;
            default:
                return whiteColor;
        }
    }


}
