using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowPanel
{
    private Renderer _renderer;
    private const float alpha = 0.8f;    //パネルの透過率
    private GameObject instance;

    public GlowPanel(Vector3 setPos, Transform parent)
    {
        GameObject prefab = GlowPanelGenerator.InstantiateGlowPanel(setPos);
        prefab.transform.SetParent(parent);
        instance = prefab;

        _renderer = prefab.GetComponent<Renderer>();

        Initialize(prefab.transform);
        ResetPanel();
    }

    private void Initialize(Transform _transform)
    {
        
        //パネルの大きさをボードの大きさによって初期化
        Vector3 panelScale = Vector3.one;
        panelScale.x = (Board.board_rightUp.x - Board.board_leftDown.x) / Board.boardWidth;
        panelScale.z = (Board.board_rightUp.z - Board.board_leftDown.z) / Board.boardWidth;

        //パネルの元々の大きさを正規化 (元々10*10)
        panelScale.x /= 10;
        panelScale.z /= 10;

        _transform.localScale = panelScale;
        
    }

    public void Glow(Color color)
    {
        instance.SetActive(true);
        color.a = alpha; //透過率調整
        _renderer.material.SetColor("glowColor", color);
    }

    public void ResetPanel()
    {
        instance.SetActive(false);
    }
}
