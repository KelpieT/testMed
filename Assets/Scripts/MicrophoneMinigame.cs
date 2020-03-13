using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicrophoneMinigame : MonoBehaviour
{
    private int timeForOneClip = 100;//Time in seconds
    private AudioClip audioClip;
    private int frequancy = 44100;
    public Slider TargetSlider;//Assign in editor
    public Slider ValueSlider;//Assign in editor
    private float curVel;//For SmoothDamp slider value
    public float smTime;//Assign in editor | For SmoothDamp slider value
    public float sliderSpeed;//Assign in editor | For SmoothDamp slider value
    public GameObject windowOk;//Assign in editor
    private bool windowOpen = false;

    private void Start()
    {
        StartRecordAudio();
    }
    void Update()
    {
        SetValueToSlider();
    }
    private void StartRecordAudio()
    {
        audioClip = Microphone.Start(Microphone.devices[0], true, timeForOneClip, frequancy);
    }
    private void SetValueToSlider()
    {
        ValueSlider.value = Mathf.SmoothDamp(ValueSlider.value, GetValue(audioClip), ref curVel, smTime, sliderSpeed);
        if (TargetSlider.value > ValueSlider.value)
        {
            windowOpen = false;
        }
        else if (!windowOpen)
        {
            windowOpen = true;
            windowOk.SetActive(true);
            Invoke("DisableWindow", 1f);
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
    private void DisableWindow()//Use for Invoke
    {
        windowOk.SetActive(false);
    }
}
