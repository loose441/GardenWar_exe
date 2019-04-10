using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAttack
{
    public abstract int atk { get; protected set; }    // 攻撃力

    //アニメーション全体の長さ
    public abstract float animLength { get; protected set; }
    //アニメーションの攻撃時までの時間
    protected abstract float attackTime { get; }
    protected abstract List<Vector2Int> selectableArea { get; set; }
    private List<BoardCell> glowingCell = new List<BoardCell>();



    protected UnitBase unitBase;
    protected UnitState unitState;


    public UnitAttack(UnitBase i_unitBase)
    {
        unitBase = i_unitBase;
        unitState = unitBase.unitState;
        
    }

    protected virtual void GenerateHitEff(GameObject _instance)
    {
        EffectGenerator.NormalHit(_instance);
    }

    protected virtual void GenerateHitSE()
    {
        SEManager.singleton.PlayHitSE();
    }

    private void GenerateAttackSE()
    {
        SEManager.singleton.PlayAttackSE();
    }

    protected virtual void HitEvent(List<BoardCell> attackableArea)
    {
        //敵ユニット攻撃時の処理
        bool hitFlag = false;
        foreach (BoardCell cell in attackableArea)
        {
            if (!cell.ExistUnit())
                continue;


            if (cell.unitBase.unitState.unitColor != unitState.unitColor)
            {
                hitFlag = true;
                GenerateHitEff(cell.unitBase.instance);
                cell.unitBase.unitState.Damage(atk);

            }

        }

        //音割れ回避のため1度のみ再生
        GenerateAttackSE();
        if (hitFlag)
            GenerateHitSE();


    }

    protected virtual void GlowSectableCell(BoardCell cell)
    {
        //マスを赤色にする
        GlowPanelManager.Glow(cell, GlowPanelManager.ColorVariety.Red);
    }


    public void GetSelectableCell(Board board, out List<BoardCell> area)
    {
        area = new List<BoardCell>();

        if (unitState.ActEnd())
            return;

        
        foreach (Vector2Int distance in selectableArea)
        {

            BoardCell cell = board.FindDistantCell(unitState.currentCell, distance);

            if (cell != null)
            {
                area.Add(cell);
            }

        }
    }

    public void GlowSelectableCell(Board board)
    {
        List<BoardCell> area = new List<BoardCell>();
        GetSelectableCell(board, out area);

        foreach(BoardCell cell in area)
        {
            GlowSectableCell(cell);
        }
    }


    public abstract void GetAttackableArea(Board board, Vector2Int atkDirection, out List<BoardCell> area);

    public void GetAllAttackableArea(Board board, out List<BoardCell> area)
    {
        area = new List<BoardCell>();
        List<BoardCell> selectable = new List<BoardCell>();

        GetSelectableCell(board, out area);

        List<BoardCell> tmpArea = new List<BoardCell>();
        foreach(BoardCell cell in selectable)
        {
            Vector2Int direction = cell.cellPosition - unitState.currentCell.cellPosition;
            GetAttackableArea(board, direction, out tmpArea);

            foreach(BoardCell tmpCell in tmpArea)
            {
                if (area.Contains(tmpCell))
                    continue;

                area.Add(tmpCell);
            }
        }

    }

    //攻撃範囲の表示
    public void GlowAttackableArea(Board board, Vector2Int atkDirection)
    {
        List<BoardCell> attackableArea = new List<BoardCell>();
        GetAttackableArea(GameManager.board, atkDirection, out attackableArea);

        HideAttackableArea();

        foreach(BoardCell cell in attackableArea)
        {

            //選択可能マス以外を表示
            if (cell != board.FindDistantCell(unitState.currentCell,atkDirection))
            {
                glowingCell.Add(cell);
                GlowPanelManager.Glow(cell, GlowPanelManager.ColorVariety.Orange);
            }

        }
    }
    //攻撃範囲の非表示
    public void HideAttackableArea()
    {
        foreach (BoardCell cell in glowingCell)
        {
            GlowPanelManager.ResetPanel(cell);
        }

        glowingCell.Clear();
    }


    public IEnumerator Attack(Vector2Int atkDirection)
    {
        unitBase.animator.SetTrigger("Attack");

        //攻撃タイミングの調整
        yield return new WaitForSeconds(attackTime);

        
        List<BoardCell> attackableArea = new List<BoardCell>();
        GetAttackableArea(GameManager.board, atkDirection, out attackableArea);

        //攻撃処理
        HitEvent(attackableArea);
    }
    

    public void ImproveAtk(int amount)
    {
        atk += amount;
    }

    public void Copy(int originalAtk)
    {
        atk = originalAtk;
    }
}
