using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState
{
    public int currentHP { get; private set; }
    public int unitColor { get; private set; }
    private int actNum = 1;
    private int level = 1;
    private UnitBase unitBase;
    public BoardCell currentCell { get; private set; }
    public UnitText unitText;

    public const int maxLevel = 2;
    public const int levelUp_heal = 5;
    public const int levelUp_atkUp = 1;
    public static readonly int[] levelUpLine = new int[] { 5, 2 };

    public UnitState(int _unitColor, BoardCell _currentCell, UnitBase _unitBase)
    {
        //ユニットの色を初期化
        unitColor = _unitColor;
        //HPを初期化
        currentHP = _unitBase.maxHp;

        unitBase = _unitBase;
        currentCell = _currentCell;

    }

    public int GetActNum()
    {
        return actNum;
    }

    private void Death()
    {
        currentCell.ReleaseCell();

        CoroutineManager.singleton.StartCoroutine(
            unitBase.unitStateUI.TransBackGroundColor(UnitStateUI.ColorVariety.Black));


    }

    public void SetCell(BoardCell boardCell)
    {
        currentCell = boardCell;
    }


    public void Damage(int damage)
    {
        currentHP -= damage;
        unitText.DamageText(damage.ToString());

        if (currentHP <= 0)
        {
            currentHP = 0;
            Death();
        }

        CoroutineManager.singleton.StartCoroutine(
            unitBase.unitStateUI.TransHpBar(currentHP));
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        unitText.HealText(amount.ToString());

        if (currentHP >= unitBase.maxHp)
        {
            currentHP = unitBase.maxHp;
        }

        CoroutineManager.singleton.StartCoroutine(
            unitBase.unitStateUI.TransHpBar(currentHP));
    }

    public void PhaseChanged()
    {
        actNum = 1;
    }

    public void Act()
    {
        actNum--;
    }
    
    public void RecoverActNum()
    {
        actNum = 1;
    }
    
    public bool ActEnd()
    {
        if (actNum <= 0)
            return true;
        else
            return false;
    }

    public bool CanLevelUp()
    {
        if (unitColor == (int)UnitTeam.ColorVariety.black && currentCell.cellPosition.y < levelUpLine[unitColor])
        {
            return false;
        }
        if (unitColor == (int)UnitTeam.ColorVariety.white && currentCell.cellPosition.y > levelUpLine[unitColor])
        {
            return false;
        }

        if (unitBase.canLevelUp && level < maxLevel)
            return true;
        else
            return false;
    }

    public bool IsLevelMax()
    {
        return level == maxLevel;
    }

    public void LevelUp()
    {
        level++;
        Heal(levelUp_heal);
        unitBase.unitAttack.ImproveAtk(levelUp_atkUp);

        EffectGenerator.PowerUpEff(unitBase.instance);
        SEManager.singleton.PlayPowerUpSE();
        unitText.LevelUpText();
    }



    public void Copy(UnitState originalState)
    {
        currentHP = originalState.currentHP;
        unitColor = originalState.unitColor;
        actNum = originalState.actNum;
        level = originalState.level;
    }
}