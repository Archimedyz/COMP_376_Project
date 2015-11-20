﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

	private bool mRunning;
	private bool mWalking;
	private bool mMoving;
	private bool mDefending;
	private bool mJumping;
	private bool mFalling;
	private bool mGetHit;
	private bool mGetKnockdown;
	private bool mSliding;
	private bool mDashing;
	private int mNormalAttack;
	private int mStrongAttack;
	private float mGroundY;
	
	private bool canMove = true;

	private bool mHitting;

	public float mMoveSpeedX;
	public float mMoveSpeedY;
	public float mJumpForce;
	public float mGravityScale;


	private Vector2 mFacingDirection;

	private Animator mAnimator;
	Rigidbody mRigidBody;

	// Floor Variables - START

	private FloorController mFloorControllerRef;
	public int mFloorIndex;
	public float[] mFloorBoundary;
	private SpriteRenderer mSpriteRenderer;
	private int mInitialOrderInLayer;
	private bool floorBoundaryInitialized;

	// Floor Variables - END

	// Health Bar Variables - Start

	private HealthBar mHealthBarRef;

	// Health Bar Variables - End

	//Combat variables - Start
	
	private float mInvincibleTimer;
	private float kInvincibilityDuration = 0.5f;
	private float kKnockdownInvincibilityDuration = 0.5f;
	public float mHitPushBack;
	public float mKnockdownPushBack;

	public Stats mStats;

	//Combat variables - End

	//Sound Variables - START

	AudioSource strongPunch;
	AudioSource normalPunch;

	//Sound Variables - END

	void Start ()
	{
		mStats = new Stats (79.0f, 5, 5, 5);
		mAnimator = GetComponent<Animator> ();
		mAnimator.speed = 1 + (float)(mStats.Spd / 20.0f);
		mRigidBody = GetComponent<Rigidbody> ();
		mFacingDirection = Vector2.right;
		mJumping = false;
		mSliding = false;
		mDashing = false;
		mNormalAttack = 0;
		mStrongAttack = 0;

		mMoveSpeedX = 4.0f;
		mMoveSpeedY = 2.5f;
		mJumpForce = 5.0f;
		mGravityScale = 0.8f;

		// Init Floor stuff
		mFloorControllerRef = FindObjectOfType<FloorController> ();
		mFloorBoundary = new float[4];
		mSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer> ();
		mInitialOrderInLayer = (int)(transform.position.y);
		floorBoundaryInitialized = false;

		// Init HealthBar Stuff
		mHealthBarRef = FindObjectOfType<HealthBar> ();
		//mHealthBarRef.MaxHealthValue = 500.0f;

		//AudioSource[] audioSources = GetComponents<AudioSource> ();

	}


	void Update ()
	{

		if (!floorBoundaryInitialized) {
			// get current boundary
			mFloorControllerRef.GetCurrentFloorBoundary (mFloorBoundary, mFloorIndex, mSpriteRenderer);
			floorBoundaryInitialized = true;
		}

		ResetBoolean ();
        
		if (transform.position.y < mGroundY && mJumping) {
			mRigidBody.useGravity = false;
			mRigidBody.velocity = new Vector3 (0, 0, 0);
			transform.position = new Vector3 (transform.position.x, mGroundY, transform.position.z);
			mJumping = false;
		}

		if (mNormalAttack > 0 && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			mNormalAttack = 0;
			mHitting = false;
		} 


		if (Input.GetKeyDown ("z")) {
			mNormalAttack++;
			mHitting = true;
		} else if (Input.GetKey ("x")) {
			mStrongAttack ++;
		}

		if (mJumping && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			mJumping = false;
		} else if (mSliding && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			mSliding = false;
		}

		if (!mGetHit && !mGetKnockdown && CanMove ()) {
			if (Input.GetKey ("space")) {
				Defend ();
			} else {
				float horizontal = Input.GetAxis ("Horizontal");
				float vertical = Input.GetAxis ("Vertical");

				if (!Mathf.Approximately (vertical, 0.0f) || !Mathf.Approximately (horizontal, 0.0f)) {
					Vector3 direction = new Vector3 (horizontal, vertical, 0.0f);
					direction = Vector3.ClampMagnitude (direction, 1.0f);
					if (direction.x > 0.00f)
						FaceDirection (Vector2.right);
					else {
						if (direction.x < 0.00f)
							FaceDirection (Vector2.left);
					}
					direction.x = direction.x * mMoveSpeedX;
					direction.y = direction.y * mMoveSpeedY;
					if (mJumping) {
						direction /= 2.0f;
						mGroundY += direction.y * Time.deltaTime; 
						direction.y = 0;
					}
					transform.Translate (direction * Time.deltaTime, Space.World);

					mMoving = true;
					mRunning = true;
                    
					// if u pass the bottom of the floor boundary 
					if ((mJumping && mGroundY < mFloorBoundary [Floor.Y_MIN_INDEX]) || (!mJumping && transform.position.y < mFloorBoundary [Floor.Y_MIN_INDEX])) {
						int newFloorIndex = mFloorControllerRef.NextFloorDown (mFloorIndex);
						if (newFloorIndex != mFloorIndex) {
							mFloorIndex = newFloorIndex;
							mFloorControllerRef.GetCurrentFloorBoundary (mFloorBoundary, mFloorIndex, mSpriteRenderer);
							mJumping = true;
							mFalling = true;
							mGroundY = mFloorBoundary [Floor.Y_MAX_INDEX];
							mRigidBody.useGravity = true;
						}
					} else if (mJumping && mGroundY > mFloorBoundary [Floor.Y_MAX_INDEX]) {
						int newFloorIndex = mFloorControllerRef.NextFloorUp (mFloorIndex);
						if (newFloorIndex != mFloorIndex) {
							// check if the player has even reached the next level in terms of animation.
							float[] newFloorBoundary = new float[4];
							mFloorControllerRef.GetCurrentFloorBoundary (newFloorBoundary, newFloorIndex, mSpriteRenderer);
                            
							if (transform.position.y > newFloorBoundary [Floor.Y_MIN_INDEX]) {
								mFloorIndex = newFloorIndex;
								mFloorBoundary = newFloorBoundary;
								mGroundY = mFloorBoundary [Floor.Y_MIN_INDEX];
							}
						}
					}
				}
			}
		}

		if (mMoving && Input.GetKeyDown ("f")) {
			Slide ();
		} else if (Input.GetKeyDown ("q")) {
			Dash ();
		} else if (Input.GetKeyDown ("j") && !mJumping) {
			mGroundY = transform.position.y;
			Jump ();
		}

		CheckFalling ();

		// if one is not jumping or falling, then they must be on the floor, meaning they must abide by the boundaries.
		if (!mJumping && !mFalling) {
			transform.position = new Vector3 (Mathf.Clamp (transform.position.x, mFloorBoundary [Floor.X_MIN_INDEX], mFloorBoundary [Floor.X_MAX_INDEX]), Mathf.Clamp (transform.position.y, mFloorBoundary [Floor.Y_MIN_INDEX], mFloorBoundary [Floor.Y_MAX_INDEX]), transform.position.z);

		}
		UpdateOrderInLayer ();

		UpdateAnimator ();

		if (mGetHit) {
			mInvincibleTimer += Time.deltaTime;
			if (mInvincibleTimer >= kInvincibilityDuration) {
				mGetHit = false;
				mRigidBody.isKinematic = true;
				mInvincibleTimer = 0.0f;
			}
		}
		
		if (mGetKnockdown) {
			mInvincibleTimer += Time.deltaTime;
			if (mInvincibleTimer >= kKnockdownInvincibilityDuration) {
				mGetKnockdown = false;
				mRigidBody.isKinematic = true;
				mInvincibleTimer = 0.0f;
			}
		}
	}

	private void Dash ()
	{
		mDashing = true;
		FaceDirection (mFacingDirection);
	}

	private void Slide ()
	{
		mSliding = true;
		FaceDirection (mFacingDirection);
	}

	private void Jump ()
	{
		mJumping = true;
		FaceDirection (mFacingDirection);
		mRigidBody.AddForce (Vector2.up * mJumpForce, ForceMode.Impulse);
		mRigidBody.useGravity = true;
	}

	public void GetHit (Vector2 direction, int damage)
	{
		if (!mGetHit && !mGetKnockdown) {
			mGetHit = true;
			mHealthBarRef.LoseHealth (damage);
			mRigidBody.isKinematic = false;
			mRigidBody.velocity = Vector2.zero;
			mRigidBody.AddForce (new Vector2 (direction.x, 0.0f) * mHitPushBack, ForceMode.Impulse);
		}
	}
	
	public void GetKnockdown (Vector2 direction, int damage)
	{
		if (!mGetHit && !mGetKnockdown) {
			mGetKnockdown = true;
			mHealthBarRef.LoseHealth (damage);
			mRigidBody.isKinematic = false;
			mRigidBody.velocity = Vector2.zero;
			mRigidBody.AddForce (new Vector2 (-direction.x, 0.0f) * mKnockdownPushBack, ForceMode.Impulse);
		}
	}

	private void Defend ()
	{
		mDefending = true;
		FaceDirection (mFacingDirection);
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

	private bool CanMove ()
	{
		bool cond1 = !mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("GetKnockdown");
		bool cond2 = !mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("GetUp");
		bool cond3 = !mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("GetHit");
		return cond1 && cond2 & cond3;
	}

	private void ResetBoolean ()
	{
		mStrongAttack = 0;
		mMoving = false;
		mRunning = false;
		mDefending = false;
		mWalking = false;
		mDashing = false;
	}

	private void UpdateAnimator ()
	{
		mAnimator.SetBool ("isMoving", mMoving);
		mAnimator.SetBool ("isRunning", (mRunning && !mJumping));
		mAnimator.SetBool ("isWalking", (mWalking && !mJumping));
		mAnimator.SetBool ("isDefending", mDefending);
		mAnimator.SetBool ("isJumping", mJumping);
		mAnimator.SetBool ("isFalling", mFalling);
		mAnimator.SetBool ("isHit", mGetHit);
		mAnimator.SetBool ("isKnockdown", mGetKnockdown);
		mAnimator.SetInteger ("isHitting", mNormalAttack % 6);
		mAnimator.SetInteger ("isStrongHitting", mStrongAttack);
		mAnimator.SetBool ("isHittingBool", mHitting);
		mAnimator.SetBool ("isSliding", mSliding);
		mAnimator.SetBool ("isDashing", mDashing);
	}

	private void CheckFalling ()
	{
		mFalling = (mRigidBody.velocity.y < 0.0f);
	}

	private void UpdateOrderInLayer ()
	{
		mSpriteRenderer.sortingOrder = mInitialOrderInLayer - (int)(transform.position.y);
	}

	public Vector2 GetFacingDirection ()
	{
		return mFacingDirection;
	}

	public int GetLayerIndex ()
	{
		return mFloorIndex;
	}

	public void SetCanMove (bool a)
	{
		canMove = a;
	}
	
	public void SetPosition (Vector3 position)
	{
		transform.position = position;
	}
	
	public bool IsStrongAttack ()
	{
		return mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("StrongAttackPhase2");
	}
}