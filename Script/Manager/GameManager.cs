using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton { get; private set; }
    public static List<UnitTeam> teamList = new List<UnitTeam>();
    public static Board board;
    
    public const int gameEndUnitNum = 3;   //ユニットの数がこの値以下の場合敗北
    public const int actLimit = 2;         //行動回数制限

    public static bool gameEnd { get; private set; } = false;

    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Screen.SetResolution(1024, 768, false, 60);
    }

    private void Awake()
    {
        singleton = this;
        teamList.Clear();
        board = null;
    }

    private void Start()
    {
        ScreenManager.AwakeStartScreen();
    }


    public static void GameStart()
    {
        //プレイヤー及び敵のチームの色を決定
        int randomValue = UnityEngine.Random.Range(1, 3);
        PlayerController.InitializePlayerColor(randomValue / 2);
        EnemyAI.InitializeColor(randomValue % 2);

        board = new Board();
        InstantiateFirstUnit();

        gameEnd = false;
        PhaseManager.PhaseTrans();
        CameraController.PauseEnd();


    }

    public static void GameEnd(int loserColor)
    {
        gameEnd = true;
        PlayerController.Pause();
        CameraController.Pause();
        ScreenManager.AwakeEndScreen(loserColor != PlayerController.playerColor);

    }


    private static void InstantiateFirstUnit()
    {
        //黒ユニットを生成する場所を定義
        List<Vector2Int> instancePos = new List<Vector2Int>();
        for (int x = 1; x < 7; x++)
        {
            for(int y = 0; y < 2; y++)
            {
                instancePos.Add(new Vector2Int(x, y));
            }
        }
        
        //黒ユニット生成
        foreach (UnitBase.UnitClass unitType in Enum.GetValues(typeof(UnitBase.UnitClass)))
        {
            int randomNum = UnityEngine.Random.Range(0, instancePos.Count);
            InstantiateUnit(instancePos[randomNum]
                , (int)UnitTeam.ColorVariety.black
                , unitType);

            instancePos.RemoveAt(randomNum);
        }

        
        //白ユニットを生成する場所を定義
        instancePos.Clear();
        for (int x = 1; x < 7; x++)
        {
            for (int y = 6; y < 8; y++)
            {
                instancePos.Add(new Vector2Int(x, y));
            }
        }

        //白ユニット生成
        foreach (UnitBase.UnitClass unitType in Enum.GetValues(typeof(UnitBase.UnitClass)))
        {
            int randomNum = UnityEngine.Random.Range(0, instancePos.Count);
            InstantiateUnit(instancePos[randomNum]
                , (int)UnitTeam.ColorVariety.white
                , unitType);

            instancePos.RemoveAt(randomNum);
        }
        
    }


    private static void RegistUnit(UnitBase unit)
    {
        foreach(UnitTeam team in teamList)
        {
            if (team.teamColor == unit.unitState.unitColor)
            {
                team.unitList.Add(unit);
                return;
            }
        }

        teamList.Add(new UnitTeam(unit.unitState.unitColor));
        GetUnitTeam(unit.unitState.unitColor).unitList.Add(unit);

    }

    public static UnitTeam GetUnitTeam(int teamColor)
    {
        foreach(UnitTeam team in teamList)
        {
            if (team.teamColor == teamColor)
                return team;
        }

        Debug.Log("目的のチームを探すことができませんでした");
        return null;
    }
    public static UnitTeam GetUnitTeam(UnitBase unit)
    {
        return GetUnitTeam(unit.unitState.unitColor);
    }

    
    public static bool IsMainPhaseEnd(UnitBase[] units)
    {
        int actEndCount = 0;
        
        //ユニットの数が行動回数制限より少ない場合は終了
        if (units.Length < actLimit)
            return true;


        foreach (UnitBase unit in units)
        {
            if (unit.unitState.ActEnd())
                actEndCount++;
        }
        
        if (actEndCount >= actLimit)
            return true;

        return false;
    }

    public static bool IsBattlePhaseEnd()
    {

        List<UnitBase> units = GetUnitTeam(PhaseManager.turnPlayer).unitList;


        foreach (UnitBase unit in units)
        {
            if (!unit.unitState.ActEnd())
                return false;
        }

        return true;
    }

    public static bool IsGameEnd()
    {
        foreach (UnitTeam team in teamList)
        {
            if (team.unitList.Count > gameEndUnitNum)
                continue;

            GameEnd(team.teamColor);
            return true;
        }

        return false;
    }

    

    private static void InstantiateUnit(BoardCell cell, int unitColor, UnitBase.UnitClass unitClass)
    {

        if (!cell.IsFreeCell())
        {
            Debug.Log("Inputed cell has another Unit");
            return;
        }


        UnitBase unit = null;


        switch (unitClass)
        {
            case UnitBase.UnitClass.Knight:
                unit = new Knight(unitColor, cell);
                break;

            case UnitBase.UnitClass.Mage:
                unit = new Mage(unitColor, cell);
                break;

            case UnitBase.UnitClass.Archer:
                unit = new Archer(unitColor, cell);
                break;

            case UnitBase.UnitClass.Lancer:
                unit = new Lancer(unitColor, cell);
                break;

            case UnitBase.UnitClass.Breaker:
                unit = new Breaker(unitColor, cell);
                break;

            case UnitBase.UnitClass.Assassin:
                unit = new Assassin(unitColor, cell);
                break;

            default:
                Debug.Log("Error : データが設定されていません");
                return;
        }

        unit.InstantiateUnitPrefab(unitColor, cell);
        unit.GenerateStateUI();
        cell.SetUnit(unit);
        RegistUnit(unit);


    }

    private static void InstantiateUnit(Vector2Int coordinates, int unitColor, UnitBase.UnitClass unitClass)
    {
        BoardCell cell = board.FindCell(coordinates);
        if (cell != null)
            InstantiateUnit(cell, unitColor, unitClass);
        else
            Debug.Log("Invalid coordinates has inputted");

    }


    public static int GetNextColorNum(int colorNum)
    {
        if (teamList.Count == 0)
            return -1;

        int i = 0;
        while (i < teamList.Count-1)
        {
            if (teamList[i].teamColor != colorNum)
            {
                i++;
                continue;
            }
            
            return teamList[i + 1].teamColor;
        }

        return teamList[0].teamColor;

    }

    public static void GetCurrentTurnUnits(ref List<UnitBase> units)
    {
        units = GetUnitTeam(PhaseManager.turnPlayer).unitList;
    }


    
}