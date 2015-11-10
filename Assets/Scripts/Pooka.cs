using UnityEngine;
using System.Collections;

public class Pooka : MonoBehaviour
{
	public float Life;

	private bool mMoving;
	private bool mDead = false;
	
	private Animator mAnimator;
	private Rigidbody rb;
	
	public float mVertiMoveSpeed;
	
	public Transform mTarget;
	
	private Vector2 mFacingDirection;
	
	void Start ()
	{
		mAnimator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
	}
	
	void Update ()
	{
		ResetBoolean ();

		if (Life <= 0) {
			mDead = true;
		}

		if (!mDead) {
			if (Input.GetKey ("s")) {
				//MovingDown ();
			} else if (Input.GetKey ("w")) {
				//MovingUp ();
			}
		
			if (mTarget.position.x >= transform.position.x)
				FaceDirection (Vector2.right);
			else
				FaceDirection (Vector2.left);
		} else {

		}

		UpdateAnimator ();
	}

	//TODO change damage
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "FightCollider") {
			if (!mDead)
				Life -= 50;
			else {
				rb.isKinematic = false;
				rb.AddForce (Vector2.right * 10, ForceMode.Impulse);
			}

		}
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
	}
	
	private void UpdateAnimator ()
	{
		mAnimator.SetBool ("isMoving", mMoving);
		mAnimator.SetBool ("isDead", mDead);
	}

}
