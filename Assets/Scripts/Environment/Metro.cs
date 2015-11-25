using UnityEngine;
using System.Collections;

public class Metro : MonoBehaviour
{

	public float mSpeed;
	public bool mGoingRight;
	public float mDistanceToLive;
	public int mOrderInLayer;
	public float mTopOfTrack;
	public float mBottomOfTrack;

	float mInitialX;

	// Use this for initialization

	void Start ()
	{
		GetComponent<SpriteRenderer> ().sortingOrder = mOrderInLayer;
		mInitialX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Translate ((mGoingRight ? Vector3.right : Vector3.left) * mSpeed * Time.deltaTime);
		if (Mathf.Abs (mInitialX - transform.position.x) >= mDistanceToLive) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Player") {
            
			// get the player's ground y position and determine if he is to be hit.
			float playerGroundY = col.gameObject.GetComponent<Player> ().GetFootY ();
			if (playerGroundY < mTopOfTrack && playerGroundY > mBottomOfTrack) {
				Debug.Log ("Metro hit Player.");
			}
		}
	}

	public void SetOrderInLayer (int orderInLayer)
	{
		mOrderInLayer = orderInLayer;
	}

	public void SetGoingRight (bool goingRight)
	{
		mGoingRight = goingRight;
	}

	public void SetDistanceToLive (float distanceToLive)
	{
		mDistanceToLive = distanceToLive;
	}

	public void SetTopOfTrack (float topOfTrack)
	{
		mTopOfTrack = topOfTrack;
	}

	public void SetBottomOfTrack (float bottomOfTrack)
	{
		mBottomOfTrack = bottomOfTrack;
	}
}
