using UnityEngine;
using System.Collections;

public class Neanderthal : MonoBehaviour
{
	private bool mMoving;
	private bool mThrowing;
	private bool mDying;
	private bool mGetHit;
	
	private Vector2 mFacingDirection;
	
	public float mHoriMoveSpeed;
	public float mVertiMoveSpeed;
	
	private Animator mAnimator;
	private Rigidbody mRigidBody;

	public GameObject projectile;
	private GameObject coconut;

	public Transform mTarget;
	public float mFollowRange;

	public float mPushBack;
	private float mInvincibleTimer;
	private float kInvincibilityDuration = 0.1f;
	
	public float mAttackDistance;

	public float attackTimeWait;
	private float attackTimer = 0.0f;

	void Start ()
	{
		mRigidBody = GetComponent<Rigidbody> ();
		mAnimator = GetComponent<Animator> ();
		coconut = null;
	}

	void Update ()
	{
		ResetBoolean ();

		if (attackTimer > attackTimeWait && Vector2.Distance (transform.position, mTarget.position) < mAttackDistance && mTarget.position.y < (transform.position.y + 1) && mTarget.position.y > (transform.position.y - 1)) {
			attackTimer = 0;
			Throw ();
		} else if (Vector2.Distance (transform.position, mTarget.position) < mFollowRange) {
			if (mTarget.position.x >= transform.position.x)
				FaceDirection (Vector2.right);
			else
				FaceDirection (Vector2.left);
			
			if (mTarget.position.y > (transform.position.y + 1)) {
				MovingUp ();
			} else if (mTarget.position.y < (transform.position.y - 1)) {
				MovingDown ();
			}
		}

		attackTimer += Time.deltaTime;
		UpdateAnimator ();

		if (mGetHit) {
			mInvincibleTimer += Time.deltaTime;
			if (mInvincibleTimer >= kInvincibilityDuration) {
				mGetHit = false;
				mRigidBody.isKinematic = true;
				mInvincibleTimer = 0.0f;
			}
		}
	}

	public void GetHit (Vector2 direction)
	{
		if (!mGetHit) {
			attackTimer = 0;
			mRigidBody.isKinematic = false;
			mGetHit = true;
			mRigidBody.velocity = Vector2.zero;
			mRigidBody.AddForce (new Vector2 (direction.x, 0.0f) * mPushBack, ForceMode.Impulse);
		}
	}

	private void Die ()
	{
		mDying = true;
	}

	private void Throw ()
	{
		if (!mGetHit) {
			mThrowing = true;
			coconut = Instantiate (projectile, new Vector3 (transform.position.x - 0.15f, transform.position.y - 0.3f, transform.position.z), Quaternion.identity) as GameObject;
			coconut.gameObject.GetComponent<Coconut> ().SetDirection (-mFacingDirection);
			coconut.transform.parent = gameObject.transform;
		}
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
	}
	
	private void UpdateAnimator ()
	{
		mAnimator.SetBool ("isMoving", mMoving);
		mAnimator.SetBool ("isGettingHit", mGetHit);
		mAnimator.SetBool ("isThrowing", mThrowing);
		mAnimator.SetBool ("isDying", mDying);
	}
}
