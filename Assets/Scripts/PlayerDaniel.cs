using UnityEngine;
using System.Collections;

public class PlayerDaniel : MonoBehaviour
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
	
	void Start ()
	{
		mAnimator = GetComponent<Animator> ();
        mRigidBody = GetComponent<Rigidbody>();
		mFacingDirection = Vector2.right;
		mJumping = false;
		mGetHit = false;
		mSliding = false;
		mDashing = false;
		mNormalAttack = 0;
		mStrongAttack = 0;

        mMoveSpeedX = 4.0f;
        mMoveSpeedY = 2.5f;
        mJumpForce = 5.0f;
        mGravityScale = 0.8f;

        // Init Floor stuff
        mFloorControllerRef = FindObjectOfType<FloorController>();
        mFloorBoundary = new float[4];
        mSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        mInitialOrderInLayer = (int)(transform.position.y);
        floorBoundaryInitialized = false;

        // Init HealthBar Stuff
        mHealthBarRef = FindObjectOfType<HealthBar>();
	}


	void Update ()
	{
        if (!floorBoundaryInitialized)
        {
            // get current boundary
            mFloorControllerRef.GetCurrentFloorBoundary(mFloorBoundary, mFloorIndex, mSpriteRenderer);
            floorBoundaryInitialized = true;
        }

		ResetBoolean ();
        
        if (transform.position.y < mGroundY && mJumping)
        {
            mRigidBody.useGravity = false;
            mRigidBody.velocity = new Vector3(0,0,0);
            transform.position = new Vector3(transform.position.x, mGroundY, transform.position.z);
            mJumping = false;
        }

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
                    if (mJumping)
                        direction.y = 0;
                    transform.Translate(direction * Time.deltaTime, Space.World);

                    mMoving = true;
                    mRunning = true;
                    //mWalking = true;
                    
                    // if u pass the bottom of the floor boundary 
                    if ((mJumping && mGroundY < mFloorBoundary[Floor.Y_MIN_INDEX]) || (!mJumping && transform.position.y < mFloorBoundary[Floor.Y_MIN_INDEX]))
                    {
                        int newFloorIndex = mFloorControllerRef.NextFloorDown(mFloorIndex);
                        if (newFloorIndex != mFloorIndex)
                        {
                            mFloorIndex = newFloorIndex;
                            mFloorControllerRef.GetCurrentFloorBoundary(mFloorBoundary, mFloorIndex, mSpriteRenderer);
                            mJumping = true;
                            mFalling = true;
                        }
                    }
                    else if (mJumping && !mFalling && mGroundY > mFloorBoundary[Floor.Y_MAX_INDEX])
                    {
                        int newFloorIndex = mFloorControllerRef.NextFloorUp(mFloorIndex);
                        if (newFloorIndex != mFloorIndex)
                        {
                            mFloorIndex = newFloorIndex;
                            mFloorControllerRef.GetCurrentFloorBoundary(mFloorBoundary, mFloorIndex, mSpriteRenderer);
                        }
                    }
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
        else if (Input.GetKeyDown("j") && !mJumping)
        {
            mGroundY = transform.position.y;
            Jump();
        }
        print(mRigidBody.velocity);
        CheckFalling();

        // if one is not jumping or falling, then they must be on the floor, meaning they must abide by the boundaries.
        if (!mJumping && !mFalling)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, mFloorBoundary[Floor.X_MIN_INDEX], mFloorBoundary[Floor.X_MAX_INDEX]), Mathf.Clamp(transform.position.y, mFloorBoundary[Floor.Y_MIN_INDEX], mFloorBoundary[Floor.Y_MAX_INDEX]), transform.position.z);
        }
        UpdateOrderInLayer();

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
        mRigidBody.AddForce(Vector2.up * mJumpForce, ForceMode.Impulse);
        mRigidBody.useGravity = true;
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
		mAnimator.SetBool ("isRunning", (mRunning && !mJumping));
        mAnimator.SetBool("isWalking", (mWalking && !mJumping));
		mAnimator.SetBool ("isDefending", mDefending);
		mAnimator.SetBool ("isJumping", mJumping);
        mAnimator.SetBool("isFalling", mFalling);
		mAnimator.SetBool ("isHit", mGetHit);
		mAnimator.SetBool ("isKnockdown", mGetKnockdown);
		mAnimator.SetInteger ("isHitting", mNormalAttack % 6);
		mAnimator.SetInteger ("isStrongHitting", mStrongAttack);
		mAnimator.SetBool ("isHittingBool", mHitting);
		mAnimator.SetBool ("isSliding", mSliding);
		mAnimator.SetBool ("isDashing", mDashing);
	}

    private void CheckFalling()
    {
        mFalling = (mRigidBody.velocity.y < 0.0f);
    }

    private void UpdateOrderInLayer()
    {
        mSpriteRenderer.sortingOrder = mInitialOrderInLayer - (int)(transform.position.y);
    }
}
