using UnityEngine;
using System.Collections;

public class HoboFightCollider : MonoBehaviour
{
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "GeneralColliderPlayer") {
            col.gameObject.transform.parent.GetComponent<Player>().GetHit(gameObject.transform.parent.GetComponent<Hobo>().GetFacingDirection(), gameObject.transform.parent.GetComponent<Hobo>().mStats.DoDynamicDamage());
		}
	}
}
