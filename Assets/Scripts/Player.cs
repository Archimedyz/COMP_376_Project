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
	private bool mGetKnockdown;
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
		mJumping = false;
		mGetHit = false;
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


		/*if (mJumping && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			mJumping = false;
		} else if (mGetHit && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
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
		} else if (Input.GetKeyDown ("d")) {
			GetKnockdown ();
		}*/

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

	private void GetKnockdown ()
	{
		ResetBoolean ();
		mGetKnockdown = true;
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
		mGetKnockdown = false;
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
	}
}
