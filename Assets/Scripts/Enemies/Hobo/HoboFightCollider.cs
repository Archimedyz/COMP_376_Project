using UnityEngine;
using System.Collections;

public class HoboFightCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "GeneralColliderPlayer")
        {
            if (col.gameObject.transform.parent.GetComponent<Player>().IsDefending())
            {
                gameObject.transform.parent.GetComponent<Hobo>().Recoil(col.gameObject.transform.parent.GetComponent<Player>().GetFacingDirection(), 3.0f);
            }
            else
            {
                col.gameObject.transform.parent.GetComponent<Player>().GetHit(gameObject.transform.parent.GetComponent<Hobo>().GetFacingDirection(), gameObject.transform.parent.GetComponent<Hobo>().mStats.DoDynamicDamage());
            }
        }
    }
}
