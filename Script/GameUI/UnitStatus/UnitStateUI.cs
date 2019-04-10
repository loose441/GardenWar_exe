using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitStateUI
{
    public GameObject instance { get; private set; }
    private Image backGround;
    private Text hpText;
    private Slider hpSlider;

    private UnitBase unitBase;

    const float trasSpeed = 3;
    private readonly Color[] colorIndex = new Color[]
    {
        new Color(1,1,1,0.78f),
        Color.black,
        Color.yellow
    };


    public enum ColorVariety
    {
        White,
        Black,
        Yellow
    }

    public UnitStateUI(GameObject _instance, UnitBase unit)
    {
        instance = _instance;
        unitBase = unit;
        
        //HP欄への参照
        hpText = instance.transform.Find("HPText").GetComponent<Text>();
        hpSlider = instance.transform.Find("HPSlider").GetComponent<Slider>();


        //背景への参照
        backGround = instance.transform.Find("BackGround").GetComponent<Image>();



        //名前欄の初期化
        Text nameText = instance.transform.Find("Name").GetComponent<Text>();
        nameText.text = unit.unitName;

        //HP欄の初期化
        UpdateHPText(unitBase.maxHp);
        hpSlider.value = 1;
        

    }

    private void UpdateHPText(int newHp)
    {
        hpText.text = newHp.ToString() + "/" + unitBase.maxHp.ToString();
    }

    public IEnumerator TransBackGroundColor(ColorVariety color)
    {
        //float time = 0;
        Color firstColor = backGround.color;
        Color finalColor = colorIndex[(int)color];

        backGround.color = finalColor;

        yield break;
        /*
        while (time < 1)
        {
            yield return null;

            time += Time.deltaTime * trasSpeed;
            backGround.color = Color.Lerp(firstColor, finalColor, time);
        }
        */
    }

    public IEnumerator TransHpBar(float newHp)
    {
        float time = 0;
        float currentHP = hpSlider.value;

        //テキスト更新
        UpdateHPText((int)newHp);

        while (time < 1)
        {
            yield return null;

            time += Time.deltaTime * trasSpeed;
            if (time > 1)
                time = 1;
            hpSlider.value = ((newHp / unitBase.maxHp - currentHP) * time + currentHP);
        }
    }



}
