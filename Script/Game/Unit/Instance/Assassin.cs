using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin : UnitBase
{
    protected override string prefabName { get; } = "Assassin";
    public override string unitName { get; } = "Assassin";
    public override UnitMovement unitMovement { get; }
    public override UnitAttack unitAttack { get; }

    public override int maxHp { get; } = 15;
    public override bool canLevelUp { get; } = false;


    public Assassin(int unitColor, BoardCell firstCell) : base(unitColor, firstCell)
    {
        unitMovement = new AssassinMovement(this);
        unitAttack = new AssassinAttack(this);
    }


    public override UnitBase Clone()
    {
        UnitBase clone = new Assassin(unitState.unitColor, unitState.currentCell);
        clone.unitState.Copy(this.unitState);
        clone.unitAttack.Copy(this.unitAttack.atk);

        return clone;
    }
}
