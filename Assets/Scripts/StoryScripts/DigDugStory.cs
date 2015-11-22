using UnityEngine;
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

	private float digdugTargetPosition;

	private int hitTime = 0;

	private float timer = 0.0f, startTimer = 0.0f;

	void Start ()
	{
		player = GameObject.Find ("Player").GetComponent<Player> ();
		digdug = GameObject.Find ("DigDug").GetComponent<DigDug> ();
		mainCamera = GameObject.Find ("Main Camera") as GameObject;
		enemies = GameObject.Find ("Enemies") as GameObject;
	}

	void Update ()
	{
		timer += Time.deltaTime;

		if (player.transform.position.x >= 30 && !meetDigDug) {
			meetDigDug = true;
			player.SetCanMove (false);
			player.SetInStory (true);
			digdug.SetCanMove (false);
			//camera.GetComponent<FollowCam> ().enabled = false;
		}

		if (meetDigDug && digdug.transform.position.y >= player.transform.position.y) {
			for (int i = 0; i < 5; i++) {
				digdug.transform.position -= new Vector3 (0f, 0.1f, 0f);
				if (digdug.transform.position.y <= player.transform.position.y) {
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
			digdug.transform.position -= new Vector3 (0.2f, 0, 0f);
			if (digdug.transform.position.x <= 2.0) {
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
			player.SetInStory (false);
			digdug.SetInStory (false);
			enemies.GetComponent<Boss1Controller> ().enabled = true;
		}

		if (moveDigDug && hitTime <= 3) {
			digdug.transform.position += new Vector3 (0.2f, 0, 0f);
			mainCamera.transform.position += new Vector3 (0.05f, 0f, 0f);
			player.SetMoveRight (true);
			if (digdug.transform.position.x >= digdugTargetPosition) {
				Debug.Log ("Allo");
				enemies.GetComponent<Boss1Controller> ().CreateWave (1);
				moveDigDug = false;
				player.SetMoveRight (false);
				player.SetInStory (false);
				digdug.SetInStory (false);
			}
		}
	}

	public void MoveDigDug (int numberOfHit)
	{
		enemies.GetComponent<Boss1Controller> ().enabled = false;
		moveDigDug = true;
		hitTime = numberOfHit;
		player.SetInStory (true);
		digdug.SetInStory (true);

		if (hitTime == 1)
			digdugTargetPosition = digdug.transform.position.x + 15;
		else if (hitTime == 2) {
			digdugTargetPosition = digdug.transform.position.x + 10;
		} else if (hitTime == 3) {
			digdugTargetPosition = digdug.transform.position.x + 10;
		}
	}
}
