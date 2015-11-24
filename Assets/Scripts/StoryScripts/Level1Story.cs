using UnityEngine;
using System.Collections;

public class Level1Story : MonoBehaviour
{
	private AudioSource theme;

	private Player player;
	private GameObject mainCamera;
	private GameObject dialogue;
	private Dialogue dialogueText;

	private bool scottArrives = true;
	private bool scottStopMove = false;
	private bool hoodedStartTalking = false;
	
	void Start ()
	{
		player = GameObject.Find ("Player").GetComponent<Player> ();
		mainCamera = GameObject.Find ("Main Camera") as GameObject;
		dialogue = GameObject.Find ("Dialogue") as GameObject;
		dialogueText = GameObject.Find ("DialogueText").GetComponent<Dialogue> ();
		dialogue.SetActive (false);
		//enemies = GameObject.Find ("Enemies") as GameObject;
		//hoodedCharacter = GameObject.Find ("Enemies") as GameObject;

		AudioSource[] audioSources = GetComponents<AudioSource> ();
		theme = audioSources [0];
		player.SetInStory (true);
	}

	void Update ()
	{
		/*if (!theme.isPlaying) {
			theme.Play ();
		}*/

		//TODO Arrives in metro

		if (scottArrives) {
			player.SetMoveUp (true);
			scottArrives = false;
			scottStopMove = true;
		}

		if (scottStopMove) {
			if (player.transform.position.y >= -3.5f) {
				player.SetMoveUp (false);
				scottStopMove = false;
				//TODO Character can move ?
				hoodedStartTalking = true;
			}
		}

		//TODO Something in between ?

		if (hoodedStartTalking) {
			dialogue.SetActive (true);
			dialogueText.SelectTextFile ("FirstScene");
			hoodedStartTalking = false;
		}
	}
}
