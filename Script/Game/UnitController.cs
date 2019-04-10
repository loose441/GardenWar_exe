using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public delegate void EventCallBack();
    private static UnitController singleton;

    private void Awake()
    {
        singleton = this;
    }


    private static void RemoveDeadUnits(ref List<UnitBase> units, ref List<UnitBase> deadUnits)
    {
        deadUnits.Clear();

        for (int i = units.Count - 1; i >= 0; i--)
        {
            if (units[i].unitState.currentHP <= 0)
            {
                deadUnits.Add(units[i]);
                GameManager.GetUnitTeam(units[i].unitState.unitColor).RemoveUnit(units[i]);
            }
        }

    }

    private static IEnumerator RotateAfterAttackEnd(UnitBase attacker)
    {
        yield return new WaitForSeconds(attacker.unitAttack.animLength);

        singleton.StartCoroutine(
            UnitRotation
            .RotateUnit(attacker.instance, UnitTeam.forwardDir[attacker.unitState.unitColor]));
    }

    public static void MoveUnit(UnitBase unit, BoardCell nextCell)
    {
        //行動回数消費
        unit.unitState.Act();
        //セル移動
        unit.unitState.currentCell.UpdateCellUnit(nextCell);
    }

    public static void UndoUnitMove(UnitBase unit, BoardCell lastCell)
    {
        //セル移動
        unit.unitState.currentCell.UpdateCellUnit(lastCell);
        unit.unitState.RecoverActNum();
    }

    public static IEnumerator MoveEvent(UnitBase unit, BoardCell nextCell, EventCallBack callBack = null)
    {
        //行動回数消費
        unit.unitState.Act();

        //移動アニメーション開始
        yield return singleton.StartCoroutine(
            unit
            .unitMovement
            .MoveInstance(nextCell));

        //セル移動
        unit.unitState.currentCell.UpdateCellUnit(nextCell);


        //レベルアップ確認
        if (unit.unitState.CanLevelUp())
        {
            unit.unitState.LevelUp();
            yield return new WaitForSeconds(0.5f);
        }
        

        //ターン遷移確認
        if (GameManager.IsMainPhaseEnd(GameManager.GetUnitTeam(PhaseManager.turnPlayer).unitList.ToArray()))
        {
            PhaseManager.PhaseTrans();
            yield break;
        }
        
        if (callBack != null)
            callBack();
    }



    public static IEnumerator AttackEvent(UnitBase attacker, BoardCell selectedCell, EventCallBack callBack = null)
    {
        if (attacker.unitState.ActEnd())
            Debug.Log("error");
        //行動回数消費
        attacker.unitState.Act();
        if (attacker.unitName == "Mage")
            Debug.Log(attacker.unitState.GetActNum());

        Vector2Int attackDir = selectedCell.cellPosition - attacker.unitState.currentCell.cellPosition;


        //攻撃方向へ向ける
        yield return singleton.StartCoroutine(
            UnitRotation
            .RotateUnit(attacker.instance, attackDir));
        //攻撃アニメーション終了時に元に戻す
        singleton.StartCoroutine(RotateAfterAttackEnd(attacker));


        //攻撃開始
        yield return singleton.StartCoroutine(
            attacker
            .unitAttack
            .Attack(attackDir));


        //ユニットの生死を確認
        List<UnitBase> deadUnits = new List<UnitBase>();
        foreach (UnitTeam team in GameManager.teamList)
        {
            if (PhaseManager.IsTurnPlayer(team.teamColor))
                continue;

            RemoveDeadUnits(ref team.unitList, ref deadUnits);
        }


        //死亡ユニットがある場合、死亡アニメーション
        if (deadUnits.Count > 0)
        {
            foreach (UnitBase unitData in deadUnits)
            {
                singleton.StartCoroutine(unitData.unitAnimation.DeathAnim());
                Destroy(unitData.instance, UnitAnimation.deathTime);
            }

            deadUnits.Clear();
            SEManager.singleton.PlayDeathSE();
            yield return new WaitForSeconds(UnitAnimation.deathTime);

        }

        //ゲーム終了確認
        if (GameManager.IsGameEnd())
            yield break;


        //ターン遷移確認
        if (GameManager.IsBattlePhaseEnd())
        {
            PhaseManager.PhaseTrans();
            yield break;
        }

        if (callBack != null)
            callBack();
    }
}
