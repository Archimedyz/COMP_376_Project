using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Final_Scene : MonoBehaviour
{
	private bool start = false;

	private Player player;
	private GameObject hooded;
	private GameObject fevens;
	private Text levelText;
	
	private GameObject dialogue;
	private Dialogue dialogueText;

	void Start ()
	{
		player = GameObject.Find ("Player").GetComponent<Player> ();
		SetLevelLabel ();
		hooded = GameObject.Find ("HoodedCharacter") as GameObject;
		fevens = GameObject.Find ("EvilFevens") as GameObject;

		dialogue = GameObject.Find ("Dialogue") as GameObject;
		dialogueText = GameObject.Find ("DialogueText").GetComponent<Dialogue> ();
		dialogue.SetActive (false);

		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
		levelText.gameObject.SetActive (false);
	}

	void Update ()
	{
		if (start) {
			start = false;
			dialogue.SetActive (true);
			levelText.gameObject.SetActive (true);
			if (player.mStats.Level >= 6) {			
				levelText.text = "A+";
				levelText.color = new Color (0f, 1f, 0f);
				dialogueText.SelectTextFile ("GoodEnding");
			} else if (player.mStats.Level < 6)
				dialogueText.SelectTextFile ("BadEnding");
		}
	}

	public void SetLevelLabel ()
	{
		switch (player.mStats.Level) {
		case 2:
			levelText.text = "E";
			levelText.color = new Color (1f, 0.25f, 0f);
			break;
		case 3:
			levelText.text = "D";
			levelText.color = new Color (1f, 0.40f, 0f);
			break;
		case 4:
			levelText.text = "C";
			levelText.color = new Color (1f, 1f, 0f);
			break;
		case 5:
			levelText.text = "B";
			levelText.color = new Color (0.5f, 1f, 0f);
			break;
		case 6:
			levelText.text = "A";
			levelText.color = new Color (0f, 1f, 0f);
			break;
		}
	}

	public IEnumerator GoToMainMenu ()
	{
		yield return new WaitForSeconds (5.0f);
		GameObject.FindGameObjectWithTag ("GameController").SendMessage ("MainMenu");
	}

	public void SetStart ()
	{
		start = true;
	}

	public void SetGameEnd ()
	{
		StartCoroutine (GoToMainMenu ());
	}
}
