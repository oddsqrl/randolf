using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SliderVal(Slider slide)
    {
        //Get text component with name val (and set to val)
        //Check cur value
        
        string temp = slide.name.Substring(0, slide.name.Length - 6);
        Debug.Log(temp + "   " + slide.value);
        slide.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = slide.value.ToString();
        gameData.SettingsChange(temp, slide.value);
    }

    public void StartGame()
    {
        gameData.SSPlayLevel();
    }

    public void BackToMain()
    {
        gameData.SSMainMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
