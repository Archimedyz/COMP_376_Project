using UnityEngine;
using System.Collections;

public class Pizza : MonoBehaviour
{

	void Start ()
	{
	
	}

	void Update ()
	{

	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "Player") {
			col.GetComponent<Player> ().GetHealth (20);
			Destroy (gameObject);
		}
	}
}
