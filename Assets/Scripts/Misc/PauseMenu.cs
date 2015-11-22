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
        Debug.Log("menuButton.Length: " + menuButtons.Length);
        for(int i = 0; i < menuButtons.Length; ++i) {
            Debug.Log(menuButtons[i].gameObject.name);
        }

        // do this at the end of start, so we get everything in the panel
        mPausePanel.SetActive(false);
        paused = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if(paused) {
            if(Input.GetButtonDown("Pause")) {
                mPausePanel.SetActive(false);
                Time.timeScale = 1;
                paused = false;
            } else if(Input.GetButtonDown("Up")) {
                buttonSelected = (buttonSelected - 1) % menuButtons.Length;
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
}
