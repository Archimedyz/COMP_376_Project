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

	private Vector2 mDirection;

	private float maxExistTime = 5.0f;

	public int damage = 20;

	private Transform player;

	void Start ()
	{
		player = GameObject.Find ("Player").transform;

		canThrow = true;
		throwed = false;
		rb = GetComponent<Rigidbody> ();
		Timer = 0.0f;
		throwTimer = 0.0f;
	}

	void Update ()
	{
		Timer += Time.deltaTime;

		if (Timer >= 0.24f && Timer <= 0.3f) {
			transform.position = new Vector3 (transform.parent.position.x - 0.1f, transform.parent.position.y + 0.5f, transform.position.z);
		} else if (Timer >= 0.12f && Timer < 0.24f) {
			transform.position = new Vector3 (transform.parent.position.x - 0.5f, transform.parent.position.y - 0.1f, transform.position.z);
		} else if (Timer >= 0.3f) {
			if (canThrow)
				Throw ();
			transform.Translate (Vector3.Normalize (player.position - transform.position) * mThrowForce * Time.deltaTime);
		}

		if (throwed) {
			if (transform.position.x - initialPositionX >= maxDisplacement || Timer >= maxExistTime) {
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
	}

	public void SetDirection (Vector2 direction)
	{
		mDirection = direction;
	}

	public Vector2 GetDirection ()
	{
		return mDirection;
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.name == "Player") {
			if (col.gameObject.GetComponent<Player> ().IsDefending ()) {
				col.gameObject.GetComponent<Player> ().GetBlockDamage (damage / 3);
			} else
				col.gameObject.GetComponent<Player> ().GetKnockdown (mDirection, damage);

			Destroy (gameObject);
		}
	}
}
