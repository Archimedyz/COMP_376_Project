using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

	private bool mRunning;
	private bool mMoving;

	public float mMoveSpeed;

	private Vector2 mFacingDirection;

	private Animator mAnimator;
	
	void Start ()
	{
		mAnimator = GetComponent<Animator> ();
	}


	void Update ()
	{
		mMoving = false;
		mRunning = false;

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

		UpdateAnimator ();
	}

	private void MovingLeft ()
	{
		transform.Translate (-Vector2.right * mMoveSpeed * Time.deltaTime);
		FaceDirection (-Vector2.right);
		mMoving = true;
		mRunning = true;
	}
	
	private void MovingRight ()
	{
		transform.Translate (Vector2.right * mMoveSpeed * Time.deltaTime);
		FaceDirection (Vector2.right);
		mMoving = true;
		mRunning = true;
	}
	
	private void MovingUp ()
	{
		transform.Translate (Vector2.up * mMoveSpeed * Time.deltaTime);
		mMoving = true;
		mRunning = true;
	}
	
	private void MovingDown ()
	{
		transform.Translate (Vector2.down * mMoveSpeed * Time.deltaTime);
		mMoving = true;
		mRunning = true;
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

	private void UpdateAnimator ()
	{
		Debug.Log (mRunning);
		mAnimator.SetBool ("isMoving", mMoving);
		mAnimator.SetBool ("isRunning", mRunning);
	}
}
