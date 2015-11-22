using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    GameObject mMenuPanel;
    UnityEngine.UI.Button[] menuButtons;
    private int buttonSelected;

    // Use this for initialization
    void Start()
    {
        mMenuPanel = GameObject.FindGameObjectWithTag("MainMenu");
        menuButtons = mMenuPanel.GetComponentsInChildren<UnityEngine.UI.Button>();
        buttonSelected = 0;
        menuButtons[buttonSelected].Select();

        // check if there exists a save file. If yes, allow for continue.
        // TODO
    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetButtonDown("Up"))
        {
            buttonSelected = (buttonSelected + menuButtons.Length - 1) % menuButtons.Length; // add menuButtons.Length because % allows for negative remainders . . .
            menuButtons[buttonSelected].Select();
        }
        else if (Input.GetButtonDown("Down"))
        {
            buttonSelected = (buttonSelected + 1) % menuButtons.Length;
            menuButtons[buttonSelected].Select();
        }
    }

    void LoadGame()
    {
        buttonSelected = 0;
        Debug.Log("Load Game");
    }

    void NewGame()
    {
        buttonSelected = 1;
        Debug.Log("New Game");
    }

    void Quit()
    {
        buttonSelected = 2;
        Application.Quit();
        Debug.Log("Quit");
        Debug.DebugBreak();
    }
}
