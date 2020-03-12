using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MicrophoneMinigame : MonoBehaviour
{
    [SerializeField]
    private float offsetVolume;
    private float timeForOneClip;
    private AudioClip audioClip;
    int frequancy = 44100;
    public Slider targetSlider;
    public Slider ValueSlider;
    float curVel;
    public float smTime;
    public float sliderSpeed;
    int test = 0;
    private void Start()
    {
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }
        audioClip = Microphone.Start(Microphone.devices[0], true, 1, frequancy);
    }
    void Update()
    {
        SetValueToSlider();

    }
    private void SetValueToSlider()
    {
        ValueSlider.value = Mathf.SmoothDamp(ValueSlider.value, GetValue(audioClip), ref curVel, smTime, sliderSpeed);

    }
    private float GetValue(AudioClip audioClip)
    {

        int dec = 128;
        float[] waveData = new float[dec];
        int micPosition = Microphone.GetPosition(Microphone.devices[0]) - (dec + 1);
        audioClip.GetData(waveData, micPosition);


        float levelMax = 0;
        for (int i = 0; i < dec; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }

        return Mathf.Sqrt(levelMax);
    }
}
