﻿using UnityEngine;
using System.Collections;

public class DigDugStory : MonoBehaviour
{
	
	private Player player;
	private DigDug digdug;
	private GameObject mainCamera;
	private GameObject enemies;
	
	private bool meetDigDug = false;
	private bool meetPlayer = false;
	private bool goTowardPlayer = false;
	private bool moveCamera = false;
	private bool startBattle = false;
	private bool moveDigDug = false;
	private bool finish = false;
	private bool digdugDie = false;
	
	
	private float digdugTargetPosition;
	
	private int hitTime = 0;
	
	private float timer = 0.0f, startTimer = 0.0f;

	private AudioSource backgroundMusic;
	private AudioSource crash;
	private AudioSource falling;
	
	void Start ()
	{
		player = GameObject.Find ("Player").GetComponent<Player> ();
		player.SetInStory (false);
		digdug = GameObject.Find ("DigDug").GetComponent<DigDug> ();
		mainCamera = GameObject.Find ("Main Camera") as GameObject;
		mainCamera.GetComponent<FollowCam> ().SetTarget ();
		enemies = GameObject.Find ("Enemies") as GameObject;

		// before using the healthbar, check if stats need to be set:
		if (GameObject.FindGameObjectWithTag ("GameController") != null) {
			GameObject.FindGameObjectWithTag ("GameController").SendMessage ("SetStats", player.mStats);
		}
		
		player.SetLevelLabel ();

		AudioSource[] audioSources = GetComponents<AudioSource> ();
		backgroundMusic = audioSources [0];
		crash = audioSources [1];
		falling = audioSources [2];
	}
	
	void Update ()
	{
		if (!finish) {
			player.PlayRunSound ();
			StartBattle ();
		}
		
		if (moveDigDug && hitTime <= 3) {
			digdug.transform.position += new Vector3 (0.1f, 0, 0f);
			mainCamera.transform.position += new Vector3 (0.1f, 0f, 0f);
			if (player.transform.position.x <= (digdugTargetPosition - 9))
				player.SetMoveRight (true);
			else
				player.SetMoveRight (false);
			if (digdug.transform.position.x >= (digdugTargetPosition - 0.3f)) {
				enemies.GetComponent<Boss1Controller> ().enabled = true;
				enemies.GetComponent<Boss1Controller> ().CreateWave (0);
				moveDigDug = false;
				player.SetMoveRight (false);
				player.SetInStory (false);
				digdug.SetInStory (false);	
				digdug.SetColor ();
				digdug.IncreaseDifficulty ();
			}
		}

		if (digdugDie) {
			if (digdug.transform.position.x >= (digdugTargetPosition - 0.3f)) {
				if (!falling.isPlaying)
					falling.Play ();
				digdug.transform.position += new Vector3 (0f, -0.1f, 0f);
				StartCoroutine (GoToMainMenu ());
			} else {
				digdug.transform.position += new Vector3 (0.05f, 0, 0f);
				mainCamera.transform.position += new Vector3 (0.05f, 0f, 0f);
			}
		}
	}
	
	public void MoveDigDug (int numberOfHit)
	{
		if (numberOfHit <= 3) {
			GameObject.Find ("Enemies").GetComponent<Boss1Controller> ().DestroyWave ();
			GameObject.Find ("Enemies").GetComponent<Boss1Controller> ().IncreaseDifficulty ();
			enemies.GetComponent<Boss1Controller> ().enabled = false;
			moveDigDug = true;
			hitTime = numberOfHit;
			player.SetInStory (true);
			digdug.SetInStory (true);
			digdug.Reset ();
			if (hitTime == 1)
				digdugTargetPosition = digdug.transform.position.x + 13;
			else if (hitTime == 2) {
				digdugTargetPosition = digdug.transform.position.x + 10;
			} else if (hitTime == 3) {
				digdugTargetPosition = digdug.transform.position.x + 13;
			}
			
		}
	}

	public IEnumerator GoToMainMenu ()
	{
		yield return new WaitForSeconds (5.0f);
		GameObject.FindGameObjectWithTag ("GameController").SendMessage ("NextLevel");
	}

	public void DigDugDie ()
	{
		backgroundMusic.Stop ();
		GameObject.Find ("Enemies").GetComponent<Boss1Controller> ().DestroyWave ();
		enemies.GetComponent<Boss1Controller> ().enabled = false;
		digdugDie = true;
		player.SetInStory (true);
		digdug.SetInStory (true);
		digdug.Reset ();
		digdugTargetPosition = digdug.transform.position.x + 5;
	}
	
	public void StartBattle ()
	{
		timer += Time.deltaTime;
		
		if (player.transform.position.x >= 30 && !meetDigDug) {
			meetDigDug = true;
			player.SetCanMove (false);
			player.SetInStory (true);
			digdug.SetCanMove (false);
			digdug.SetInStory (true);
		}
		
		if (meetDigDug && digdug.transform.position.y >= player.transform.position.y) {
			for (int i = 0; i < 5; i++) {
				digdug.transform.position -= new Vector3 (0f, 0.1f, 0f);
				if (!falling.isPlaying)
					falling.Play ();
				if (digdug.transform.position.y <= player.transform.position.y) {
					falling.Stop ();
					crash.Play ();
					meetDigDug = false;
					meetPlayer = true;
					break;
				}
			}
		}
		
		if (meetPlayer) {
			digdug.Pumping ();
			meetPlayer = false;
			goTowardPlayer = true;
		}
		
		if (goTowardPlayer) {
			startTimer += Time.deltaTime;
		}
		
		if (startTimer > 4.0f && !digdug.isPumping () && goTowardPlayer && digdug.transform.position.x >= 2.0f) {
			digdug.transform.position -= new Vector3 (0.3f, 0, 0f);
			if (digdug.transform.position.x <= 2f) {
				backgroundMusic.Play ();
				goTowardPlayer = false;
				moveCamera = true;
				startTimer = 0.0f;
			}
		}
		
		if (moveCamera) {
			mainCamera.GetComponent<FollowCam> ().enabled = false;
			if (mainCamera.transform.position.x <= -3.0f) {
				mainCamera.transform.position += new Vector3 (0.05f, 0f, 0f);
			} else {
				moveCamera = false;
				startBattle = true;
			}
		}
		
		if (startBattle) {
			digdug.SetHealthBar ();
			player.SetInStory (false);
			digdug.SetInStory (false);
			startBattle = false;
			enemies.GetComponent<Boss1Controller> ().enabled = true;
			finish = true;
		}
	}
}
