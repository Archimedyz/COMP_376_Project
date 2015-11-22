using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour
{
	private bool canLaunch = false;
	private float launchSpeed = 0.2f;
	private Vector3 target;
	
	private float startTime;
	private float journeyLength;

	private AudioSource launch;
	private bool hasLaunched = false;

	void Start ()
	{
		target = new Vector3 (transform.position.x - 1.0f, transform.position.y, transform.position.z);        
		startTime = Time.time;
		journeyLength = Vector3.Distance (transform.position, target);

		AudioSource[] audioSources = GetComponents<AudioSource> ();
		launch = audioSources [0];
	}
	
	void Update ()
	{
		if (transform.position.x > target.x) {
			float distCovered = (Time.time - startTime) * 0.5f;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp (transform.position, target, fracJourney);
		}

		if (canLaunch) {
			if (!hasLaunched) {
				hasLaunched = true;
				launch.Play ();
			}
			Launch ();
		}

		if (canLaunch && transform.position.x < -10) {
			Destroy (gameObject);
			GameObject.Find ("DigDug").GetComponent<DigDug> ().SetThrowTitle ();
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "GeneralColliderPlayer") {
			col.gameObject.transform.parent.GetComponent<Player> ().GetHit (Vector2.left, 10);
		}
	}

	private void Launch ()
	{
		transform.Translate (Vector2.left * launchSpeed);
	}

	public void SetLaunch ()
	{
		canLaunch = true;
	}

	public void SetLaunchSpeed (float speed)
	{
		launchSpeed = speed;
	}
}
