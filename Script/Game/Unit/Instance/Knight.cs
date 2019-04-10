using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : UnitBase
{
    protected override string prefabName { get; } = "Knight";
    public override string unitName { get; } = "Knight";
    public override UnitMovement unitMovement { get; }
    public override UnitAttack unitAttack { get; }

    public override int maxHp { get; } = 20;
    public override bool canLevelUp { get; } = true;

    public Knight(int unitColor, BoardCell firstCell) : base(unitColor, firstCell)
    {
        unitMovement = new KnightMovement(this);
        unitAttack = new KnightAttack(this);
    }

    public override UnitBase Clone()
    {
        UnitBase clone = new Knight(unitState.unitColor, unitState.currentCell);
        clone.unitState.Copy(this.unitState);
        clone.unitAttack.Copy(this.unitAttack.atk);

        return clone;
    }
}
