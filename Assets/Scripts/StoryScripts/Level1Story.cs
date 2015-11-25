﻿using UnityEngine;
using System.Collections;

public class Level1Story : MonoBehaviour
{
	private AudioSource theme;

	public Player player;
	private GameObject mainCamera;
	private GameObject dialogue;
	private Dialogue dialogueText;

	private bool metroArrives = true;
	private bool scottArrives = false;
	private bool scottStopMove = false;
	private bool hoodedStartTalking = false;

	public GameObject metroPrefab;
	private GameObject metro;
	private float metroTarget;
	private float startTime;
	private float journeyLength;
	
	void Start ()
	{
		metro = Instantiate (metroPrefab, new Vector3 (60f, -5.5f, -1f), Quaternion.identity) as GameObject;
		metro.GetComponent<Metro> ().enabled = false;
		startTime = Time.time;

		metroTarget = -60f;
		journeyLength = Mathf.Abs (60) + Mathf.Abs (metroTarget);

		mainCamera = GameObject.Find ("Main Camera") as GameObject;
		mainCamera.GetComponent<FollowCam> ().SetTarget (metro);

		dialogue = GameObject.Find ("Dialogue") as GameObject;
		dialogueText = GameObject.Find ("DialogueText").GetComponent<Dialogue> ();
		dialogue.SetActive (false);

		//enemies = GameObject.Find ("Enemies") as GameObject;
		//hoodedCharacter = GameObject.Find ("Enemies") as GameObject;

		AudioSource[] audioSources = GetComponents<AudioSource> ();
		theme = audioSources [0];
	}

	void Update ()
	{
		/*if (!theme.isPlaying) {
			theme.Play ();
		}*/

		//TODO Arrives in metro
		if (metroArrives) {
			float distCovered = (Time.time - startTime) * 0.5f;
			float fracJourney = distCovered / journeyLength;
			metro.transform.position = Vector3.Lerp (metro.transform.position, new Vector3 (metroTarget, metro.transform.position.y, metro.transform.position.z), fracJourney);
			if (metro.transform.position.x <= -59f) {
				metroArrives = false;
				scottArrives = true;
			}
		}

		if (scottArrives) {
			player.gameObject.SetActive (true);
			player.SetInStory (true);
			mainCamera.GetComponent<FollowCam> ().SetTarget ();
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
