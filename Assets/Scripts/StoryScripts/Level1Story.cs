using UnityEngine;
using System.Collections;

public class Level1Story : MonoBehaviour
{
	private AudioSource theme;
	private AudioSource terraTheme;

	public Player player;
	private GameObject mainCamera;
	private GameObject dialogue;
	private Dialogue dialogueText;

	private bool metroArrives = true;
	private bool metroDeparts = false;
	private bool scottArrives = false;
	private bool scottGoesUp = false;
	private bool scottStopMove = false;
	private bool scottCanWalk = false;
	private bool hoodedStartTalking = false;
	private bool hoodedFinishedTalking = false;

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
		theme.volume = 0f;
		terraTheme = audioSources [1];
	}

	void Update ()
	{
		if (metroArrives) {
			if (!terraTheme.isPlaying) {
				terraTheme.Play ();
			}
			float distCovered = (Time.time - startTime) * 0.05f;
			float fracJourney = distCovered / journeyLength;
			metro.transform.position = Vector3.Lerp (metro.transform.position, new Vector3 (metroTarget, metro.transform.position.y, metro.transform.position.z), fracJourney);
			if (metro.transform.position.x <= -57f) {
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
			player.gameObject.SetActive (true);
			mainCamera.GetComponent<FollowCam> ().SetTarget ();
			scottArrives = false;
		}

		if (scottGoesUp) {
			player.SetInStory (true);
			player.SetMoveUp (true);
			scottGoesUp = false;
			scottStopMove = true;
		}

		if (scottStopMove) {
			if (player.transform.position.y >= -3.5f) {
				player.SetMoveUp (false);
				scottStopMove = false;
				//TODO Character can move ?
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
			player.SetInStory (false);
			hoodedFinishedTalking = false;
		}
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
}
