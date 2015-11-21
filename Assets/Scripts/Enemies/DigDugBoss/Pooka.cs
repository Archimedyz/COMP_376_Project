﻿using UnityEngine;
using System.Collections;

public class Pooka : MonoBehaviour
{
	public float Life;

	private bool mMoving;
	private bool mDead = false;
	private bool mExplode = false;

	private bool moveDown = true, moveUp = false;
	private bool canMove = false;
	
	private Animator mAnimator;
	private Rigidbody rb;
	
	public float mVertiMoveSpeed;
	
	private Transform mTarget;
	
	private Vector2 mFacingDirection;

	private float destroyTimer = 0.0f;

	public Vector3 initialPosition;
	private float maxY, minY;
	
	void Start ()
	{
		mAnimator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		mTarget = GameObject.Find ("Player").transform;
	}
	
	void Update ()
	{
		ResetBoolean ();

		if (mExplode) {
			destroyTimer += Time.deltaTime;
			if (destroyTimer >= 0.3f) {
				Destroy (gameObject);
			}
		}

		if (canMove) {
			if (Life <= 0) {
				mDead = true;
			} else {
				if (transform.position.y - 0.1f <= minY) {
					moveUp = true;
					moveDown = false;
				} else if (transform.position.y + 0.1f >= maxY) {
					moveUp = false;
					moveDown = true;
				}

				if (moveDown) {
					MovingDown ();
				} else if (moveUp) {
					MovingUp ();
				}
		
				if (mTarget.position.x >= transform.position.x)
					FaceDirection (Vector2.right);
				else
					FaceDirection (Vector2.left);
			}
		} else {
			MovingUp ();
		}

		UpdateAnimator ();
	}

	//TODO change damage
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "FightCollider") {
			if (!mDead)
				Life -= 50;
			else if (GameObject.Find ("Player").GetComponent<Player> ().IsStrongAttack ()) {
				rb.isKinematic = false;
				rb.AddForce (Vector2.right * 10, ForceMode.Impulse);
			}
		} else if (col.gameObject.name == "DigDug") {
			mExplode = true;
		} 
	}
	
	private void MovingUp ()
	{
		transform.Translate (new Vector2 (0.3f, 1.0f) * mVertiMoveSpeed * Time.deltaTime);
		mMoving = true;
	}
	
	private void MovingDown ()
	{
		transform.Translate (new Vector2 (-0.3f, -1.0f) * mVertiMoveSpeed * Time.deltaTime);
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
		mAnimator.SetBool ("isExploding", mExplode);
	}

	public void SetLife (int life)
	{
		Life = life;
	}

	public void SetBoundaries (float max, float min)
	{
		maxY = max;
		minY = min;
	}

	public void SetCanMove (bool a)
	{
		canMove = a;
	}

	public Vector3 GetInitialPosition ()
	{
		return initialPosition;
	}

	public void SetSpeed (float speed)
	{
		mVertiMoveSpeed = speed;
	}
}
