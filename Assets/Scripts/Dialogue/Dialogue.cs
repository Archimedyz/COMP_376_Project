using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class Dialogue: MonoBehaviour
{

	private Text thisText;
	
	protected FileInfo theSourceFile = null;
	protected StreamReader reader = null;
	protected string text = " ";
	
	void Start ()
	{
		thisText = gameObject.GetComponent<Text> ();
		theSourceFile = null;
	}
	
	void Update ()
	{
		if (Input.anyKeyDown && theSourceFile != null) {
			text = reader.ReadLine ();
			if (text != null) {
				StartCoroutine (DisplayText ());
			} else {
				theSourceFile = null;
				GameObject.Find ("Dialogue").SetActive (false);
			}
		}
	}

	public void SelectTextFile (string file)
	{
		if (file == "FirstScene") {
			theSourceFile = new FileInfo ("Assets/Scripts/Dialogue/FirstScene.txt");
			reader = theSourceFile.OpenText ();
		} else {
			theSourceFile = null;
		}
	}

	private IEnumerator DisplayText ()
	{
		int charLineNumber = 0;
		thisText.text = "";
		for (int i = 0; i < text.Length; i++, charLineNumber++) {
			if (text [i] == '$') {
				thisText.text += "\n";
			} else {
				thisText.text += text [i];
			}
			yield return new WaitForSeconds (0.05f);
		}
	}
}
