


using UnityEngine;
using UnityEngine.UI;
using System.Collections;


// VERY SIMPLE SCRIPT TO TOGGLE ON/OFF SCREEN CONTROLS


public class OnScreenControls : MonoBehaviour {

	private bool isToggled; 
	public GameObject sThrow, sPlace, sPickUp, blasts, dash, normal, strong, defend;

	// Use this for initialization
	void Start () {
		isToggled = true;

		sThrow = GameObject.Find ("ShurikenThrow");
		sPlace = GameObject.Find ("ShurikenPlace");
		sPickUp = GameObject.Find ("ShurikenPickUp");
		blasts = GameObject.Find ("BlastLeft");
		dash = GameObject.Find ("Dash");
		normal = GameObject.Find ("Normal/Slide");
		strong = GameObject.Find ("Strong");
		defend = GameObject.Find ("Defend"); 
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Y) || Input.GetKeyDown (KeyCode.F)) 
		{
			if(isToggled)  // turn off
			{
				sThrow.SetActive(false);
				sPlace.SetActive(false);
				sPickUp.SetActive(false); 
				blasts.SetActive(false);
				dash.SetActive(false);
				normal.SetActive(false); 
				strong.SetActive(false);
				defend.SetActive(false);

				isToggled = false;
			}
			else  // turn on
			{
				sThrow.SetActive(true);
				sPlace.SetActive(true);
				sPickUp.SetActive(true); 
				blasts.SetActive(true);
				dash.SetActive(true);
				normal.SetActive(true);
				strong.SetActive(true);
				defend.SetActive(true);
				
				isToggled = true;
			}
		}
	}
}