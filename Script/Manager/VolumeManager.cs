using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public const string mixerName = "AudioMixer";
    public static UnityEngine.Audio.AudioMixer audioMixer { get; private set; }

    private void Awake()
    {
        //オーディオミキサーを取得
        audioMixer = Resources.Load(mixerName) as UnityEngine.Audio.AudioMixer;
    }


    public void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            audioMixer.SetFloat("Master", 0);
        }
        else
        {
            audioMixer.SetFloat("Master", -80);
        }
    }
}
