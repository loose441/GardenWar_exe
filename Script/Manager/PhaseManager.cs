using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public delegate void PhaseTransCallBack();
    public static PhaseTransCallBack phaseTransCallBack;

    public static PhaseManager singleton;
    private const float transLength = 1.2f;
    public static int turnPlayer { get; private set; }
    public static PhaseName currentPhase { get; private set; }
    private static readonly PhaseName[] PhaseDecisionTable = new PhaseName[]
    {
        PhaseName.MainPhase,
        PhaseName.BattlePhase,
        PhaseName.MainPhase
    };


    public enum PhaseName
    {
        StandbyPhase=0,
        MainPhase,
        BattlePhase
    }

    private void Awake()
    {
        phaseTransCallBack = null;
        singleton = this;
    }

    private void Start()
    {
        currentPhase = PhaseName.StandbyPhase;
        turnPlayer = (int)UnitTeam.ColorVariety.black;
    }

    public static void PhaseTrans()
    {
        if (GameManager.gameEnd)
            return;

        PlayerController.Pause();
        
        //行動回数回復
        foreach(UnitBase unit in GameManager.GetUnitTeam(turnPlayer).unitList)
        {
            unit.unitState.PhaseChanged();
        }
        

        if (currentPhase == PhaseName.BattlePhase)
        { 
            turnPlayer = GameManager.GetNextColorNum(turnPlayer);
        }
        currentPhase = PhaseDecisionTable[(int)currentPhase];


        PlayerController.ClearForcus();

        SEManager.singleton.PlayPhaseTransSE();

       
        //遷移UIの表示
        PhasePanel.PhaseTrans(
            GetTurnColor() + " " + currentPhase.ToString()
            , PhasePanel.ColorVariety.White);

        singleton.StartCoroutine(WaitTransEnd());
    }

    private static IEnumerator WaitTransEnd()
    {
        yield return new WaitForSeconds(transLength);
        PlayerController.PauseEnd();

        if (phaseTransCallBack != null)
            phaseTransCallBack();
    }

    public static bool IsTurnPlayer(int color)
    {
        return color == turnPlayer;
    }

    public static bool IsMainPhase()
    {
        return currentPhase == PhaseName.MainPhase;
    }

    public static bool IsBattlePhase()
    {
        return currentPhase == PhaseName.BattlePhase;
    }

    public static string GetTurnColor()
    {
        return UnitTeam.teamName[turnPlayer];
    }

    public static bool IsYourTurn()
    {
        return turnPlayer == PlayerController.playerColor;
    }
}
