using UnityEngine;
using System.Collections;

public class HoboFightCollider : MonoBehaviour
{
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "GeneralColliderPlayer") {
			if (col.gameObject.transform.parent.GetComponent<Player> ().IsDefending ()) {
                gameObject.transform.parent.GetComponent<Hobo>().Recoil(-gameObject.transform.parent.GetComponent<Hobo>().GetFacingDirection(), 0.2f);
                col.gameObject.transform.parent.GetComponent<Player>().GetBlockDamage((int)(gameObject.transform.parent.GetComponent<Hobo>().mStats.DoDynamicDamage() / 3.0f));
			} else {
				col.gameObject.transform.parent.GetComponent<Player> ().GetHit (gameObject.transform.parent.GetComponent<Hobo> ().GetFacingDirection (), gameObject.transform.parent.GetComponent<Hobo> ().mStats.DoDynamicDamage ());
			}
		}
	}
}
