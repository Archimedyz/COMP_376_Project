using UnityEngine;
using System.Collections;

public class HoboFightCollider : MonoBehaviour
{
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "GeneralColliderPlayer") {
			Debug.Log (gameObject.transform.parent.GetComponent<Hobo> ().GetFacingDirection ());
			col.gameObject.transform.parent.GetComponent<Player> ().GetHit (gameObject.transform.parent.GetComponent<Hobo> ().GetFacingDirection ());
		}
	}
}
