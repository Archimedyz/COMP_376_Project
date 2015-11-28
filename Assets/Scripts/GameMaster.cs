using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }

    void LoadLevel() {
        int file_loaded = PlayerPrefs.GetInt("file_loaded");
        if(file_loaded != 0) {
            int stage = PlayerPrefs.GetInt("f" + file_loaded + "_stage");
            int at_boss = PlayerPrefs.GetInt("f" + file_loaded + "_at_boss");
            Application.LoadLevel(2 * stage - 1 + at_boss);
        } else {
            Application.LoadLevel(0);
            foreach(Singleton s in GameObject.FindObjectsOfType<Singleton>()) {
                if(s.gameObject.tag != gameObject.tag) {
                    Destroy(s.gameObject);
                }
            }
        }
    }

    void NextLevel()
    {
        Application.LoadLevel(Application.loadedLevel + 1);
    }
}
