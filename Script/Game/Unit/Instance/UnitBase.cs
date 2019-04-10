using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase
{
    protected abstract string prefabName { get; }
    public abstract string unitName { get; }
    public abstract int maxHp { get; }
    public abstract bool canLevelUp { get; }

    public GameObject instance { get; private set; }
    public Animator animator { get; private set; }

    public Transform instanceTransform { get; protected set; }
    public UnitStateUI unitStateUI { get; protected set; }
    public UnitState unitState { get; private set; }
    public UnitAnimation unitAnimation { get; private set; }
    public abstract UnitMovement unitMovement { get; }
    public abstract UnitAttack unitAttack { get; }


    
    public enum UnitClass
    {
        Knight,
        Lancer,
        Archer,
        Breaker,
        Mage,
        Assassin
    }


    public UnitBase(int _unitColor, BoardCell firstCell)
    {
        unitState = new UnitState(_unitColor, firstCell, this);
    }
    
    public void GenerateStateUI()
    {
        unitStateUI = StatusNodeGenerator.GenerateStateNode(PlayerController.playerColor == unitState.unitColor, this);
    }

    public void InstantiateUnitPrefab(int _unitColor, BoardCell firstCell)
    {
        //プレファブのインスタンス化
        instance = UnitGenerator.InstantiatePrefab(prefabName, firstCell.unitSetPosition);
        //プレファブの色を変更
        UnitColor.SetUnitColor(instance, _unitColor);

        //ユニットの向きを初期化
        if (_unitColor == (int)UnitTeam.ColorVariety.white)
            instance.transform.Rotate(new Vector3(0, 180, 0));


        animator = instance.GetComponent<Animator>();
        unitAnimation = new UnitAnimation(instance);

        unitState.unitText = instance.GetComponent<UnitText>();
        instanceTransform = instance.transform;
    }

    public abstract UnitBase Clone();
}
