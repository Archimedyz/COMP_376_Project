using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
	private Text thisText;
	
	private FileInfo theSourceFile = null;
	private StreamReader reader = null;
	private string text = " ";
	
	private bool finished = true;

	private GameObject levelController;
	private GameObject creditCanvas;

	void Start ()
	{
		thisText = gameObject.GetComponent<Text> ();
		thisText.color = new Color (thisText.color.r, thisText.color.g, thisText.color.b, 0.0f);
		thisText.text = " ";

		theSourceFile = new FileInfo ("Assets/Scripts/Dialogue/IntroDialogue.txt");
		reader = theSourceFile.OpenText ();

		creditCanvas = GameObject.Find ("CreditCanvas") as GameObject;
		creditCanvas.SetActive (false);

		levelController = GameObject.Find ("LevelController") as GameObject;
		//levelController.SetActive (false);
	}

	void Update ()
	{
		if (theSourceFile != null && finished) {
			text = MakeText ();
			if (text != null) {
				StartCoroutine (DisplayText ());
			} else {
				theSourceFile = null;
				StartCoroutine (RemoveCanvas ());
			}
		}
	}

	private string MakeText ()
	{
		string temp = reader.ReadLine ();
		string returnString = "";

		if (temp != null) {
			for (int i = 0; i < temp.Length; i++) {
				if (temp [i] == '$') {
					returnString += '\n';
				} else if (temp [i] == '%') {
					returnString += ' ';
				} else {
					returnString += temp [i];
				}
			}
		} else {
			return null;
		}
		return returnString;
	}

	private IEnumerator DisplayText ()
	{
		thisText.text = "";
		finished = false;
			
		thisText.text = text;

		while (thisText.color.a < 1) {
			thisText.color = new Color (thisText.color.r, thisText.color.g, thisText.color.b, thisText.color.a + (0.2f * Time.deltaTime));
			yield return new WaitForSeconds (0.005f);
		}

		yield return new WaitForSeconds (2f);

		while (thisText.color.a > 0) {
			thisText.color = new Color (thisText.color.r, thisText.color.g, thisText.color.b, thisText.color.a - (0.2f * Time.deltaTime));
			yield return new WaitForSeconds (0.005f);
		}

		thisText.text = "";

		finished = true;
	}

	private IEnumerator RemoveCanvas ()
	{
		Image panel = GameObject.Find ("IntroPanel").GetComponent<Image> ();

		while (panel.color.a > 0) {
			panel.color = new Color (panel.color.r, panel.color.g, panel.color.b, panel.color.a - (0.5f * Time.deltaTime));
			yield return new WaitForSeconds (0.005f);
		}
		
		creditCanvas.SetActive (true);
		GameObject.Find ("IntroCanvas").SetActive (false);
		//levelController.SetActive (true);
		levelController.GetComponent<Level1Story> ().SetCanStart ();
	}
}
