using UnityEngine;
using System.Collections;

public class Level1Story : MonoBehaviour
{

	private AudioSource theme;
	
	void Start ()
	{
		AudioSource[] audioSources = GetComponents<AudioSource> ();
		theme = audioSources [0];
	}

	void Update ()
	{
		if (!theme.isPlaying) {
			theme.Play ();
		}
	}
}
