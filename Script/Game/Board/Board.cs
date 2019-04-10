using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    public const int boardWidth = 8;
    public const int boardHeight = 8;
    //ボードの左下の座標
    public static readonly Vector3 board_leftDown = new Vector3(-15.9f, 0, -16.4f);
    //ボードの右上の座標
    public static readonly Vector3 board_rightUp = new Vector3(16.1f, 0, 15.7f);


    public BoardCell[,] boardCells = new BoardCell[boardWidth, boardHeight];
    

    public Board()
    {
        GenerateCell();

    }

    
    private void GenerateCell()
    {

        //左下から右上にかけてセルを配置
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0;y < boardHeight; y++)
            {

                boardCells[x, y] = new BoardCell(new Vector2Int(x, y), CenterPosOfCell(x, y));

            }
        }
        
    }
    



    private bool IsCellFree(Vector2Int cellPosition)
    {
        //入力されたセルが盤面に収まっているか
        if (!IsCellOnBoard(cellPosition))
            return false;

        //入力されたセルにユニットなどが配置されていないか
        return boardCells[cellPosition.x, cellPosition.y].IsFreeCell();
    }

    
    private string ConvertNumToColor(int colorNum)
    {
        if (colorNum == 1)
            return "Black";
        else
            return "White";
    }


    
    public static Vector3 CenterPosOfCell(int x, int y)
    {
        Vector3 center = board_leftDown;
        center.x += (board_rightUp.x - board_leftDown.x) / (2 * boardWidth) * (2 * x + 1);
        center.y += 1;
        center.z += (board_rightUp.z - board_leftDown.z) / (2 * boardHeight) * (2 * y + 1);

        return center;
    }


    //入力されたセルの位置を求める
    public Vector2Int FindCellPosition(BoardCell targetCell)
    {
        int x, y;

        for (x = 0; x < boardWidth; x++)
        {
            for (y = 0; y < boardHeight; y++)
            {
                if (boardCells[x, y] == targetCell)
                    return new Vector2Int(x, y);
            }
        }

        return new Vector2Int(-1, -1);
    }

    
    //入力された位置から、入力された距離だけ離れた位置にあるセルを求める
    public BoardCell FindDistantCell(BoardCell currentCell, Vector2Int distance)
    {
        Vector2Int currentPosition = FindCellPosition(currentCell);
        Vector2Int destination = currentPosition + distance;

        if (!IsCellOnBoard(destination))
        {
            return null;
        }

        return boardCells[destination.x, destination.y];
    }

    public BoardCell FindDistantFreeCell(BoardCell currentCell, Vector2Int distance)
    {
        BoardCell distantCell = FindDistantCell(currentCell, distance);

        if (distantCell == null)
            return null;

        if (!distantCell.IsFreeCell())
            return null;
        else
            return distantCell;
    }

    //入力されたセルの位置がボード上に存在するか
    public bool IsCellOnBoard(Vector2Int cellPosition)
    {
        //入力された座標が盤面の内に収まっているか
        if (cellPosition.x < 0 || cellPosition.x >= boardWidth)
            return false;
        if (cellPosition.y < 0 || cellPosition.y >= boardHeight)
            return false;

        return true;
    }


    //入力された位置のセルを返す
    public BoardCell FindCell(Vector2Int cellPosition)
    {
        if (!IsCellOnBoard(cellPosition))
            return null;
        return boardCells[cellPosition.x, cellPosition.y];
    }
    
    public Board Clone(out List<UnitBase> unitList)
    {
        unitList = new List<UnitBase>();
        Board newBoard = new Board();
        

        foreach (BoardCell originalCell in this.boardCells)
        {

            if (originalCell.ExistUnit())
            {
                newBoard.FindCell(originalCell.cellPosition)
                    .SetUnit(originalCell.unitBase.Clone());
                unitList.Add(newBoard.FindCell(originalCell.cellPosition).unitBase);
            }
                

        }

        return newBoard;
    }

    public static float DistanceBetweenCells(BoardCell cell1,BoardCell cell2)
    {
        return Vector2.Distance(cell1.cellPosition, cell2.cellPosition);
    }

}
