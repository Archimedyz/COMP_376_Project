using UnityEngine;
using System.Collections;

public class Hobo : MonoBehaviour
{
	public float Life;

	private bool mMoving;
	private bool mMovingRight;
	private bool mMovingLeft;
	private bool mHitting;
	private bool mDying;

	private bool mGetHit;

	private Vector2 mFacingDirection;

	public float mHoriMoveSpeed;
	public float mVertiMoveSpeed;

	public Transform mTarget;
	public float mFollowRange;
	public float mFollowSpeed;
	public float mAttackDistance;

	private Animator mAnimator;
	private Rigidbody mRigidBody;

	public float mPushBack;
	private float mInvincibleTimer;
	private float kInvincibilityDuration = 0.1f;
	
	private float dyingTimer = 0.0f;
	private float attackTimer = 0.0f;	
	public float attackTimeWait;

	// Floor Variables - START
	
	private FloorController mFloorControllerRef;
	public int mFloorIndex;
	public float[] mFloorBoundary;
	private SpriteRenderer mSpriteRenderer;
	private int mInitialOrderInLayer;
	private bool floorBoundaryInitialized;
	
	// Floor Variables - END


	
	void Start ()
	{
		mRigidBody = GetComponent<Rigidbody> ();
		mAnimator = GetComponent<Animator> ();
		mFacingDirection = Vector2.right;
		mDying = false;

		mFloorControllerRef = FindObjectOfType<FloorController> ();
		mFloorBoundary = new float[4];
		mSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer> ();
		mInitialOrderInLayer = (int)(transform.position.y);
		floorBoundaryInitialized = false;
	}

	void Update ()
	{
		if (!floorBoundaryInitialized) {
			// get current boundary
			//mFloorControllerRef.GetCurrentFloorBoundary (mFloorBoundary, mFloorIndex, mSpriteRenderer);
			floorBoundaryInitialized = true;
		}
		
		ResetBoolean ();

		if (!mGetHit && !mDying && mFloorIndex == mTarget.gameObject.GetComponent<Player> ().GetLayerIndex ()) {
			if (attackTimer > attackTimeWait && Vector2.Distance (transform.position, mTarget.position) < mAttackDistance) {
				attackTimer = 0;
				Hit ();
			} else if (Vector2.Distance (transform.position, mTarget.position) < mFollowRange && Vector2.Distance (transform.position, mTarget.position) > mAttackDistance) {
				if (transform.position.x < mTarget.position.x) {
					MovingRight ();
				} else if (transform.position.x > mTarget.position.x) {
					MovingLeft ();
				}

				if (transform.position.y < mTarget.position.y) {
					MovingUp ();
				} else if (transform.position.y > mTarget.position.y) {
					MovingDown ();
				}
			}
		}

		attackTimer += Time.deltaTime;

		if (Life <= 0 && !mDying) {
			Die ();
		}

		UpdateAnimator ();
		
		if (mDying) {
			dyingTimer += Time.deltaTime;
			if (dyingTimer >= 1.0f) {
				Destroy (gameObject);
			}
		}

		if (mGetHit) {
			mInvincibleTimer += Time.deltaTime;
			if (mInvincibleTimer >= kInvincibilityDuration) {
				mGetHit = false;
				mRigidBody.isKinematic = true;
				mInvincibleTimer = 0.0f;
			}
		}
	}

	private void Die ()
	{
		mDying = true;
	}

	public void GetHit (Vector2 direction, float damage)
	{
		if (!mGetHit && !mDying) {
			Life -= damage;
			mRigidBody.isKinematic = false;
			mGetHit = true;
			mRigidBody.velocity = Vector2.zero;
			mRigidBody.AddForce (new Vector2 (direction.x, 0.0f) * mPushBack, ForceMode.Impulse);
		}
	}

	private void Hit ()
	{
		attackTimer = 0;
		mHitting = true;
	}
	
	private void MovingLeft ()
	{
		transform.Translate (-Vector2.right * mHoriMoveSpeed * Time.deltaTime);
		mMoving = true;
		mMovingLeft = true;
	}
	
	private void MovingRight ()
	{
		transform.Translate (Vector2.right * mHoriMoveSpeed * Time.deltaTime);
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
		mAnimator.SetBool ("isDying", mDying);
	}

	public Vector2 GetFacingDirection ()
	{
		return mFacingDirection;
	}
}
