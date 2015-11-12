using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{

	private float lifeTimer = 5.0f;
	private float timer = 0.0f;

	Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
	}

	void Update ()
	{
		rb.AddForce (Vector2.left * 10, ForceMode.Force);

		timer += Time.deltaTime;

		if (timer >= lifeTimer) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "GeneralColliderPlayer") {
			col.gameObject.transform.parent.GetComponent<PlayerTesting> ().GetKnockdown (Vector2.left, 20);
		}
	}
}
