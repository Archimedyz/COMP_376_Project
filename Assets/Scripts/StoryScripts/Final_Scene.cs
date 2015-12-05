using UnityEngine;
using System.Collections;

public class Final_Scene : MonoBehaviour
{
	private bool start = false;

	private GameObject player;
	private GameObject hooded;
	private GameObject fevens;
	
	private GameObject dialogue;
	private Dialogue dialogueText;

	void Start ()
	{
		player = GameObject.Find ("Player") as GameObject;
		hooded = GameObject.Find ("HoodedCharacter") as GameObject;
		fevens = GameObject.Find ("EvilFevens") as GameObject;

		dialogue = GameObject.Find ("Dialogue") as GameObject;
		dialogueText = GameObject.Find ("DialogueText").GetComponent<Dialogue> ();
		dialogue.SetActive (false);
	}

	void Update ()
	{
		if (start) {
			start = false;
			dialogue.SetActive (true);
			dialogueText.SelectTextFile ("Level1End");
		}
	}

	public void SetStart ()
	{
		start = true;
	}

	public void SetGameEnd ()
	{

	}
}
