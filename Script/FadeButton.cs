using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeButton : MonoBehaviour
{
    public Direction slideDirection = Direction.left;
    
    private float slideDistance = 5f;

    private Vector2 firstPosition;
    private RectTransform rectTransform;

    public enum Direction
    {
        up,
        down,
        left,
        right
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        firstPosition = rectTransform.localPosition;
    }

    private Vector3 GetDirection()
    {
        switch (slideDirection)
        {
            case Direction.down:
                return Vector3.down;
            case Direction.up:
                return Vector3.up;
            case Direction.left:
                return Vector3.left;
            default:
                return Vector3.right;
        }
    }
   
    

    public void FadeOutButton()
    {
        //初期位置から移動
        rectTransform.localPosition -= GetDirection() * slideDistance;

        //StartCoroutine(SlideButton());
    }

    public void FadeInButton()
    {
        //StartCoroutine(SlideButton());
    }

}
