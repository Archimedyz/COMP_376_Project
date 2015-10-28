using UnityEngine;
using System.Collections;

public class Enemy1AnimTest : MonoBehaviour
{
	private bool mMoving;
	private bool mMovingRight;
	private bool mMovingLeft;
	private bool mHitting;

	private Vector2 mFacingDirection;

	public float mHoriMoveSpeed;
	public float mVertiMoveSpeed;

	private Animator mAnimator;

	void Start ()
	{
		mAnimator = GetComponent<Animator> ();
		mFacingDirection = Vector2.right;
	}

	void Update ()
	{
		ResetBoolean ();

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

		if (Input.GetKey ("space")) {
			Hit ();
		}

		UpdateAnimator ();
	}

	private void Hit ()
	{
		mHitting = true;
	}
	
	private void MovingLeft ()
	{
		transform.Translate (-Vector2.right * mHoriMoveSpeed * Time.deltaTime);
		//mFacingDirection = -Vector2.right;
		mMoving = true;
		mMovingLeft = true;
	}
	
	private void MovingRight ()
	{
		transform.Translate (Vector2.right * mHoriMoveSpeed * Time.deltaTime);
		//mFacingDirection = Vector2.right;
		mMoving = true;
		mMovingRight = true;
	}
	
	private void MovingUp ()
	{
		transform.Translate (Vector2.up * mVertiMoveSpeed * Time.deltaTime);
		mMoving = true;
		mMovingRight = true;
	}
	
	private void MovingDown ()
	{
		transform.Translate (Vector2.down * mVertiMoveSpeed * Time.deltaTime);
		mMoving = true;
		mMovingLeft = true;
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
		mMovingLeft = false;
		mMovingRight = false;
		mHitting = false;
	}

	private void UpdateAnimator ()
	{
		mAnimator.SetBool ("isMovingRight", mMovingRight);
		mAnimator.SetBool ("isMovingLeft", mMovingLeft);
		mAnimator.SetBool ("isHitting", mHitting);
	}
}
