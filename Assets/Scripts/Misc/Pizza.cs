using UnityEngine;
using System.Collections;

public class Pizza : MonoBehaviour
{
	public int health;
	void Start ()
	{

	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "Player") {
			col.GetComponent<Player> ().GetHealth (health);
			Destroy (gameObject);
		}
	}
}
