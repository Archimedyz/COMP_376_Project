
// Authored by Robert Anthony Di Monaco


using UnityEngine;
using System.Collections;

public class ShurikenController : MonoBehaviour 
{
	private Rigidbody rb;
	private bool placed;   // is Shuriken mobile or placed 

	void Start () 
	{	
		rb = GetComponent<Rigidbody>();

		placed = false;  // default its mobile, the thrown version
	}
	
	void Update () 
	{
		
		
	}


//	void OnTriggerEnter(Collider other)
//	{Debug.Log("TRIGGER SO WHAT THE FUCK");
//		if (placed == true)   // this shuriken does not move
//		{  
//
//		} 
//		else    // mobile shuriken so teleports with enemy
//		{
//			if (other.gameObject.tag == "Enemy") 
//			{
//
//
////				rb.velocity = Vector3.zero;
////				rb.angularVelocity = Vector3.zero;
////				transform.parent = other.transform;
//			}
//		}
//	}
//	void OnTriggerExit(Collider other)
//	{
//		if (other.gameObject.tag == "Enemy") 
//		{
//			transform.parent = null;
//		}
//	}

	// set placed, this is used in ShurikenSpawner
	public void SetPlaced(bool newValue)
	{
		placed = newValue;
	}
}



















