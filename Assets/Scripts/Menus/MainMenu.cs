using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    private int activeMenu;

    GameObject[] mainMenuPanels;
    GameObject[] loadButtonPanels;
    GameObject[] newButtonPanels;
    UnityEngine.UI.Button[][] mainMenuButtons;
    private int buttonSelected;

    private int mainMenuIndex = -1;
    private int newMenuIndex = -1;
    private int loadMenuIndex = -1;


    // Use this for initialization
    void Start()
    {
        mainMenuPanels = GameObject.FindGameObjectsWithTag("MenuPanel");
        mainMenuButtons = new UnityEngine.UI.Button[mainMenuPanels.Length][];

         //* REMOVE

        PlayerPrefs.SetInt("f1_stage", 0);
        PlayerPrefs.SetInt("f2_stage", 0);
        PlayerPrefs.SetInt("f3_stage", 0);

        // */

        Debug.Log("Length: " + mainMenuPanels.Length);

        for (int i = 0; i < mainMenuPanels.Length; ++i)
        {
            mainMenuButtons[i] = mainMenuPanels[i].GetComponentsInChildren<UnityEngine.UI.Button>();
            Debug.Log(mainMenuPanels[i].name + " - Buttons[" + i + "]: " + mainMenuButtons[i].Length);
            mainMenuButtons[i][0].Select();

            if(mainMenuPanels[i].name == "MainPanel") {
                mainMenuIndex = i;
            } else if(mainMenuPanels[i].name == "NewPanel") {
                newMenuIndex = i;
                newButtonPanels = new GameObject[mainMenuButtons[i].Length - 1]; // minus the back button
                for(int j = 0; j < newButtonPanels.Length; ++j) {
                    newButtonPanels[j] = mainMenuButtons[i][j].gameObject.GetComponentsInChildren<RectTransform>()[2].gameObject;
                }
            } else if(mainMenuPanels[i].name == "LoadPanel") {
                loadMenuIndex = i;
                loadButtonPanels = new GameObject[mainMenuButtons[i].Length - 1]; // minus the back button
                for(int j = 0; j < newButtonPanels.Length; ++j) {
                    loadButtonPanels[j] = mainMenuButtons[i][j].gameObject.GetComponentsInChildren<RectTransform>()[2].gameObject;
                }
            }

            if(i != mainMenuIndex) {
                mainMenuPanels[i].SetActive(false);
            }
        }

        activeMenu = mainMenuIndex;
        buttonSelected = 0;

        // check if there exists a save file. If yes, allow for continue.
        int gamesSaved = PlayerPrefs.GetInt("games_saved");
        if(gamesSaved == 0) {
            mainMenuButtons[mainMenuIndex][0].gameObject.SetActive(false);
            mainMenuButtons[mainMenuIndex][1].Select();            
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
        mainMenuPanels[activeMenu].SetActive(true);
        mainMenuPanels[mainMenuIndex].SetActive(false);
    }

    void NewGame()
    {
        buttonSelected = 0;
        Debug.Log("New Game");
        activeMenu = newMenuIndex;
        mainMenuPanels[activeMenu].SetActive(true);
        mainMenuPanels[mainMenuIndex].SetActive(false);

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
        mainMenuPanels[activeMenu].SetActive(true);
        mainMenuPanels[newMenuIndex].SetActive(false);
        mainMenuPanels[loadMenuIndex].SetActive(false);
    }

    void NewFile1()
    {
        Debug.Log("New Game - File 1");

        foreach (UnityEngine.UI.Text txt in newButtonPanels[0].GetComponentsInChildren<UnityEngine.UI.Text>())
        {
            Debug.Log("NEW: " + txt.gameObject.name);
        }

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
