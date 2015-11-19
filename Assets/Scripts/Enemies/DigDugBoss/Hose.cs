using UnityEngine;
using System.Collections;

public class Hose : MonoBehaviour
{
	private Vector3 target;
	
	private float startTime;
	private float timer = 0.0f;
	private float journeyLength;

	private GameObject[] hoseParts;
	private int hoseNumber;

	void Start ()
	{
		target = new Vector3 (transform.position.x - 2.0f, transform.position.y, transform.position.z);        
		startTime = Time.time;
		journeyLength = Vector3.Distance (transform.position, target);
		hoseParts = new GameObject[transform.childCount];

		for (int i = 0; i < transform.childCount; i++) {
			hoseParts [i] = transform.GetChild (i).gameObject;
		}
	}
	
	void Update ()
	{
		if (transform.position.x > target.x) {
			if (timer == 0.0f || (timer >= 0.1f && hoseNumber < 4)) {
				hoseParts [hoseNumber].SetActive (true);
				hoseNumber ++;
				timer = 0.0f;
			}
			float distCovered = (Time.time - startTime) * 0.8f;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp (transform.position, target, fracJourney);
		}
		
		timer += Time.deltaTime;
	}
}