﻿using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour
{
	private GameObject mFollowTarget;
	public float mHorizontalDistanceFromCenter; // Set in Unity Editor
	public float mVerticalDistanceFromCenter; // Set in Unity Editor
    
	// Use this for initialization
	void Start ()
	{
		mFollowTarget = GameObject.FindGameObjectWithTag ("Player");
		transform.position = new Vector3 (mFollowTarget.transform.position.x, mFollowTarget.transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update ()
	{
		float newX = transform.position.x;
		float newY = transform.position.y;

		// determine movement on x if applicable
		if (transform.position.x - mFollowTarget.transform.position.x > mHorizontalDistanceFromCenter) { // If the target has moved to the left
			newX = mFollowTarget.transform.position.x + mHorizontalDistanceFromCenter;
		} else if (mFollowTarget.transform.position.x - transform.position.x > mHorizontalDistanceFromCenter) { // If the tagret has moved to the right
			newX = mFollowTarget.transform.position.x - mHorizontalDistanceFromCenter;
		}

		// determine movement on y if applicable.
		if (transform.position.y - mFollowTarget.transform.position.y > mVerticalDistanceFromCenter) { // If the target has moved to the left
			newY = mFollowTarget.transform.position.y + mVerticalDistanceFromCenter;
		} else if (mFollowTarget.transform.position.y - transform.position.y > mVerticalDistanceFromCenter) { // If the tagret has moved to the right
			newY = mFollowTarget.transform.position.y - mVerticalDistanceFromCenter;
		}

		transform.position = new Vector3 (newX, newY, transform.position.z);
	}

	public void SetTarget (GameObject target = null)
	{
		if (target == null) {
			mFollowTarget = GameObject.FindGameObjectWithTag ("Player");
		} else {
			mFollowTarget = target;
		}
	}
}
