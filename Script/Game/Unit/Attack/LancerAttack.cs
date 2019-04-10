using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerAttack : UnitAttack
{
    public override float animLength { get; protected set; } = 1.2f;
    protected override float attackTime { get; } = 0.7f;
    public override int atk { get; protected set; } = 3;

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



    public LancerAttack(UnitBase i_unitBase) : base(i_unitBase)
    {

    }



    public override void GetAttackableArea(Board board, Vector2Int atkDirection, out List<BoardCell> area)
    {
        area = new List<BoardCell>();

        for (int i = 1; i < 3; i++)
        {
            //攻撃可能マスを計算
            Vector2Int distance = atkDirection * i;


            BoardCell attackableCell = board.FindDistantCell(unitState.currentCell, distance);

            if (attackableCell == null)
                continue;

            area.Add(attackableCell);
        }
    }
}
