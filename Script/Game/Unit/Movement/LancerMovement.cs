using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerMovement : UnitMovement
{
    public LancerMovement(UnitBase i_unitBase) : base(i_unitBase)
    {

    }

    protected override Vector2Int[] movableArea { get; set; } =
    {
        Vector2Int.up,
        Vector2Int.left,
        Vector2Int.right,
        Vector2Int.down,
        new Vector2Int(1, -1),
        new Vector2Int(-1, -1)
        
    };

    protected override Vector2Int[] additionalCheckPos { get; set; } =
    {
        Vector2Int.up
    };


    protected override void AdditionalMovableArea(Board board, Vector2Int direction, ref List<BoardCell> area)
    {
        if (!Contain(additionalCheckPos, direction))
            return;

        BoardCell destination = board.FindDistantFreeCell(
            unitBase.unitState.currentCell, direction * 2);

        if (destination != null)
        {
            area.Add(destination);
        }
    }
    
}
