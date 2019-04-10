using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell
{
    //セルの位置
    public Vector2Int cellPosition { get; private set; }
    //ユニットを設置する座標
    public Vector3 unitSetPosition { get; private set; }

    //このセルに存在するユニット
    public UnitBase unitBase { get; private set; } 

    
    
    public BoardCell(Vector2Int _cellPosition,Vector3 _unitSetPosition)
    {
        //初期化
        cellPosition = _cellPosition;
        unitSetPosition = _unitSetPosition;
        //セルを空き状態に初期化
        unitBase = null;
        
    }
    

    public bool IsFreeCell()
    {
        if (unitBase == null)
            return true;
        else
            return false;
    }

    public bool ExistUnit()
    {
        if (unitBase != null)
            return true;
        else
            return false;
    }

    public void ReleaseCell()
    {
        unitBase = null;
    }
    

    public void SetUnit(UnitBase _unitData)
    {
        unitBase = _unitData;
        unitBase.unitState.SetCell(this);
    }

    public void UpdateCellUnit(BoardCell nextCell)
    {
        nextCell.SetUnit(unitBase);
        ReleaseCell();
    }
}
