using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    GameObject mPausePanel;
    private bool paused;

	// Use this for initialization
	void Start () {
        mPausePanel = GetComponentInChildren<RectTransform>().gameObject;
        paused = false;
	}
	
	// Update is called once per frame
	void Update () {
	    if(paused) {
            if(Input.GetButtonDown("Pause")) {
                Debug.Log("Resuming.");
                mPausePanel.SetActive(false);
                Time.timeScale = 1;
                paused = false;
            }
        } else {
            if (Input.GetButtonDown("Pause")) {
                Debug.Log("Pausing.");
                mPausePanel.SetActive(true);
                Time.timeScale = 0;
                paused = true;
            }
        }
	}
}
