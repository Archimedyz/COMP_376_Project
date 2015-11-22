using UnityEngine;
using System.Collections;

public class PlayerFightCollider : MonoBehaviour
{
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "GeneralColliderHobo") {
            col.gameObject.transform.parent.GetComponent<Hobo>().GetHit(gameObject.transform.parent.GetComponent<Player>().GetFacingDirection(), gameObject.transform.parent.GetComponent<Player>().mStats.DoDynamicDamage(), gameObject.transform.parent.GetComponent<Player>().mStats.wasCrit);
            if (col.gameObject.transform.parent.GetComponent<Hobo>().mStats.isDead())
                gameObject.transform.parent.GetComponent<Player>().mStats.Exp += col.gameObject.transform.parent.GetComponent<Hobo>().expGiven;
		} else if (col.gameObject.name == "GeneralColliderNeanderthal") {
            col.gameObject.transform.parent.GetComponent<Neanderthal>().GetHit(gameObject.transform.parent.GetComponent<Player>().GetFacingDirection(), gameObject.transform.parent.GetComponent<Player>().mStats.DoDynamicDamage(), gameObject.transform.parent.GetComponent<Player>().mStats.wasCrit);
            if (col.gameObject.transform.parent.GetComponent<Neanderthal>().mStats.isDead())
                gameObject.transform.parent.GetComponent<Player>().mStats.Exp += col.gameObject.transform.parent.GetComponent<Neanderthal>().expGiven;
        }
	}
}
