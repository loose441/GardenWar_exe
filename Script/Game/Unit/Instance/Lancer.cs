using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lancer : UnitBase
{
    protected override string prefabName { get; } = "Lancer";
    public override string unitName { get; } = "Lancer";
    public override UnitMovement unitMovement { get; }
    public override UnitAttack unitAttack { get; }

    public override int maxHp { get; } = 20;
    public override bool canLevelUp { get; } = true;


    public Lancer(int unitColor, BoardCell firstCell) : base(unitColor, firstCell)
    {
        unitMovement = new LancerMovement(this);
        unitAttack = new LancerAttack(this);
    }

    public override UnitBase Clone()
    {
        UnitBase clone = new Lancer(unitState.unitColor, unitState.currentCell);
        clone.unitState.Copy(this.unitState);
        clone.unitAttack.Copy(this.unitAttack.atk);

        return clone;
    }
}
