using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class MicrophoneMinigame : MonoBehaviour
{
   
    private int timeForOneClip = 100;
    private AudioClip audioClip;
    private int frequancy = 44100;
    public Slider targetSlider;
    public Slider ValueSlider;
    private float curVel;
    public float smTime;
    public float sliderSpeed;
    public GameObject windowOk;
    private bool windowOpen = false;
    int test = 0;
    private void Start()
    {

        audioClip = Microphone.Start(Microphone.devices[0], true, timeForOneClip, frequancy);

    }
    void Update()
    {
        SetValueToSlider();

    }
    private void SetValueToSlider()
    {
        ValueSlider.value = Mathf.SmoothDamp(ValueSlider.value, GetValue(audioClip), ref curVel, smTime, sliderSpeed);
        if (targetSlider.value < ValueSlider.value && !windowOpen)
        {
            windowOpen = true;
            windowOk.SetActive(true);
            Invoke("DisableWindow",1f);
        }

    }
    private float GetValue(AudioClip audioClip)
    {

        int dec = 128;
        float[] waveData = new float[dec];
        int micPosition = Microphone.GetPosition(Microphone.devices[0]) - (dec + audioClip.channels);
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
    void DisableWindow()
    {
        windowOk.SetActive(false);
        windowOpen = false;
    }
}
