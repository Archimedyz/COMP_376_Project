
//**
//** Authored by Robert Anthony Di Monaco
//**


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AreaBlastController : MonoBehaviour
{
	public Text UI_Blasttext;  // Set UI to display how many blasts are left

	private ParticleSystem blastEffect;

	public int blastsLeft;    // set max amount of blasts
	public float blastSize;   // how far are they pushed 

	void Start ()
	{
		blastEffect = GetComponent<ParticleSystem> ();
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.M) && blastsLeft > 0) {   // play effect and reduce amount of blasts left
			blastEffect.Play (false);
			blastsLeft--;
		}

		if(UI_Blasttext != null)
			UI_Blasttext.text = "Key M - Area Blast: " + blastsLeft; 
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Enemy" && Input.GetKeyDown (KeyCode.M) && blastsLeft > 0) {			
			// translate gameObject
			if (transform.position.x <= other.transform.position.x)   					// enemy to the right of player, blast right
				other.transform.Translate (blastSize * Vector3.right, Space.Self);
			else if (transform.position.x > other.transform.position.x)  				// enemy to the left of player, blast left
				other.transform.Translate (blastSize * Vector3.left, Space.Self);
		}
	} 

	// NOTE:: ON TRIGGER STAY IS A LITTLE FLIMSY ITS NOT CALLED EVERY FRAME
	void OnTriggerStay (Collider other)
	{
		if (other.gameObject.tag == "Enemy" && Input.GetKeyDown (KeyCode.M) && blastsLeft > 0) {			
			// translate gameObject
			if (transform.position.x <= other.transform.position.x)   					// enemy to the right of player, blast right
				other.transform.Translate (blastSize * Vector3.right, Space.Self);
			else if (transform.position.x > other.transform.position.x)  				// enemy to the left of player, blast left
				other.transform.Translate (blastSize * Vector3.left, Space.Self);
		}
	}
}





