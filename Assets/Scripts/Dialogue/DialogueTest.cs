using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class DialogueTest : MonoBehaviour
{

	private Text thisText;
	
	protected FileInfo theSourceFile = null;
	protected StreamReader reader = null;
	protected string text = " ";
	
	void Start ()
	{
		thisText = gameObject.GetComponent<Text> ();
		theSourceFile = new FileInfo ("Assets/Scripts/Dialogue/dialogueTest.txt");
		reader = theSourceFile.OpenText ();
	}
	
	void Update ()
	{
		if (text != null) {
			if (Input.anyKeyDown) {
				StartCoroutine (DisplayText ());
			}
		}
	}

	private IEnumerator DisplayText ()
	{
		int charLineNumber = 0;
		text = reader.ReadLine ();
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
