using UnityEngine;
using System.Collections;

public class Coconut : MonoBehaviour
{
	private bool canThrow;
	private bool throwed;
	private float initialPositionX;

	public float mThrowForce;
	public float maxDisplacement;

	private Rigidbody rb;

	private float Timer;
	private float throwTimer;

	void Start ()
	{
		canThrow = true;
		throwed = false;
		rb = GetComponent<Rigidbody> ();
		Timer = 0.0f;
		throwTimer = 0.0f;
	}

	void FixedUpdate ()
	{
	}

	void Update ()
	{
		Timer += Time.deltaTime;

		if (Timer >= 0.24f && Timer <= 0.3f) {
			transform.position = new Vector3 (-0.1f, 0.5f, transform.position.z);
		} else if (Timer >= 0.12f && Timer < 0.24f) {
			transform.position = new Vector3 (-0.5f, -0.1f, transform.position.z);
		} else if (Timer >= 0.3f && canThrow) {
			Throw ();
		}

		if (throwed) {
			if (transform.position.x - initialPositionX >= maxDisplacement) {
				Destroy (gameObject);
			}
		}
	}

	public void Throw ()
	{
		canThrow = false;
		throwed = true;
		initialPositionX = transform.position.x;
		gameObject.transform.parent = null;
		//rb.useGravity = true;
		rb.AddForce (transform.right * mThrowForce, ForceMode.Force);
	}
}
