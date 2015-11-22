using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour
{

	GameObject mShadow;

	private AudioSource falling;
	private AudioSource hit;

	void Start ()
	{
		mShadow = transform.parent.GetChild (1).gameObject;

		AudioSource[] audioSources = GetComponents<AudioSource> ();
		falling = audioSources [0];
		falling.Play ();
		hit = audioSources [1];
	}

	void Update ()
	{
		if (transform.localPosition.y <= 0.0f) {
			falling.Stop ();
			hit.Play ();
			Destroy (transform.parent.gameObject);
			return;
		}
		float shadowScale = Mathf.Min (1.5f / transform.localPosition.y, 1.0f);
		mShadow.transform.localScale = new Vector3 (shadowScale, shadowScale, mShadow.transform.localScale.z);
	}

	void OnTriggerEnter (Collider col)
	{
		hit.Play ();
		if (col.gameObject.name == "GeneralColliderPlayer") {
			// determine the Players Script.
			Player player = col.gameObject.transform.parent.GetComponent<Player> ();
			if (Mathf.Abs (player.GetFootY () - (transform.parent.position.y - 0.3f)) <= 0.2f) {
				falling.Stop ();
				player.GetHit (Vector2.left, 10);
				Destroy (transform.parent.gameObject);
			}
		} else if (col.gameObject.name == "Pooka") {
			falling.Stop ();
			col.gameObject.GetComponent<Pooka> ().SetLife (0);
			Destroy (transform.parent.gameObject);
		} else if (col.gameObject.name == "Fygar") {
			falling.Stop ();
			col.gameObject.GetComponent<Fygar> ().SetLife (0);
			Destroy (transform.parent.gameObject);
		}
	}
}
