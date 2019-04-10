using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaker : UnitBase
{
    protected override string prefabName { get; } = "Breaker";
    public override string unitName { get; } = "Breaker";
    public override UnitMovement unitMovement { get; }
    public override UnitAttack unitAttack { get; }

    public override int maxHp { get; } = 25;
    public override bool canLevelUp { get; } = false;


    public Breaker(int unitColor, BoardCell firstCell) : base(unitColor, firstCell)
    {
        unitMovement = new BreakerMovement(this);
        unitAttack = new BreakerAttack(this);
    }

    public override UnitBase Clone()
    {
        UnitBase clone = new Breaker(unitState.unitColor, unitState.currentCell);
        clone.unitState.Copy(this.unitState);
        clone.unitAttack.Copy(this.unitAttack.atk);

        return clone;
    }
}
