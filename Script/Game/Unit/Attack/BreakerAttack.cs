﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakerAttack : UnitAttack
{
    public override float animLength { get; protected set; } = 1.8f;
    protected override float attackTime { get;} = 1f;
    public override int atk { get; protected set; } = 5;

    protected override List<Vector2Int> selectableArea { get; set; } = new List<Vector2Int>
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right,
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, -1),
        new Vector2Int(-1, 1)
    };


    public BreakerAttack(UnitBase i_unitBase) : base(i_unitBase)
    {

    }



    public override void GetAttackableArea(Board board,Vector2Int atkDirection, out List<BoardCell> area)
    {
        area = new List<BoardCell>();

        BoardCell attackableCell = board.FindDistantCell(unitState.currentCell, atkDirection);

        if (attackableCell == null)
            return;

        area.Add(attackableCell);
    }
}