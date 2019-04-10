using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : UnitBase
{
    protected override string prefabName { get; } = "Archer";
    public override string unitName { get; } = "Archer";
    public override UnitMovement unitMovement { get; }
    public override UnitAttack unitAttack { get; }

    public override int maxHp { get; } = 20;
    public override bool canLevelUp { get; } = true;


    public Archer(int unitColor, BoardCell firstCell) : base(unitColor, firstCell)
    {
        unitMovement = new ArcherMovement(this);
        unitAttack = new ArcherAttack(this);
    }

    public override UnitBase Clone()
    {
        UnitBase clone = new Archer(unitState.unitColor, unitState.currentCell);
        clone.unitState.Copy(this.unitState);
        clone.unitAttack.Copy(this.unitAttack.atk);

        return clone;
    }
}
