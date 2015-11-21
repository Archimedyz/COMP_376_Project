using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "GeneralColliderPlayer") {
			col.gameObject.transform.parent.GetComponent<Player> ().GetHit (Vector2.left, 10);
			//col.gameObject.transform.parent.GetComponent<PlayerTesting> ().GetHit (Vector2.left, 10);
		} else if (col.gameObject.name == "Pooka") {
			col.gameObject.GetComponent<Pooka> ().SetLife (0);
		} else if (col.gameObject.name == "Fygar") {
			col.gameObject.GetComponent<Fygar> ().SetLife (0);
		}
		Destroy (gameObject);
	}
}
