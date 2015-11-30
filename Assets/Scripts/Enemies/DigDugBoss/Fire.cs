using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
	private float lifeTimer = 5.0f;
	private float timer = 0.0f;

	public int damage;

	Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
	}

	void Update ()
	{
		rb.AddForce (Vector2.left * 7, ForceMode.Force);

		timer += Time.deltaTime;

		if (timer >= lifeTimer) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "Player") {
			if (col.gameObject.GetComponent<Player> ().IsDefending ()) {
				col.gameObject.GetComponent<Player> ().GetBlockDamage (damage / 3);
			} else
				col.gameObject.GetComponent<Player> ().GetKnockdown (Vector2.left, damage);

			Destroy (gameObject);
		}
	}
}
