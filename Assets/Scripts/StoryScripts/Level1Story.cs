using UnityEngine;
using System.Collections;

public class Level1Story : MonoBehaviour
{
	private AudioSource theme;
	private AudioSource terraTheme;

	public Player player;
	private HoodedCharacter hooded;
	private HoodedCharacter hoodedEnd;
	private GameObject mainCamera;
	private GameObject dialogue;
	private GameObject enemies;
	private GameObject metroSpawners;
	private GameObject levelStuff;
	private Dialogue dialogueText;
	private GameObject pause;
	private GameObject pickUps;
	private GameObject healthbar;
	private GameObject levelText;
	private GameObject control;

	private bool canStart = false;
	private bool metroArrives = true;
	private bool metroDeparts = false;
	private bool scottArrives = false;
	private bool scottGoesUp = false;
	private bool scottStopMove = false;
	private bool scottCanWalk = false;
	private bool hoodedStartTalking = false;
	private bool hoodedFinishedTalking = false;
	private bool hoodedDissapeared = false;
	private bool doLevel = false;
	private bool endLevel = false;

	private bool finish = false;

	public GameObject metroPrefab;
	private GameObject metro;
	private float metroTarget;
	private float startTime;
	private float journeyLength;
	
	void Start ()
	{
		metro = Instantiate (metroPrefab, new Vector3 (150f, -5.5f, -1f), Quaternion.identity) as GameObject;
		metro.GetComponent<Metro> ().enabled = false;
		startTime = Time.time;

		player.transform.position = new Vector3 (146f, -4.33f, -10f);
		player.GetComponent<Player> ().enabled = false;

		metroTarget = -68f;
		journeyLength = Mathf.Abs (60) + Mathf.Abs (metroTarget);

		mainCamera = GameObject.Find ("Main Camera") as GameObject;
		mainCamera.GetComponent<FollowCam> ().SetPosition (new Vector3 (65f, -3.8f, -10f));
		//mainCamera.GetComponent<FollowCam> ().SetTarget (metro);

		dialogue = GameObject.Find ("Dialogue") as GameObject;
		dialogueText = GameObject.Find ("DialogueText").GetComponent<Dialogue> ();
		dialogue.SetActive (false);

		enemies = GameObject.Find ("Enemies") as GameObject;
		enemies.SetActive (false);

		hooded = GameObject.Find ("HoodedCharacter").GetComponent<HoodedCharacter> ();
		hoodedEnd = GameObject.Find ("HoodedCharacterEnd").GetComponent<HoodedCharacter> ();
		hoodedEnd.gameObject.SetActive (false);

		metroSpawners = GameObject.Find ("MetroSpawners") as GameObject;
		metroSpawners.SetActive (false);
		levelStuff = GameObject.Find ("LevelEndStart") as GameObject;
		levelStuff.SetActive (false);

		pause = GameObject.Find ("PauseCanvas") as GameObject;
		pause.SetActive (false);

		pickUps = GameObject.Find ("PickUps") as GameObject;
		pickUps.SetActive (false);

		healthbar = GameObject.Find ("UI") as GameObject;
		healthbar.SetActive (false);

		levelText = GameObject.Find ("LevelCanvas") as GameObject;
		levelText.SetActive (false);

		control = GameObject.Find ("ControlsUI") as GameObject;
		control.SetActive (false);

		AudioSource[] audioSources = GetComponents<AudioSource> ();
		theme = audioSources [0];
		theme.volume = 0f;
		terraTheme = audioSources [1];
	}

	void Update ()
	{
		if (!finish) {
			StartLevel ();
		}

		if (doLevel) {
			if (player.transform.position.x >= hoodedEnd.transform.position.x - 2f) {
				doLevel = false;
				StartCoroutine (FadeOut (theme));
				pickUps.SetActive (false);
				player.SetInStory (true);
				enemies.SetActive (false);
				metroSpawners.SetActive (false);
				dialogue.SetActive (true);
				dialogueText.SelectTextFile ("Level1End");
			}
		}

		if (endLevel) {
			hoodedEnd.SetDissapears ();
			player.SetMoveRight (true);
			endLevel = false;
		}
	}

