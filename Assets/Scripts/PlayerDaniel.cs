using UnityEngine;
using System.Collections;

public class PlayerDaniel : MonoBehaviour
{

	private bool mRunning;
	private bool mWalking;
	private bool mMoving;
	private bool mDefending;
	private bool mJumping;
	private bool mGetHit;
	private bool mGetKnockdown;
	private bool mSliding;
	private bool mDashing;
	private int mNormalAttack;
	private int mStrongAttack;

	private bool mHitting;

	//public float mMoveSpeed;
    public float mMoveSpeedX;
    public float mMoveSpeedY;
	public float mJumpForce;

	private Vector2 mFacingDirection;

	private Animator mAnimator;
    Rigidbody2D mRigidBody2D;
	
	void Start ()
	{
		mAnimator = GetComponent<Animator> ();
        mRigidBody2D = GetComponent<Rigidbody2D>();
		mFacingDirection = Vector2.right;
		mJumping = false;
		mGetHit = false;
		mSliding = false;
		mDashing = false;
		mNormalAttack = 0;
		mStrongAttack = 0;
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
		} else if (Input.GetKey ("w")) {
			mStrongAttack ++;
			//mHitting = true;
		}

		if (mJumping && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			mJumping = false;
		} else if (mGetHit && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			mGetHit = false;
		} else if (mSliding && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			mSliding = false;
		} 

		if (!mGetHit) {
			if (Input.GetKey ("space")) {
				Defend ();
            }
            else
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                if (!Mathf.Approximately(vertical, 0.0f) || !Mathf.Approximately(horizontal, 0.0f))
                {
                    Vector3 direction = new Vector3(horizontal, vertical, 0.0f);
                    direction = Vector3.ClampMagnitude(direction, 1.0f);
                    if (direction.x > 0.00f)
                        FaceDirection(Vector2.right);
                    else
                    {
                        if (direction.x < 0.00f)
                            FaceDirection(Vector2.left);
                    }
                    direction.x = direction.x * mMoveSpeedX;
                    direction.y = direction.y * mMoveSpeedY;
                    transform.Translate(direction * Time.deltaTime, Space.World);

                    mMoving = true;
                    mRunning = true;
                    //mWalking = true;
                }
            }
		}

		if (Input.GetKeyDown ("s")) {
			GetHit ();
		} else if (Input.GetKeyDown ("d")) {
			GetKnockdown ();
		} else if (mMoving && Input.GetKeyDown ("f")) {
			Slide ();
		} else if (Input.GetKeyDown ("q")) {
			Dash ();
		}

		UpdateAnimator ();
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
        mRigidBody2D.AddForce(Vector2.up * mJumpForce, ForceMode2D.Impulse);
	}

	private void GetHit ()
	{
		ResetBoolean ();
		FaceDirection (mFacingDirection);
		mJumping = false;
		mGetHit = true;
	}

	private void GetKnockdown ()
	{
		ResetBoolean ();
		FaceDirection (mFacingDirection);
		mGetKnockdown = true;
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
            Vector3 newScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            transform.localScale = newScale;
		}
	}

	private void ResetBoolean ()
	{
		mStrongAttack = 0;
		mMoving = false;
		mRunning = false;
		mDefending = false;
		mWalking = false;
		mGetKnockdown = false;
		mDashing = false;
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
		mAnimator.SetInteger ("isStrongHitting", mStrongAttack);
		mAnimator.SetBool ("isHittingBool", mHitting);
		mAnimator.SetBool ("isSliding", mSliding);
		mAnimator.SetBool ("isDashing", mDashing);
	}
}
