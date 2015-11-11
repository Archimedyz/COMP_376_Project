﻿using UnityEngine;
using System.Collections;

public class DigDug : MonoBehaviour
{
	public GameObject Tile1;
	public GameObject Tile2;
	public GameObject Tile3;
	public GameObject Tile4;

	public float tileRangeXMin;
	public float tileRangeXMax;
	public float tileRangeY;

	private Animator mAnimator;
	private Rigidbody rb;

	private bool mMoving;
	private bool mDead = false;
	private bool mThrowing = false;
	private bool mPumping = false;
	private bool mHit;

	private float hitTimer = 0.0f;

	void Start ()
	{
		mAnimator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
	}

	void Update ()
	{
		ResetBoolean ();

		if (Input.GetKey ("z")) {
			ThrowTiles ();
		}

		if (mHit) {
			hitTimer += Time.deltaTime;
			if (hitTimer >= 1.0f) {
				mHit = false;
			}
		} else {
			hitTimer = 0.0f;
		}

		UpdateAnimator ();
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Enemy") {
			mHit = true;
			UpdateAnimator ();
		}
	}

	private void ThrowTiles ()
	{
		for (int i = 0; i < 5; i++) {
			int a = Random.Range (0, 4);
			if (a == 0) {
				Instantiate (Tile1, new Vector3 (Random.Range (tileRangeXMin, tileRangeXMax), tileRangeY, -1.0f), Quaternion.identity);
			} else if (a == 1) {
				Instantiate (Tile2, new Vector3 (Random.Range (tileRangeXMin, tileRangeXMax), tileRangeY, -1.0f), Quaternion.identity);
			} else if (a == 2) {
				Instantiate (Tile3, new Vector3 (Random.Range (tileRangeXMin, tileRangeXMax), tileRangeY, -1.0f), Quaternion.identity);
			} else if (a == 3) {
				Instantiate (Tile4, new Vector3 (Random.Range (tileRangeXMin, tileRangeXMax), tileRangeY, -1.0f), Quaternion.identity);
			}
		}
	}

	private void ResetBoolean ()
	{
		mMoving = false;
	}

	private void UpdateAnimator ()
	{
		mAnimator.SetBool ("isMoving", mMoving);
		mAnimator.SetBool ("isDead", mDead);
		mAnimator.SetBool ("isThrowing", mThrowing);
		mAnimator.SetBool ("isPumping", mPumping);
		mAnimator.SetBool ("isHit", mHit);
	}
}
