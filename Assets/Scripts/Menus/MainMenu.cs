using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    private int activeMenu;

    GameObject[] mMenuPanels;
    UnityEngine.UI.Button[][] mainMenuButtons;
    private int buttonSelected;

    private int mainMenuIndex = -1;
    private int newMenuIndex = -1;
    private int loadMenuIndex = -1;


    // Use this for initialization
    void Start()
    {
        mMenuPanels = GameObject.FindGameObjectsWithTag("MenuPanel");
        mainMenuButtons = new UnityEngine.UI.Button[mMenuPanels.Length][];

        Debug.Log("Length: " + mMenuPanels.Length);

        for (int i = 0; i < mMenuPanels.Length; ++i)
        {
            mainMenuButtons[i] = mMenuPanels[i].GetComponentsInChildren<UnityEngine.UI.Button>();
            Debug.Log(mMenuPanels[i].name + " - Buttons[" + i + "]: " + mainMenuButtons[i].Length);
            mainMenuButtons[i][0].Select();

            if(mMenuPanels[i].name == "MainPanel") {
                mainMenuIndex = i;
            } else if(mMenuPanels[i].name == "NewPanel") {
                newMenuIndex = i;
            } else if(mMenuPanels[i].name == "LoadPanel") {
                loadMenuIndex = i;
            }

            if(i != mainMenuIndex) {
                mMenuPanels[i].SetActive(false);
            }
        }

        activeMenu = mainMenuIndex;
        buttonSelected = 0;

        // check if there exists a save file. If yes, allow for continue.
        int gamesSaved = PlayerPrefs.GetInt("games_saved");
        if(gamesSaved == 0) {
            //Destroy(mainMenuButtons[mainMenuIndex][0].gameObject);
            mainMenuButtons[mainMenuIndex][0].Select();            
        }
    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetButtonDown("Up"))
        {
            buttonSelected = (buttonSelected + mainMenuButtons[activeMenu].Length - 1) % mainMenuButtons[activeMenu].Length; // add menuButtons.Length because % allows for negative remainders . . .
            mainMenuButtons[activeMenu][buttonSelected].Select();
        }
        else if (Input.GetButtonDown("Down"))
        {
            buttonSelected = (buttonSelected + 1) % mainMenuButtons[activeMenu].Length;
            mainMenuButtons[activeMenu][buttonSelected].Select();
        }
    }

    void LoadGame()
    {
        buttonSelected = 0;
        Debug.Log("Load Game");
        activeMenu = loadMenuIndex;
        mMenuPanels[activeMenu].SetActive(true);
        mMenuPanels[mainMenuIndex].SetActive(false);
    }

    void NewGame()
    {
        buttonSelected = 0;
        Debug.Log("New Game");
        activeMenu = newMenuIndex;
        mMenuPanels[activeMenu].SetActive(true);
        mMenuPanels[mainMenuIndex].SetActive(false);

    }

    void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
        Debug.DebugBreak();
    }

    void Back()
    {
        activeMenu = mainMenuIndex;
        mMenuPanels[activeMenu].SetActive(true);
        mMenuPanels[newMenuIndex].SetActive(false);
        mMenuPanels[loadMenuIndex].SetActive(false);
    }

    void NewFile1()
    {
        Debug.Log("New Game - File 1");
    }

    void NewFile2()
    {
        Debug.Log("New Game - File 2");
    }

    void NewFile3()
    {
        Debug.Log("New Game - File 3");
    }

    void LoadFile1()
    {
        Debug.Log("Load Game - File 1");
    }

    void LoadFile2()
    {
        Debug.Log("Load Game - File 2");
    }

    void LoadFile3()
    {
        Debug.Log("Load Game - File 3");
    }
}
