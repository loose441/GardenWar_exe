using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static bool pauseFlag = true;
    public static int playerColor;

    //現在カーソルが合っているセル
    private BoardCell cellOnCursor;

    //現在選択されているセル
    private static BoardCell forcusedCell;
    //現在選択されているユニットが干渉できるセル
    private static List<BoardCell> selectableCells = new List<BoardCell>();

   
    public static void InitializePlayerColor(int color)
    {
        playerColor = color;

        if (playerColor == (int)UnitTeam.ColorVariety.white)
            CameraController.ReversePosition();
    }
    

    private void Update()
    {
        if (pauseFlag)
        {
            return;
        }


        //カーソルが合っているセルの取得
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        BoardCell cell = GameManager.board.FindCell(RaycastCell(ray));
        if (cell != cellOnCursor)
        {
            cellOnCursor = cell;

            AttackableArea_PanelChange();
        }



        //クリック時のイベント
        if (Input.GetMouseButtonDown(0) && cellOnCursor != null)
        {
            ClickEvent();
        }
        
            
    }
    
    private Vector2Int RaycastCell(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            //セルの位置を計算
            Vector3 point = hit.point - Board.board_leftDown;
            int x = (int)(point.x * Board.boardWidth / (Board.board_rightUp.x - Board.board_leftDown.x));
            int y = (int)(point.z * Board.boardHeight / (Board.board_rightUp.z - Board.board_leftDown.z));

            return new Vector2Int(x, y);
        }

        return new Vector2Int(-1, -1);
    }

    private void ClickEvent()
    {
        //何もフォーカスしていない時
        if (forcusedCell == null && cellOnCursor.ExistUnit())
        {
            ForcusUnit();
            return;
        }

        //選択可能マスを選択時
        if (selectableCells.Contains(cellOnCursor))
        {
            if (!PhaseManager.IsYourTurn())
            {
                ClearForcus();
                return;
            }

            if (PhaseManager.IsMainPhase())
            {
                //ユニットを移動
                MoveUnit(forcusedCell.unitBase, cellOnCursor);

            }
            else
            {
                AttackUnit(forcusedCell.unitBase, cellOnCursor);
            }

            ClearForcus();
            return;
        }

        //フォーカスしていないユニットを選択時
        if (forcusedCell != cellOnCursor && cellOnCursor.ExistUnit())
        {
            ForcusUnit();
            return;
        }



        ClearForcus();
    }

    private void ForcusUnit()
    {
        InitializeUIBack(forcusedCell);
        GlowPanelManager.ResetAll();
        forcusedCell = cellOnCursor;


        //選択したマスを緑色に変化させる
        Vector2Int pos = forcusedCell.cellPosition;
        GlowPanelManager.Glow(pos.x, pos.y, GlowPanelManager.ColorVariety.Green);

        //ステータスUI操作
        CoroutineManager.singleton.StartCoroutine(
            forcusedCell.unitBase.unitStateUI.TransBackGroundColor(
                UnitStateUI.ColorVariety.Yellow));


        if (!PhaseManager.IsTurnPlayer(forcusedCell.unitBase.unitState.unitColor))
        {
            selectableCells.Clear();
            return;
        }
        
        if(PhaseManager.IsYourTurn())
            UpdateSelectableCell();
    }

    private void MoveUnit(UnitBase unit, BoardCell nextCell)
    {
        Pause();
        StartCoroutine(UnitController.MoveEvent(unit, nextCell, PauseEnd));
        
    }

    private void AttackUnit(UnitBase attacker, BoardCell selectedCell)
    {
        Pause();
        StartCoroutine(UnitController.AttackEvent(attacker, selectedCell, PauseEnd));
        
    }

    private void UpdateSelectableCell()
    {
        List<BoardCell> area = new List<BoardCell>();

        if (PhaseManager.IsMainPhase())
        {
            forcusedCell.unitBase.unitMovement.GetSelectableCell(GameManager.board, out area);

            foreach (BoardCell cell in area)
            {
                //マスを黄色にする
                GlowPanelManager.Glow(cell, GlowPanelManager.ColorVariety.Yellow);
            }
        }
        else if (PhaseManager.IsBattlePhase())
        {
            forcusedCell.unitBase.unitAttack.GetSelectableCell(GameManager.board,out area);
            forcusedCell.unitBase.unitAttack.GlowSelectableCell(GameManager.board);

        }
        
        selectableCells.Clear();
        selectableCells = area;
    }


    private void AttackableArea_PanelChange()
    {
        /*
         * 攻撃範囲の表示・非表示の管理
         */

        if (forcusedCell == null || !PhaseManager.IsBattlePhase())
            return;
        
        

        if (selectableCells.Contains(cellOnCursor))
        {
            Vector2Int atkDirection = cellOnCursor.cellPosition - forcusedCell.cellPosition;
            forcusedCell.unitBase.unitAttack.GlowAttackableArea(GameManager.board, atkDirection);
        }
        else
        {
            forcusedCell.unitBase.unitAttack.HideAttackableArea();
        }

    }

    private static void InitializeUIBack(BoardCell cell)
    {
        //ステータスUIの初期化
        if (cell != null)
            CoroutineManager.singleton.StartCoroutine(
                cell.unitBase.unitStateUI.TransBackGroundColor(
                    UnitStateUI.ColorVariety.White));
    }

    public static void ClearForcus()
    {
        //ステータスUIの初期化
        InitializeUIBack(forcusedCell);
        
        forcusedCell = null;
        selectableCells.Clear();
        
        GlowPanelManager.ResetAll();

        //行動可能ユニットのマスを白くする
        if (!PhaseManager.IsYourTurn())
            return;
        List<UnitBase> currentTurnUnits = new List<UnitBase>();
        GameManager.GetCurrentTurnUnits(ref currentTurnUnits);
        
        foreach (UnitBase unit in currentTurnUnits)
        {

            if (!unit.unitState.ActEnd())
            {
                GlowPanelManager.Glow(unit.unitState.currentCell,
                    GlowPanelManager.ColorVariety.White);
            }
        }
    }
    
    public static void Pause()
    {
        //操作制限
        pauseFlag = true;
        if (PhaseManager.IsYourTurn())
            MenuButton.SetButtonEnable(false);
    }

    public static void PauseEnd()
    {
        //操作制限解除
        pauseFlag = false;
        if (PhaseManager.IsYourTurn())
            MenuButton.SetButtonEnable(true);
    }


}