using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAttack : UnitAttack
{
    public override float animLength { get; protected set; } = 1.3f;
    public override int atk { get; protected set; } = 3;
    private const int healAmount = 1;

    protected override float attackTime { get; } = 0.9f;
    protected override List<Vector2Int> selectableArea { get; set; }

    public MageAttack(UnitBase i_unitBase) : base(i_unitBase)
    {
        InitializeSelectableArea();
    }

    private void InitializeSelectableArea()
    {
        selectableArea = new List<Vector2Int> { };

        for(int x = -2; x < 3; x++)
        {
            for(int y = -2; y < 3; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                selectableArea.Add(new Vector2Int(x, y));
            }
        }
    }



    public override void GetAttackableArea(Board board,Vector2Int atkDirection, out List<BoardCell> area)
    {
        area = new List<BoardCell>();

        BoardCell attackableCell =  board.FindDistantCell(unitState.currentCell, atkDirection);

        if (attackableCell == null)
            return;

        area.Add(attackableCell);
    }

    protected override void GlowSectableCell(BoardCell cell)
    {
        if (!cell.ExistUnit())
            base.GlowSectableCell(cell);

        else if(cell.unitBase.unitState.unitColor != unitState.unitColor)
            base.GlowSectableCell(cell);

        else
            GlowPanelManager.Glow(cell, GlowPanelManager.ColorVariety.Blue);
    }


    protected override void GenerateHitEff(GameObject _instance)
    {
        EffectGenerator.FireHit(_instance);
    }

    protected override void GenerateHitSE()
    {
        SEManager.singleton.PlayFireSE();
    }

    private void GenerateHealEff(GameObject _instance)
    {
        EffectGenerator.HealEff(_instance);
    }

    private void GenerateHealSE()
    {
        SEManager.singleton.PlayHealSE();
    }

    


    protected override void HitEvent(List<BoardCell> attackableArea)
    {

        bool hitFlag = false;
        bool healFlag = false;
        

        foreach (BoardCell cell in attackableArea)
        {
            if (!cell.ExistUnit())
                continue;


            hitFlag = true;

            if (cell.unitBase.unitState.unitColor != unitState.unitColor)
            {
                GenerateHitEff(cell.unitBase.instance);
                cell.unitBase.unitState.Damage(atk);
                break;
            }
            else
            {
                healFlag = true;
                cell.unitBase.unitState.Heal(healAmount);
                GenerateHealEff(cell.unitBase.instance);
            }

        }

        //音割れ回避のためヒット音は1度のみ再生
        if (healFlag)
            GenerateHealSE();
        else if (hitFlag)
            GenerateHitSE();
    }
}
