using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

	private bool mRunning;
	private bool mWalking;
	private bool mMoving;
	private bool mDefending;

	public float mMoveSpeed;

	private Vector2 mFacingDirection;

	private Animator mAnimator;
	
	void Start ()
	{
		mAnimator = GetComponent<Animator> ();
	}


	void Update ()
	{
		resetBoolean ();

		if (Input.GetKey ("space")) {
			Defend ();
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

		UpdateAnimator ();
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

	private void resetBoolean ()
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
	}
}
