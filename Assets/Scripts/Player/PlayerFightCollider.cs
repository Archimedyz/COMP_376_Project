using UnityEngine;
using System.Collections;

public class PlayerFightCollider : MonoBehaviour
{
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "GeneralColliderHobo") {
			col.gameObject.transform.parent.GetComponent<Hobo> ().GetHit (gameObject.transform.parent.GetComponent<Player> ().GetFacingDirection (), gameObject.transform.parent.GetComponent<Player> ().mStats.DoDynamicDamage (), gameObject.transform.parent.GetComponent<Player> ().mStats.wasCrit);
			if (col.gameObject.transform.parent.GetComponent<Hobo> ().mStats.isDead () && col.gameObject.transform.parent.GetComponent<Hobo> ().expGiven != 0) {
				gameObject.transform.parent.GetComponent<Player> ().AddExperience (col.gameObject.transform.parent.GetComponent<Hobo> ().expGiven);
				col.gameObject.transform.parent.GetComponent<Hobo> ().expGiven = 0;
			}
		} else if (col.gameObject.name == "GeneralColliderNeanderthal") {
			col.gameObject.transform.parent.GetComponent<Neanderthal> ().GetHit (gameObject.transform.parent.GetComponent<Player> ().GetFacingDirection (), gameObject.transform.parent.GetComponent<Player> ().mStats.DoDynamicDamage (), gameObject.transform.parent.GetComponent<Player> ().mStats.wasCrit);
			if (col.gameObject.transform.parent.GetComponent<Neanderthal> ().mStats.isDead () && col.gameObject.transform.parent.GetComponent<Neanderthal> ().expGiven != 0) {
				gameObject.transform.parent.GetComponent<Player> ().AddExperience (col.gameObject.transform.parent.GetComponent<Neanderthal> ().expGiven);
				col.gameObject.transform.parent.GetComponent<Neanderthal> ().expGiven = 0;
			}
		} else if (col.gameObject.tag == "Spawner") {
			col.gameObject.GetComponent<Spawner> ().GetHit (gameObject.transform.parent.GetComponent<Player> ().GetFacingDirection (), gameObject.transform.parent.GetComponent<Player> ().mStats.DoDynamicDamage (), gameObject.transform.parent.GetComponent<Player> ().mStats.wasCrit);
			if (col.gameObject.GetComponent<Spawner> ().mStats.isDead () && col.gameObject.GetComponent<Spawner> ().expGiven != 0) {
				gameObject.transform.parent.GetComponent<Player> ().AddExperience (col.gameObject.GetComponent<Spawner> ().expGiven);
				col.gameObject.GetComponent<Spawner> ().expGiven = 0;
			}
		}
	}
}
