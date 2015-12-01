using UnityEngine;
using System.Collections;

public class ParallaxBackground : MonoBehaviour
{

	private ParallaxLayer[] mLayers;
	private Vector3 mInitPosition;
	private float oldX;
	private float absoluteY = 0.0f;

	// Use this for initialization
	void Start ()
	{
		mLayers = GetComponentsInChildren<ParallaxLayer> ();
		for (int i = 0; i < mLayers.Length; ++i) {
			mLayers [i].SetParallaxFactor (0.10f - 0.025f * i);
			absoluteY = Mathf.Max (mLayers [i].GetComponent<SpriteRenderer> ().bounds.size.y / 2.0f, absoluteY);
		}
		mInitPosition = transform.position;
		oldX = transform.position.x;
		GameObject firstFloor = FindObjectOfType<FloorController> ().GetComponentsInChildren<Floor> () [0].gameObject;
		absoluteY += firstFloor.transform.position.y + (firstFloor.GetComponentInChildren<SpriteRenderer> ().bounds.size.y / 2.0f);  
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = new Vector3 (transform.position.x, absoluteY, mInitPosition.z);
		if (transform.position.x != oldX) {
			float deltaX = transform.position.x - oldX;
			oldX = transform.position.x;
			foreach (ParallaxLayer layer in mLayers) {
				layer.Move (deltaX);
			}
		}
	}
}
