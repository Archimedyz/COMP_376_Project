
//**
//** Authored by Robert Anthony Di Monaco
//**

using UnityEngine;
using System.Collections;

public class BlastPickUpController : MonoBehaviour 
{	
	public AreaBlastController scriptRef;

	void Start () 
	{
		scriptRef = GameObject.FindWithTag ("AreaBlast").GetComponent<AreaBlastController>();
	}

	void Update () 
	{
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "BlastPickUp") 
		{
			other.gameObject.SetActive(false);
			scriptRef.blastsLeft++; 
		}
	}
}
