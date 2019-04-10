using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : UnitMovement
{
    public KnightMovement(UnitBase i_unitBase) : base(i_unitBase)
    {

    }

    protected override Vector2Int[] movableArea { get; set; } =
    {
        Vector2Int.up,
        Vector2Int.left,
        Vector2Int.down,
        Vector2Int.right,
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 1)
    };

}
