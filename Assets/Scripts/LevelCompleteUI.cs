using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCompleteUI : MonoBehaviour
{
    public GameData gameData;
    public TextMeshProUGUI cur;
    public TextMeshProUGUI best;

    void Start()
    {
        cur.text = "Time: " + gameData.TimeFormat(gameData.curTime);
        best.text = "Best: " + gameData.TimeFormat(gameData.bestTime);
    }
}
