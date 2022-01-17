using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public enum GameState
    {
        paused,
        playing,
        menu
    }

    // Setting values
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
                break;
            case "sound":
                soundVol = value;
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
