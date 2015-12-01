using UnityEngine;
using System.Collections;

public class LevelEnd : MonoBehaviour
{
	void Update ()
	{
		Debug.Log (transform.position);
	}

	void OnTriggerEnter (Collider col)
	{
		Debug.Log ("Allo");
		if (col.transform.parent != null && col.transform.parent.gameObject.tag == "Player") {
			GameObject.FindGameObjectWithTag ("GameController").SendMessage ("NextLevel");
		}
	}
}
