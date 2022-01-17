using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public enum GameState
    {
        paused,
        playing
    }

    // Setting values
    public AudioMixer mixer;

    public float FOV;
    public float sensitivity;
    public float musicVol;
    public float soundVol;

    // Timer values ingame
    public float curTime;
    public float startTime;
    public float bestTime;
    public float endTime;
    public GameState curGameState;

    public void ResetValues()
    {
        FOV = 60;
        sensitivity = 6;
        musicVol = 0;
        soundVol = 0;
    }

    public void SettingsChange(string toChange, float value)
    {
        switch (toChange.ToLower())
        {
            case "fov":
                FOV = value;
                break;
            case "sensitivity":
                sensitivity = value;
                break;
            case "music":
                musicVol = value;
                mixer.SetFloat("music", value);
                break;
            case "sound":
                soundVol = value;
                mixer.SetFloat("sound", value);
                break;
        }
    }

    public void ResetTime()
    {
        curGameState = GameState.paused;
        curTime = 0;
        startTime = 0;
    }

    public void StartTimer(float passTime)
    {
        if(curGameState != GameState.playing) startTime = passTime;
        curGameState = GameState.playing;
    }

    public void Timer(float passTime)
    {
        curTime = passTime - startTime;
    }

    public void endTimer()
    {
        endTime = curTime;
        if(bestTime < endTime) { bestTime = endTime; }
    }
}
