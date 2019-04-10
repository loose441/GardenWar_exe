using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhasePanel : MonoBehaviour
{
    public static PhasePanel singleton;
    private static Text text;
    private static Animator animator;
    private static RectTransform rectTransform;
    private static readonly Color[] colorIndex = new Color[]
    {
        Color.red,
        Color.white
    };


    public enum ColorVariety
    {
        Red,
        White
    }

    
    private void Start()
    {
        singleton = this;
        animator = singleton.gameObject.GetComponent<Animator>();
        text = singleton.transform.GetComponentInChildren<Text>();
        
    }

    public static void PhaseTrans(string phaseName, ColorVariety color)
    {
        PlayerController.Pause();
        text.text = phaseName;
        text.color = colorIndex[(int)color];

        animator.SetTrigger("Trans");
    }

    //アニメーターで設定
    public void TransEnd()
    {
        PlayerController.PauseEnd();
    }
}
