using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinMovement : UnitMovement
{
    public AssassinMovement(UnitBase i_unitBase) : base(i_unitBase)
    {
    }

    protected override Vector2Int[] movableArea { get; set; } =
    {
        Vector2Int.down,
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 1),
    };

    protected override Vector2Int[] additionalCheckPos { get; set; } =
    {
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 1),
    };

    protected override void AdditionalMovableArea(Board board, Vector2Int direction, ref List<BoardCell> area)
    {
        if (!Contain(additionalCheckPos, direction))
            return;

        int i = 2;
        while (i < 100)
        {
            BoardCell destination = board.FindDistantFreeCell(
                unitBase.unitState.currentCell, direction * i);

            if (destination != null)
            {
                area.Add(destination);
                //マスを黄色にする

            }
            else
                return;

            i++;
        }
    }

}
