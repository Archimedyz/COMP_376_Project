using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    GameObject mPausePanel;
    UnityEngine.UI.Button[] menuButtons;
    private bool paused;
    private int buttonSelected;

	// Use this for initialization
	void Start () {
        mPausePanel = GameObject.FindGameObjectWithTag("PauseMenu");
        menuButtons = mPausePanel.GetComponentsInChildren<UnityEngine.UI.Button>();
        // do this at the end of start, so we get everything in the panel
        mPausePanel.SetActive(false);
        paused = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if(paused) {
            if(Input.GetButtonDown("Pause")) {
                Resume();
            } else if(Input.GetButtonDown("Up")) {
                buttonSelected = (buttonSelected + menuButtons.Length - 1) % menuButtons.Length; // add menuButtons.Length because % allows for negative remainders . . .
                menuButtons[buttonSelected].Select();
            } else if(Input.GetButtonDown("Down")) {
                buttonSelected = (buttonSelected + 1) % menuButtons.Length;
                menuButtons[buttonSelected].Select();
            }
        } else {
            if (Input.GetButtonDown("Pause")) {
                mPausePanel.SetActive(true);
                buttonSelected = 0;
                menuButtons[buttonSelected].Select();
                Time.timeScale = 0;
                paused = true;
            }
        }
	}

    void Resume()
    {
        buttonSelected = 0;
        mPausePanel.SetActive(false);
        Time.timeScale = 1;
        paused = false;
    }

    void Save()
    {
        buttonSelected = 1;
        Debug.Log("Save");
    }

    void MainMenu()
    {
        // get the game controller.
        PlayerPrefs.SetInt("file_loaded", 0);
        Time.timeScale = 1.0f;
        GameObject.FindGameObjectWithTag("GameController").SendMessage("LoadLevel");
    }
}
