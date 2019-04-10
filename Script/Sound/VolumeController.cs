using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeController : MonoBehaviour
{
    [SerializeField]
    private VolumeType volumeType = VolumeType.BgmVolume;
    [SerializeField]
    private Toggle muteToggle = null;


    //スライダーで設定できる最小音量
    private const float lowestDecibel = -40;
    //ミュート時はデシベル値を-80に設定
    private bool mute;
    //Sliderコンポーネントへの参照
    private Slider slider;
    
    private enum VolumeType
    {
        BgmVolume,
        SEVolume
    }

    private void Start()
    {
        slider = GetComponent<Slider>();
        Initialize();
    }
    

    private void Initialize()
    {
        //オーディオミキサーの値を取得
        float value;
        VolumeManager.audioMixer.GetFloat(volumeType.ToString(), out value);

        //スライダーの位置を初期化
        slider.value = (value - lowestDecibel) / -lowestDecibel;

        //トグルの初期化
        if (value >= lowestDecibel)
            muteToggle.isOn = false;
        else
            muteToggle.isOn = true;
    }
    

    public void VolumeChange()
    {
        if (mute)
            VolumeManager.audioMixer.SetFloat(volumeType.ToString(), -80);
        else
            VolumeManager.audioMixer.SetFloat(volumeType.ToString(), Mathf.Lerp(lowestDecibel, 0, slider.value));
    }

    public void MuteSwitch(bool  boolean)
    {
        mute = boolean;

        VolumeChange();
    }
    
}
