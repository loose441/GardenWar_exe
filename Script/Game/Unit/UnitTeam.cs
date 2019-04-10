using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTeam
{
    public List<UnitBase> unitList = new List<UnitBase>();
    public int teamColor { get; private set; }
    

    public static readonly List<Vector2Int> forwardDir = new List<Vector2Int>
    {
        Vector2Int.up,
        Vector2Int.down
    };
    public static readonly List<string> teamName = new List<string>
    {
        "Black",
        "White"
    };

    public enum ColorVariety
    {
        black=0,
        white=1
    }


    public UnitTeam(int color)
    {
        teamColor = color;
    }
    
    public void RemoveUnit(UnitBase unit)
    {
        //インデックスを検索
        int index = unitList.IndexOf(unit);

        if (index < 0)
            Debug.Log("error");
        else
            unitList.RemoveAt(index);
    }
    
}
