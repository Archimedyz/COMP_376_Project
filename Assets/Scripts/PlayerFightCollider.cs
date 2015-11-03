﻿using UnityEngine;
using System.Collections;

public class PlayerFightCollider : MonoBehaviour
{
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "GeneralColliderHobo") {
			col.gameObject.transform.parent.GetComponent<Hobo> ().GetHit (gameObject.transform.parent.GetComponent<Player> ().GetFacingDirection (), 20);
		} else if (col.gameObject.name == "GeneralColliderNeanderthal") {
			col.gameObject.transform.parent.GetComponent<Neanderthal> ().GetHit (gameObject.transform.parent.GetComponent<Player> ().GetFacingDirection (), 20);
		}
	}
}
