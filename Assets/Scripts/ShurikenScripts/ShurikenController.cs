
//** 
//** Authored by Robert Anthony Di Monaco
//**

using UnityEngine;
using System.Collections;

public class ShurikenController : MonoBehaviour 
{
	private Rigidbody rb;
	private bool placed;   // is Shuriken mobile or placed 
	private Transform enemyOnPlaced;   // when an enemy walks on a shuriken you can teleport them

	void Start () 
	{	
		rb = GetComponent<Rigidbody>();
		enemyOnPlaced = null;
	}
	
	void Update () 
	{

	}

	void OnTriggerEnter(Collider other)	
	{  
		if (other.gameObject.tag == "Wall") 
		{
			placed = true;  // if shuriken hits wall it becomes a placed shuriken
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}

		if (other.gameObject.tag == "Enemy") 
			if (placed == true)   // shuriken does not move 
			{  
				enemyOnPlaced = other.transform;   // can teleport an enemy on shuriken any number of times; pay off since need to lure enemy onto shuriken
			} 
			else if (placed == false)   // mobile shuriken so teleports with enemy, then destroyed so one time use; pay off since the shuriken is attached to enemy
			{				
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
				transform.parent = other.transform;
			}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Enemy") 
		{
			enemyOnPlaced = null;
		}
	}

	// set placed to true for shuriken's that dont move, this is used in ShurikenSpawner
	public void SetPlaced(bool newValue)
	{
		placed = newValue;
	}

	// get if this shuriken is placed or mobile
	public bool GetPlaced()
	{
		return placed;
	}

	// used in shurikenSpawner to teleport enemy on a placed shuriken
	public Transform GetEnemyOnPlaced()
	{
		return enemyOnPlaced;
	}
}



















