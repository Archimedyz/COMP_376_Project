using UnityEngine;
using System.Collections;

public class NeanderthalAnimTest : MonoBehaviour
{
	private bool mMoving;
	private bool mThrowing;
	private bool mDying;
	private bool mGettingHit;
	
	private Vector2 mFacingDirection;
	
	public float mHoriMoveSpeed;
	public float mVertiMoveSpeed;
	
	private Animator mAnimator;

	void Start ()
	{
		mAnimator = GetComponent<Animator> ();
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
			Throw ();
		} else if (Input.GetKey ("z")) {
			Die ();
		} else if (Input.GetKey ("x")) {
			GettingHit ();
		}

		UpdateAnimator ();
	}

	private void GettingHit ()
	{
		mGettingHit = true;
	}

	private void Die ()
	{
		mDying = true;
	}

	private void Throw ()
	{
		mThrowing = true;
	}

	private void MovingLeft ()
	{
		transform.Translate (-Vector2.right * mHoriMoveSpeed * Time.deltaTime);
		FaceDirection (-Vector2.right);
		mMoving = true;
	}
	
	private void MovingRight ()
	{
		transform.Translate (Vector2.right * mHoriMoveSpeed * Time.deltaTime);
		FaceDirection (Vector2.right);
		mMoving = true;
	}
	
	private void MovingUp ()
	{
		transform.Translate (Vector2.up * mVertiMoveSpeed * Time.deltaTime);
		mMoving = true;
	}
	
	private void MovingDown ()
	{
		transform.Translate (Vector2.down * mVertiMoveSpeed * Time.deltaTime);
		mMoving = true;
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
		mThrowing = false;
		mDying = false;
		mGettingHit = false;
	}
	
	private void UpdateAnimator ()
	{
		mAnimator.SetBool ("isMoving", mMoving);
		mAnimator.SetBool ("isGettingHit", mGettingHit);
		mAnimator.SetBool ("isThrowing", mThrowing);
		mAnimator.SetBool ("isDying", mDying);
	}
}
