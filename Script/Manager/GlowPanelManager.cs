using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowPanelManager : MonoBehaviour
{

    private static GlowPanel[,] glowPanels = new GlowPanel[Board.boardWidth, Board.boardHeight];
    private static readonly Color[] colors = new Color[]
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.yellow,
        new Color(1, 0.59f, 0, 1),
        Color.white
    };

    public enum ColorVariety
    {
        Red,
        Blue,
        Green,
        Yellow,
        Orange,
        White
    }

    private void Start()
    {

        for (int x = 0; x < Board.boardWidth; x++)
        {
            for (int y = 0; y < Board.boardHeight; y++)
            {

                glowPanels[x, y] = new GlowPanel(Board.CenterPosOfCell(x, y), this.transform);
                glowPanels[x, y].ResetPanel();
            }
        }
        

    }

    public static void Glow(int x, int y, ColorVariety color)
    {
        glowPanels[x, y].Glow(colors[(int)color]);
    }

    public static void Glow(BoardCell cell,ColorVariety color)
    {
        Vector2Int pos = cell.cellPosition;
        Glow(pos.x, pos.y, color);
    }


    public static void ResetPanel(int x, int y)
    {
        glowPanels[x, y].ResetPanel();
    }

    public static void ResetPanel(BoardCell cell)
    {
        Vector2Int pos = cell.cellPosition;
        ResetPanel(pos.x, pos.y);
    }

    public static void ResetAll()
    {
        foreach(GlowPanel panel in glowPanels)
        {
            panel.ResetPanel();
        }
    }
}
