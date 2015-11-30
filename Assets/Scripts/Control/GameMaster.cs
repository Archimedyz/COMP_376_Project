using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

    int mFileLoaded;
    int mStage;
    bool mAtBoss;

	// Use this for initialization
	void Start () {
        mFileLoaded = 0;
        mStage = 0;
        mAtBoss = true;
        PlayerPrefs.SetInt("set_player_stats", 1);
        PlayerPrefs.SetInt("skip_dialogue", 1);
    }

    void LoadLevel() {
        mFileLoaded = PlayerPrefs.GetInt("file_loaded");
        if(mFileLoaded != 0) {
            mStage = PlayerPrefs.GetInt("f" + mFileLoaded + "_stage");
            mAtBoss = PlayerPrefs.GetInt("f" + mFileLoaded + "_at_boss") == 1;
            Application.LoadLevel(2 * mStage - (mAtBoss ? 0 : 1));
        } else {
            mFileLoaded = 0;
            mStage = 0;
            mAtBoss = false;
            Application.LoadLevel(0);
            foreach(Singleton s in GameObject.FindObjectsOfType<Singleton>()) {
                if(s.gameObject.tag != gameObject.tag) {
                    Destroy(s.gameObject);
                }
            }
            PlayerPrefs.SetInt("set_player_stats", 1);
            PlayerPrefs.SetInt("skip_dialogue", 1);
        }
    }

    void NextLevel()
    {   
        // update the stage info. 
        if(mAtBoss) {
            ++mStage;
            mAtBoss = false;
        } else {
            mAtBoss = true;
        }
        SaveGame(); // save the game.
        LoadLevel(); // then load the level.
        
    }

    void SaveGame()
    {
        if(mFileLoaded != 0) {
            // get the player.
            Player playerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            // set the stage data
            PlayerPrefs.SetInt("f" + mFileLoaded + "_stage", mStage);
            PlayerPrefs.SetInt("f" + mFileLoaded + "_at_boss", mAtBoss ? 1 : 0);

            // set the player data
            PlayerPrefs.SetInt("f" + mFileLoaded + "_lvl", playerRef.mStats.Level);
            PlayerPrefs.SetFloat("f" + mFileLoaded + "_max_hp", playerRef.mStats.MaxHp);
            PlayerPrefs.SetFloat("f" + mFileLoaded + "_hp", playerRef.mStats.Hp);
            PlayerPrefs.SetInt("f" + mFileLoaded + "_str", playerRef.mStats.Str);
            PlayerPrefs.SetInt("f" + mFileLoaded + "_def", playerRef.mStats.Def);
            PlayerPrefs.SetInt("f" + mFileLoaded + "_spd", playerRef.mStats.Spd);
        }
    }

    void SetStats(Stats stats)
    {
        Debug.Log("lvl: " + stats.Level + "\nmax_hp: " + stats.MaxHp + "\nhp: " + stats.Hp + "\nstr: " + stats.Str + "\ndef: " + stats.Def + "\nspd: " + stats.Spd);
        stats.init(
            PlayerPrefs.GetInt("f" + mFileLoaded + "_lvl"),
            PlayerPrefs.GetFloat("f" + mFileLoaded + "_hp"),
            PlayerPrefs.GetFloat("f" + mFileLoaded + "_max_hp"),
            PlayerPrefs.GetInt("f" + mFileLoaded + "_str"),
            PlayerPrefs.GetInt("f" + mFileLoaded + "_def"),
            PlayerPrefs.GetInt("f" + mFileLoaded + "_spd"),
            Stats.playerRate
        );
        Debug.Log("File_Loaded = " + mFileLoaded + " - " + 
            PlayerPrefs.GetFloat("f" + mFileLoaded + "_hp") + " - " +
            PlayerPrefs.GetFloat("f" + mFileLoaded + "_max_hp"));
        Debug.Log("lvl: " + stats.Level + "\nmax_hp: " + stats.MaxHp + "\nhp: " + stats.Hp + "\nstr: " + stats.Str + "\ndef: " + stats.Def + "\nspd: " + stats.Spd);
        PlayerPrefs.SetInt("set_player_stats", 0);
    }
}
