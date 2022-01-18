using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


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

    public void EndTimer(bool lvlComp = false)
    {
        endTime = curTime;
        if (lvlComp && bestTime < endTime) { bestTime = endTime; }
    }

    public string TimeFormat(float time)
    {
        string formatted;
        int seconds = Mathf.FloorToInt(time);
        int komma = (Mathf.RoundToInt(time * 100) - seconds * 100);
        formatted = "Time: " + seconds + "." + komma.ToString("D2");
        return formatted;
    }

    public void SSMainMenu()
    {
        SceneManager.LoadScene(0);
    }


    public void SSLevelComplete()
    {
        SceneManager.LoadScene(SceneManager.sceneCount - 2);
    }

    public void SSGameOver()
    {
        SceneManager.LoadScene(SceneManager.sceneCount - 1);
    }
}
