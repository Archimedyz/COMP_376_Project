using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    GameObject gameMasterRef;

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
        gameMasterRef = GameObject.FindGameObjectWithTag("GameController");

        mainMenuPanels = GameObject.FindGameObjectsWithTag("MenuPanel");
        mainMenuButtons = new UnityEngine.UI.Button[mainMenuPanels.Length][];

        //* REMOVE
        for (int i = 1; i < 4; ++i )
        {
            PlayerPrefs.DeleteKey("f" + i + "_rate0");
            PlayerPrefs.DeleteKey("f" + i + "_rate1");
            PlayerPrefs.DeleteKey("f" + i + "_rate2");
            PlayerPrefs.DeleteKey("f" + i + "_rate3");
        }
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
                    initLoadData(newButtonPanels[j], j + 1);                    
                }
            } else if(mainMenuPanels[i].name == "LoadPanel") {
                loadMenuIndex = i;
                loadButtonPanels = new GameObject[mainMenuButtons[i].Length - 1]; // minus the back button
                for(int j = 0; j < newButtonPanels.Length; ++j) {
                    loadButtonPanels[j] = mainMenuButtons[i][j].gameObject.GetComponentsInChildren<RectTransform>()[2].gameObject;
                    initLoadData(loadButtonPanels[j], j + 1);
                }
            }

            if(i != mainMenuIndex) {
                mainMenuPanels[i].SetActive(false);
            }
        }

        activeMenu = mainMenuIndex;
        buttonSelected = 0;

        // check if there exists a save file. If yes, allow for continue.
        int gamesSaved = PlayerPrefs.GetInt("files_saved");
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
        if(PlayerPrefs.GetInt("f1_saved") == 1) { // save file exists.
            PlayerPrefs.SetInt("file_loaded", 1);
            gameMasterRef.SendMessage("LoadLevel");
        }
    }

    void LoadFile2()
    {
        if(PlayerPrefs.GetInt("f2_saved") == 1) { // save file exists.
            PlayerPrefs.SetInt("file_loaded", 2);
            gameMasterRef.SendMessage("LoadLevel");
        }
    }

    void LoadFile3()
    {
        if(PlayerPrefs.GetInt("f3_saved") == 1) { // save file exists.
            PlayerPrefs.SetInt("file_loaded", 3);
            gameMasterRef.SendMessage("LoadLevel");
        }
    }

    void initLoadData(GameObject btnPanel, int file)
    {
        if(PlayerPrefs.GetInt("f" + file + "_saved") == 1) {
            foreach (UnityEngine.UI.Text txt in btnPanel.GetComponentsInChildren<UnityEngine.UI.Text>())
            {
                ShowButtonStats(btnPanel, true);
                if(!txt.gameObject.name.StartsWith("_")) {
                    if (txt.gameObject.name == "stage") {
                        txt.text = PlayerPrefs.GetInt("f" + file + "_stage").ToString() + (PlayerPrefs.GetInt("f" + file + "_at_boss") == 1 ? " Boss" : "");
                    } else {
                        txt.text = PlayerPrefs.GetInt("f" + file + "_" + txt.gameObject.name).ToString();
                    }
                } 
            } 
        } else {
            ShowButtonStats(btnPanel, false);
        }
    }

    void ShowButtonStats(GameObject btnPanel, bool val) {
        btnPanel.transform.parent.gameObject.GetComponentsInChildren<RectTransform>()[1].gameObject.SetActive(!val);
        btnPanel.SetActive(val);
    }
}
