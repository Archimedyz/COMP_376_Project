using UnityEngine;
using System.Collections;

public class Rock : MonoBehaviour
{

    GameObject mShadow;

    void Start()
    {
        mShadow = transform.parent.GetChild(1).gameObject;
    }

    void Update()
    {
        if(transform.localPosition.y <= 0.0f) {
            Destroy(transform.parent.gameObject);
            return;
        }
        float shadowScale = Mathf.Min(1.5f/transform.localPosition.y, 1.0f);
        mShadow.transform.localScale = new Vector3(shadowScale, shadowScale, mShadow.transform.localScale.z);
    }

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "GeneralColliderPlayer") {
            // determine the Players Script.
            Player player = col.gameObject.transform.parent.GetComponent<Player>();
            Debug.Log("Rock Ground Y: " + transform.parent.position.y + "Player Ground Y: " + player.GetFootY());
            if(Mathf.Abs(player.GetFootY() - transform.parent.position.y) <= 0.2f) {
                player.GetHit(Vector2.left, 10);
                Destroy(transform.parent.gameObject);
            }
		} else if (col.gameObject.name == "Pooka") {
            col.gameObject.GetComponent<Pooka>().SetLife(0);
            Destroy(transform.parent.gameObject);
		} else if (col.gameObject.name == "Fygar") {
			col.gameObject.GetComponent<Fygar> ().SetLife (0);
            Destroy(transform.parent.gameObject);
		}
	}
}
