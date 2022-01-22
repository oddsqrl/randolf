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
    public int curWantedLevel = 1;

    public float FOV = 60;
    public float sensitivity = 6;
    public float musicVol = 0;
    public float soundVol = 0;

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
        if (lvlComp && bestTime > endTime) { bestTime = endTime; }
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


    public void SSLevelComplete()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 2);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SSGameOver()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void SSPlayLevel()
    {
        SceneManager.LoadScene(curWantedLevel);
    }
}
