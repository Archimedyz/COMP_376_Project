using UnityEngine;
using System.Collections;

public class DigDug : MonoBehaviour
{
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
