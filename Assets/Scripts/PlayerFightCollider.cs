using UnityEngine;
using System.Collections;

public class PlayerFightCollider : MonoBehaviour
{
	void OnTriggerEnter (Collider col)
	{
		Debug.Log (col.gameObject.name);
		if (col.gameObject.name == "GeneralCollider") {
			col.gameObject.transform.parent.GetComponent<Hobo> ().GetHit (gameObject.transform.parent.GetComponent<Player> ().GetFacingDirection ());
		}
	}
}
