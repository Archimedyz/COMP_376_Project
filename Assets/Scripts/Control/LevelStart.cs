using UnityEngine;
using System.Collections;

public class LevelStart : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			// GameObject.FindGameObjectWithTag("Player").transform.position = transform.position;
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			GameObject.FindGameObjectWithTag ("Player").transform.position = transform.position;
			Destroy (gameObject);
		}
	}
}
