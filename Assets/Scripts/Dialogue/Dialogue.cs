using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class Dialogue: MonoBehaviour
{
	private Text thisText;
	
	private FileInfo theSourceFile = null;
	private StreamReader reader = null;
	private string text = " ";

	private string fileName = "";

	private bool startText = false;
	private bool finished = true;
	
	void Start ()
	{
		thisText = gameObject.GetComponent<Text> ();
		thisText.text = " ";
		theSourceFile = null;
	}
	
	void Update ()
	{
		if ((Input.anyKeyDown /*&& theSourceFile != null */ && finished) || startText) {
			startText = false;
			text = reader.ReadLine ();
			if (text != null) {
				StartCoroutine (DisplayText ());
			} else {
				theSourceFile = null;
				GameObject.Find ("Dialogue").SetActive (false);
				if (fileName == "FirstScene") {
					GameObject.Find ("LevelController").GetComponent<Level1Story> ().SetHoodedFinishedTalking (true);
				} else if (fileName == "Level1End") {
					GameObject.Find ("LevelController").GetComponent<Level1Story> ().SetLevelEnd (true);
				}
			}
		}
	}

	public void SelectTextFile (string file)
	{
		if (file == "FirstScene") {
			fileName = file;
			theSourceFile = new FileInfo ("Assets/Scripts/Dialogue/FirstScene.txt");
			reader = theSourceFile.OpenText ();
			startText = true;
		} else if (file == "Lavel1End") {
			fileName = file;
			theSourceFile = new FileInfo ("Assets/Scripts/Dialogue/Level1End.txt");
			reader = theSourceFile.OpenText ();
			startText = true;
		} else {
			theSourceFile = null;
		}
	}

	private IEnumerator DisplayText ()
	{
		int charLineNumber = 0;
		thisText.text = "";
		finished = false;
		for (int i = 0; i < text.Length; i++, charLineNumber++) {

			thisText.text += text [i];
			yield return new WaitForSeconds (0.05f);
		}
		finished = true;
	}
}
