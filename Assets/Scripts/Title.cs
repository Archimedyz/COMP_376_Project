using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour
{
	private bool canLaunch = false;
	public float lauchSpeed;
	public Vector3 target;
	
	private float startTime;
	private float journeyLength;


	void Start ()
	{
		target = new Vector3 (transform.position.x - 1.0f, transform.position.y, transform.position.z);        
		startTime = Time.time;
		journeyLength = Vector3.Distance (transform.position, target);
	}
	
	void Update ()
	{
		if (transform.position.x > target.x) {
			float distCovered = (Time.time - startTime) * 0.5f;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp (transform.position, target, fracJourney);
		}

		if (canLaunch) {
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
			col.gameObject.transform.parent.GetComponent<PlayerTesting> ().GetHit (Vector2.left, 10);
		}
	}

	private void Launch ()
	{
		transform.Translate (Vector2.left * lauchSpeed);
	}

	public void SetLaunch ()
	{
		canLaunch = true;
	}
}
