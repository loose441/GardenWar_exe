using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitMovement
{
    //移動にかかる時間
    private const float moveTime = 0.8f;
    //移動するときの高さ
    private const float moveHight = 6f;

    protected abstract Vector2Int[] movableArea { get; set; }   //移動可能範囲
    protected virtual Vector2Int[] additionalCheckPos { get; set; }
    protected UnitBase unitBase;

    public UnitMovement(UnitBase i_unitBase)
    {
        unitBase = i_unitBase;


        //白の場合反転
        if (unitBase.unitState.unitColor != (int)UnitTeam.ColorVariety.white)
            return;
        
        for(int i = 0; i < movableArea.Length; i++)
        {
            movableArea[i] *= -1;
        }

        if (additionalCheckPos != null)
        {
            for (int i = 0; i < additionalCheckPos.Length; i++)
            {
                additionalCheckPos[i] *= -1;
            }
        }

    }

    protected virtual void AdditionalMovableArea(Board board, Vector2Int direction, ref List<BoardCell> area)
    {

    }

    public void GetSelectableCell(Board board, out List<BoardCell> area)
    {
        area = new List<BoardCell>();

        if (unitBase.unitState.ActEnd())
            return;
        

        BoardCell currentCell = unitBase.unitState.currentCell;

        foreach (Vector2Int distance in movableArea)
        {
            BoardCell destination = board.FindDistantFreeCell(currentCell, distance);

            if (destination != null)
            {
                area.Add(destination);
                AdditionalMovableArea(board, distance, ref area);
            }
        }


    }

    public IEnumerator MoveInstance(BoardCell nextCell)
    {
        Vector3 startPos = unitBase.instanceTransform.position;
        float yMin = unitBase.instanceTransform.position.y;
        float time = 0;

        SEManager.singleton.PlayMoveSE();

        /*  移動アニメーション
            上下左右1マス分の移動はy軸方向の移動をさせない    */
        bool jumpFlag = true;
        if (Vector2Int.Distance(unitBase.unitState.currentCell.cellPosition, nextCell.cellPosition) <= 1)
            jumpFlag = false;


        //移動アニメーション
        while (time < moveTime)
        {
            Vector3 nextPosition = Vector3.Lerp(startPos, nextCell.unitSetPosition, time / moveTime);
            //上下移動
            if (jumpFlag)
                nextPosition.y = yMin + moveHight * Mathf.Sin(Mathf.PI / moveTime * time);

            unitBase.instanceTransform.position = nextPosition;


            time += Time.deltaTime;

            //1フレーム待機
            yield return null;
        }


        //最終的な位置調整
        unitBase.instanceTransform.position = nextCell.unitSetPosition;

    }

    protected bool Contain(Vector2Int[] array,Vector2Int target)
    {
        foreach(Vector2Int element in array)
        {
            if (element == target)
                return true;
        }

        return false;
    }
}
