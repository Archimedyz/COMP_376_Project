using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

	private bool mRunning;
	private bool mWalking;
	private bool mMoving;
	private bool mDefending;
	private bool mJumping;
	private bool mGetHit;

	public float mMoveSpeed;
	public float mJumpForce;

	private Vector2 mFacingDirection;

	private Animator mAnimator;
	private Rigidbody mRigidBody;
	
	void Start ()
	{
		mAnimator = GetComponent<Animator> ();
		mRigidBody = GetComponent<Rigidbody> ();
		mJumping = false;
		mGetHit = false;
	}


	void Update ()
	{
		ResetBoolean ();

		if (mJumping && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			mJumping = false;
		} else if (mGetHit && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			Debug.Log ("Get hit");
			mGetHit = false;
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
		} 

		UpdateAnimator ();
	}

	private void Jump ()
	{
		mJumping = true;
		mRigidBody.AddForce (Vector2.up * mJumpForce, ForceMode.Impulse);
	}

	private void GetHit ()
	{
		ResetBoolean ();
		mJumping = false;
		mGetHit = true;
	}

	private void Defend ()
	{
		mDefending = true;
	}

	private void MovingLeft ()
	{
		transform.Translate (-Vector2.right * mMoveSpeed * Time.deltaTime);
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
			Vector3 newScale = new Vector3 (Mathf.Abs (transform.localScale.x), transform.localScale.y, transform.localScale.z);
			transform.localScale = newScale;
		} else {
			Vector3 newScale = new Vector3 (-Mathf.Abs (transform.localScale.x), transform.localScale.y, transform.localScale.z);
			transform.localScale = newScale;
		}
	}

	private void ResetBoolean ()
	{
		mMoving = false;
		mRunning = false;
		mDefending = false;
		mWalking = false;
	}

	private void UpdateAnimator ()
	{
		mAnimator.SetBool ("isMoving", mMoving);
		mAnimator.SetBool ("isRunning", mRunning);
		mAnimator.SetBool ("isWalking", mWalking);
		mAnimator.SetBool ("isDefending", mDefending);
		mAnimator.SetBool ("isJumping", mJumping);
		mAnimator.SetBool ("isHit", mGetHit);
	}
}
