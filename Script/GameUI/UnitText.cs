using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitText : MonoBehaviour
{
    [SerializeField]
    private Transform textCanvas = null;
    private Text text;

    private const float fadeSpeed = 2;
    private readonly Vector3 moveSpeed = new Vector3(0, 0.5f, 0);
    private Vector2 firstPos;

    private void Start()
    {
        text = textCanvas.Find("Text").GetComponent<Text>();
        text.text = "";
        text.fontSize = 100;
        firstPos = textCanvas.localPosition;
    }

    void Update()
    {
        //カメラ方向へ向ける
        textCanvas.rotation = Camera.main.transform.rotation;

    }

    public void HealText(string newText)
    {
        text.text = "+" + newText;
        StartCoroutine(FadeText(Color.green));
    }

    public void DamageText(string newText)
    {
        text.text = "-" + newText;
        StartCoroutine(FadeText(Color.red));
    }

    public void LevelUpText()
    {
        text.text = "LevelUp\n" + "HP+" + UnitState.levelUp_heal + ",Atk+" + UnitState.levelUp_atkUp;
        StartCoroutine(FadeText(Color.yellow));
    }


    public IEnumerator FadeText(Color textColor)
    {
        textColor.a = 2;
        text.color = textColor;
        textCanvas.localPosition = firstPos;

        while (textColor.a > 0)
        {
            yield return null;
            
            textColor.a -= Time.deltaTime * fadeSpeed;
            text.color = textColor;

            textCanvas.position += Time.deltaTime * moveSpeed;
        }
    }

}
