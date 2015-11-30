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
    private int deleteMenuIndex = -1;


    private const int default_stage = 1;
    private const int default_at_boss = 0;
    private const int default_lvl = 1;
    private const float default_max_hp = 60;
    private const float default_hp = 60;
    private const int default_str = 10;
    private const int default_def = 5;
    private const int default_spd = 5;

    private int fileToDelete;

    // Use this for initialization
    void Start()
    {
        gameMasterRef = GameObject.FindGameObjectWithTag("GameController");

        mainMenuPanels = GameObject.FindGameObjectsWithTag("MenuPanel");
        mainMenuButtons = new UnityEngine.UI.Button[mainMenuPanels.Length][];

        PlayerPrefs.SetInt("game_loaded", 0);

        //* REMOVE
        
        //*/

        for (int i = 0; i < mainMenuPanels.Length; ++i)
        {
            mainMenuButtons[i] = mainMenuPanels[i].GetComponentsInChildren<UnityEngine.UI.Button>();
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
            } else if (mainMenuPanels[i].name == "DeletePanel") {
                deleteMenuIndex = i;
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
        activeMenu = loadMenuIndex;
        mainMenuPanels[activeMenu].SetActive(true);
        mainMenuPanels[mainMenuIndex].SetActive(false);
        // update all buttons in case they've changed
        for(int i = 0; i < 3; ++i) { 
            initLoadData(loadButtonPanels[i], i + 1);
        }
    }

    void NewGame()
    {
        buttonSelected = 0;
        activeMenu = newMenuIndex;
        mainMenuPanels[activeMenu].SetActive(true);
        mainMenuPanels[mainMenuIndex].SetActive(false);
    }

    void Quit()
    {
        Application.Quit();
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
        if(PlayerPrefs.GetInt("f1_saved") == 1) {
            // ask to overwrite the save file.
            ShowDeleteFileDialogue(1);
        } else {
            // start a new game with the defaults.
            InitNewFile(1);
            LoadFile1();
        }
    }

    void NewFile2()
    {
        if (PlayerPrefs.GetInt("f2_saved") == 1) {
            // ask to overwrite the save file.
            ShowDeleteFileDialogue(2);
        } else {
            // start a new game with the defaults.
            InitNewFile(2);
            LoadFile1();
        }
    }

    void NewFile3()
    {
       if(PlayerPrefs.GetInt("f3_saved") == 1) {
            // ask to overwrite the save file.
            ShowDeleteFileDialogue(3);
        } else {
            // start a new game with the defaults.
            InitNewFile(3);
            LoadFile1();
        }
    }

    void InitNewFile(int file_num)
    {
        PlayerPrefs.SetInt("files_saved", PlayerPrefs.GetInt("files_saved") + 1);
        PlayerPrefs.SetInt("f" + file_num + "_saved", 1);
        PlayerPrefs.SetInt("f" + file_num + "_stage", default_stage);
        PlayerPrefs.SetInt("f" + file_num + "_at_boss", default_at_boss);
        PlayerPrefs.SetInt("f" + file_num + "_lvl", default_lvl);
        PlayerPrefs.SetFloat("f" + file_num + "_max_hp", default_max_hp);
        Debug.Log("file: " + file_num + " - " + PlayerPrefs.GetFloat("f"+file_num+"_max_hp"));
        PlayerPrefs.SetFloat("f" + file_num + "_hp", default_hp);
        PlayerPrefs.SetInt("f" + file_num + "_str", default_str);
        PlayerPrefs.SetInt("f" + file_num + "_def", default_def);
        PlayerPrefs.SetInt("f" + file_num + "_spd", default_spd);
    }

    
    void ShowDeleteFileDialogue(int fileToDelete)
    {
        this.fileToDelete = fileToDelete;
        activeMenu = deleteMenuIndex;
        buttonSelected = 1;
        mainMenuPanels[deleteMenuIndex].SetActive(true);
    }

    void DeleteConfirm()
    {
        DeleteFile(fileToDelete);
        initLoadData(newButtonPanels[fileToDelete-1], fileToDelete);
        DeleteCancel();
    }
    
    void DeleteCancel()
    {
        fileToDelete = 0;
        activeMenu = newMenuIndex;
        buttonSelected = 0;
        mainMenuPanels[deleteMenuIndex].SetActive(false);
    }

    void DeleteFile(int file_num)
    {
        PlayerPrefs.SetInt("files_saved", PlayerPrefs.GetInt("files_saved") - 1);
        PlayerPrefs.SetInt("f" + file_num + "_saved", 0);
        PlayerPrefs.SetInt("f" + file_num + "_stage", 0);
        PlayerPrefs.SetInt("f" + file_num + "_at_boss", 0);
        PlayerPrefs.SetInt("f" + file_num + "_lvl", 0);
        PlayerPrefs.SetFloat("f" + file_num + "_max_hp", 0.0f);
        PlayerPrefs.SetFloat("f" + file_num + "_hp", 0.0f);
        PlayerPrefs.SetInt("f" + file_num + "_str", 0);
        PlayerPrefs.SetInt("f" + file_num + "_def", 0);
        PlayerPrefs.SetInt("f" + file_num + "_spd", 0);
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
