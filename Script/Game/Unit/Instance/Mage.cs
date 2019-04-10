using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : UnitBase
{
    protected override string prefabName { get; } = "Mage";
    public override string unitName { get; } = "Mage";
    public override UnitMovement unitMovement { get; }
    public override UnitAttack unitAttack { get; }

    public override int maxHp { get; } = 10;
    public override bool canLevelUp { get; } = false;

    public Mage(int unitColor, BoardCell firstCell) : base(unitColor, firstCell)
    {
        unitMovement = new MageMovement(this);
        unitAttack = new MageAttack(this);
    }

    public override UnitBase Clone()
    {
        UnitBase clone = new Mage(unitState.unitColor, unitState.currentCell);
        clone.unitState.Copy(this.unitState);
        clone.unitAttack.Copy(this.unitAttack.atk);

        return clone;
    }
}
