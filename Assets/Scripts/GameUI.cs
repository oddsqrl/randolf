using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    bool paused = false;
    public Transform UIBit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused) { Unpause(); paused = false; }
            else { Pause(); paused = true; }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        UIBit.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        UIBit.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
