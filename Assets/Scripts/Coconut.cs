using UnityEngine;
using System.Collections;

public class Coconut : MonoBehaviour
{

	private bool canThrow;

	public float mThrowForce;

	private Rigidbody rb;

	private float Timer;

	void Start ()
	{
		canThrow = true;
		rb = GetComponent<Rigidbody> ();
		Timer = 0.0f;
	}

	void FixedUpdate ()
	{
	}

	void Update ()
	{
		Timer += Time.deltaTime;

		if (Timer >= 0.24f && Timer <= 0.3f) {
			transform.position = new Vector3 (0.0f, 0.3f, transform.position.z);
		} else if (Timer >= 0.12f && Timer < 0.24f) {
			transform.position = new Vector3 (-0.3f, 0.0f, transform.position.z);
		} else if (Timer >= 0.3f && canThrow) {
			Throw ();
		}
		Debug.Log (transform.position);
	}

	public void Throw ()
	{
		canThrow = false;
		Debug.Log ("Throw");
		gameObject.transform.parent = null;
		rb.useGravity = true;
		rb.AddForce (transform.right * mThrowForce, ForceMode.Impulse);
	}
}
