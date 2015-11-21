using UnityEngine;
using System.Collections;

public class DigDugStory : MonoBehaviour
{

	private Player player;
	private DigDug digdug;
	private GameObject camera;

	private bool meetDigDug = false;
	private bool meetPlayer = false;

	void Start ()
	{
		player = GameObject.Find ("Player").GetComponent<Player> ();
		digdug = GameObject.Find ("DigDug").GetComponent<DigDug> ();
		camera = GameObject.Find ("Main Camera") as GameObject;
	}

	void Update ()
	{
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
		Debug.Log (meetPlayer);
		if (meetPlayer) {
			digdug.Pumping ();
			Debug.Log ("Allo2");
			meetPlayer = false;
		}
	}
}