	private void StartLevel ()
	{
		if (canStart) {
			if (metroArrives) {
				if (!terraTheme.isPlaying) {
					terraTheme.Play ();
				}
				float distCovered = (Time.time - startTime) * 0.01f;
				float fracJourney = distCovered / journeyLength;
				metro.transform.position = Vector3.Lerp (metro.transform.position, new Vector3 (metroTarget, metro.transform.position.y, metro.transform.position.z), fracJourney);
				player.transform.position = Vector3.Lerp (player.transform.position, new Vector3 (metroTarget, player.transform.position.y, player.transform.position.z), fracJourney);
				if (player.transform.position.x <= mainCamera.transform.position.x) {
					mainCamera.GetComponent<FollowCam> ().SetTarget ();
				}
				if (metro.transform.position.x <= -60f) {
					metroArrives = false;
					scottArrives = true;
					metroDeparts = true;
				}
			}
			
			if (metroDeparts) {
				metro.transform.position -= new Vector3 (0.1f, 0.0f, 0.0f);
				if (metro.transform.position.x <= -70f) {
					scottGoesUp = true;
					metroDeparts = false;
					Destroy (metro);
				}
			}
			
			if (scottArrives) {
				healthbar.SetActive (true);
				levelText.SetActive (true);
				player.GetComponent<Player> ().enabled = true;
				player.transform.position += new Vector3 (0f, 0f, 10f);
				player.SetInStory (true);
				scottArrives = false;
			}
			
			if (scottGoesUp) {
				player.SetMoveUp (true);
				scottGoesUp = false;
				scottStopMove = true;
			}
			
			if (scottStopMove) {
				if (player.transform.position.y >= -3.5f) {
					player.SetMoveUp (false);
					scottStopMove = false;
					scottCanWalk = true;
				}
			}
			
			if (scottCanWalk) {
				player.SetCanWalk (true);
				if (player.transform.position.x >= -40f) {
					scottCanWalk = false;
					player.SetCanWalk (false);
					hoodedStartTalking = true;
				}
			}
			
			if (hoodedStartTalking) {
				if (terraTheme.isPlaying) {
					StartCoroutine (FadeOut (terraTheme));
				}
				hoodedStartTalking = false;
				dialogue.SetActive (true);
				dialogueText.SelectTextFile ("FirstScene");
			}
			
			if (hoodedFinishedTalking) {
				StartCoroutine (FadeIn (theme));
				hoodedFinishedTalking = false;
				hoodedDissapeared = true;
				hooded.SetDissapears ();
			}
			
			if (hoodedDissapeared) {
				healthbar.SetActive (true);
				pickUps.SetActive (true);
				pause.SetActive (true);
				player.SetInStory (false);
				enemies.SetActive (true);
				hoodedDissapeared = false;
				metroSpawners.SetActive (true);
				levelStuff.SetActive (true);
				control.SetActive (true);
				doLevel = true;
				hoodedEnd.gameObject.SetActive (true);
				finish = true;
			}
		}
	}

	public void SetCanStart ()
	{
		canStart = true;
	}

	private IEnumerator FadeIn (AudioSource audio)
	{
		theme.Play ();
		if (audio.volume <= 0.8f) {
			while (audio.volume < 0.8f) {
				audio.volume += 0.3f * Time.deltaTime;
				yield return new WaitForSeconds (0.05f);
			}
		}
	}

	private IEnumerator FadeOut (AudioSource audio)
	{
		while (audio.volume > 0f) {
			audio.volume -= 0.3f * Time.deltaTime;
			yield return new WaitForSeconds (0.05f);
		}
		audio.Stop ();
	}

	public void SetHoodedFinishedTalking (bool a)
	{
		hoodedFinishedTalking = a;
	}

	public void SetHoodedDissapeared (bool a)
	{
		hoodedDissapeared = a;
	}

	public void SetLevelEnd (bool a)
	{
		endLevel = a;
	}
}
