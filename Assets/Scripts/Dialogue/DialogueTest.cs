using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueTest : MonoBehaviour
{

	private Text thisText;
	
	void Start ()
	{
		thisText = gameObject.GetComponent<Text> ();
	}
	
	void Update ()
	{
		thisText.text = "Allo";
	}
}
