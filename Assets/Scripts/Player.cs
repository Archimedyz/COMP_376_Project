﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

	private bool mRunning;
	private bool mWalking;
	private bool mMoving;
	private bool mDefending;
	private bool mJumping;
	private bool mGetHit;
	private bool mGetKnockdown;
	private bool mSliding;
	private bool mDashing;
	private int mNormalAttack;

	private bool mHitting;

	public float mMoveSpeed;
	public float mJumpForce;

	private Vector2 mFacingDirection;

	private Animator mAnimator;
	private Rigidbody mRigidBody;
	
	void Start ()
	{
		mAnimator = GetComponent<Animator> ();
		mRigidBody = GetComponent<Rigidbody> ();
		mFacingDirection = Vector2.right;
		mJumping = false;
		mGetHit = false;
		mSliding = false;
		mDashing = false;
		mNormalAttack = 0;
	}


	void Update ()
	{
		ResetBoolean ();

		if (mNormalAttack > 0 && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			mNormalAttack = 0;
			mHitting = false;
		} 


		if (Input.GetKeyDown ("z")) {
			//if (!mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle"))
			mNormalAttack++;
			mHitting = true;
		}

		if (mJumping && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			mJumping = false;
		} else if (mGetHit && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			mGetHit = false;
		} else if (mSliding && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			mSliding = false;
		} 

		if (!mGetHit) {
			if (Input.GetKey ("space")) {
				Defend ();
			} else if (Input.GetKeyDown ("a")) {
				Jump ();
			} else {
				if (Input.GetButton ("Left")) {
					MovingLeft ();
				} else if (Input.GetButton ("Right")) {
					MovingRight ();
				} 
				if (Input.GetButton ("Up")) {
					MovingUp ();
				} else if (Input.GetButton ("Down")) {
					MovingDown ();
				}
			}
		}

		if (Input.GetKeyDown ("s")) {
			GetHit ();
		} else if (Input.GetKeyDown ("d")) {
			GetKnockdown ();
		} else if (mMoving && Input.GetKeyDown ("f")) {
			Slide ();
		} else if (Input.GetKeyDown ("q")) {
			Dash ();
		}

		UpdateAnimator ();
	}

	private void Dash ()
	{
		mDashing = true;
		FaceDirection (mFacingDirection);
	}

	private void Slide ()
	{
		mSliding = true;
		FaceDirection (mFacingDirection);
	}

	private void Jump ()
	{
		mJumping = true;
		FaceDirection (mFacingDirection);
		mRigidBody.AddForce (Vector2.up * mJumpForce, ForceMode.Impulse);
	}

	private void GetHit ()
	{
		ResetBoolean ();
		FaceDirection (mFacingDirection);
		mJumping = false;
		mGetHit = true;
	}

	private void GetKnockdown ()
	{
		ResetBoolean ();
		FaceDirection (mFacingDirection);
		mGetKnockdown = true;
	}

	private void Defend ()
	{
		mDefending = true;
		FaceDirection (mFacingDirection);
	}

	private void MovingLeft ()
	{
		transform.Translate (Vector2.right * mMoveSpeed * Time.deltaTime);
		FaceDirection (-Vector2.right);
		mMoving = true;
		mRunning = true;
		//mWalking = true;
	}
	
	private void MovingRight ()
	{
		transform.Translate (Vector2.right * mMoveSpeed * Time.deltaTime);
		FaceDirection (Vector2.right);
		mMoving = true;
		mRunning = true;
		//mWalking = true;
	}
	
	private void MovingUp ()
	{
		transform.Translate (Vector2.up * mMoveSpeed * Time.deltaTime);
		mMoving = true;
		mRunning = true;
		//mWalking = true;
	}
	
	private void MovingDown ()
	{
		transform.Translate (Vector2.down * mMoveSpeed * Time.deltaTime);
		mMoving = true;
		mRunning = true;
		//mWalking = true;
	}

	private void FaceDirection (Vector2 direction)
	{
		mFacingDirection = direction;
		if (direction == Vector2.right) {
			transform.localRotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);
		} else {
			transform.localRotation = Quaternion.Euler (0.0f, 180.0f, 0.0f);
		}
	}

	private void ResetBoolean ()
	{
		mMoving = false;
		mRunning = false;
		mDefending = false;
		mWalking = false;
		mGetKnockdown = false;
		mDashing = false;
	}

	private void UpdateAnimator ()
	{
		mAnimator.SetBool ("isMoving", mMoving);
		mAnimator.SetBool ("isRunning", mRunning);
		mAnimator.SetBool ("isWalking", mWalking);
		mAnimator.SetBool ("isDefending", mDefending);
		mAnimator.SetBool ("isJumping", mJumping);
		mAnimator.SetBool ("isHit", mGetHit);
		mAnimator.SetBool ("isKnockdown", mGetKnockdown);
		mAnimator.SetInteger ("isHitting", mNormalAttack % 6);
		mAnimator.SetBool ("isHittingBool", mHitting);
		mAnimator.SetBool ("isSliding", mSliding);
		mAnimator.SetBool ("isDashing", mDashing);
	}
}
