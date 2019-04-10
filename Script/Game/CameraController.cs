using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController singleton;

    private static int reverseFlag = -1;
    private static bool pauseFlag = true;
    
    private static Vector3 forcus = new Vector3(0,1,0);
    private const float moveSpeed = 1.5f;
    private const float rotateSpeed = 1.5f;
    private const float closeSpeed = 7.0f;

    private const float maxDistance = 15f;
    private const float closenessRange_max = 60f;
    private const float closenessRange_min = 10f;
    private readonly Vector3 origin = Vector3.zero;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        transform.LookAt(forcus);
    }

    private void Update()
    {
        if (pauseFlag)
            return;

        //カメラの前後移動
        Vector3 distance = transform.forward * closeSpeed * Input.GetAxis("Mouse ScrollWheel");
        Vector3 movedPos = Camera.main.transform.position + distance;

        if ((movedPos - forcus).magnitude > closenessRange_min && (movedPos - forcus).magnitude < closenessRange_max)
        {
            Camera.main.transform.position += distance;
        }


        //フォーカスの移動
        if (Input.GetMouseButton(2))
        {
            //マウスの入力
            Vector3 mouseMovement = transform.right * Input.GetAxis("Mouse X") + transform.up * Input.GetAxis("Mouse Y");

            //入力が無い場合は終了
            if (mouseMovement.magnitude == 0)
                return;
            if (!IsMovableRange(forcus+ mouseMovement * moveSpeed * reverseFlag))
                return;

            forcus += mouseMovement * moveSpeed * reverseFlag;
            transform.position += mouseMovement * moveSpeed * reverseFlag;

        }

        //カメラの球面移動
        if (Input.GetMouseButton(1))
        {
            //マウスの入力
            Vector3 mouseMovement = transform.right * Input.GetAxis("Mouse X") + transform.up * Input.GetAxis("Mouse Y");

            //入力が無い場合は終了
            if (mouseMovement.magnitude == 0) return;


            //カメラから焦点へのベクトル
            Vector3 camToForcus = forcus - transform.position;
            //回転軸
            Vector3 rotateAxis = Vector3.Cross(mouseMovement, camToForcus);

            transform.RotateAround(forcus, rotateAxis, reverseFlag * rotateSpeed);
            transform.LookAt(forcus);
            
        }
    }

    public static void ReverseFlag()
    {
        if (reverseFlag == 1)
            reverseFlag = -1;
        else
            reverseFlag = 1;
    }

    public static void ReversePosition()
    {
        singleton.transform.RotateAround(forcus, Vector3.up, reverseFlag * 180);
        singleton.transform.LookAt(forcus);
    }

    private bool IsMovableRange(Vector3 newPosition)
    {
        if ((newPosition - origin).magnitude < maxDistance)
            return true;
        else
            return false;
    }

    public static void Pause()
    {
        pauseFlag = true;
    }
    public static void PauseEnd()
    {
        pauseFlag = false;
    }
}