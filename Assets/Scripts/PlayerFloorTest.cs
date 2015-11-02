using UnityEngine;
using System.Collections;

public class PlayerFloorTest : MonoBehaviour
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

    private bool mHitting;

    public float mMoveSpeed;
    public float mJumpForce;

    private Vector2 mFacingDirection;

    private Animator mAnimator;
    private Rigidbody mRigidBody;

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

    void Start()
    {
        mAnimator = GetComponent<Animator>();
        mRigidBody = GetComponent<Rigidbody>();
        mFacingDirection = Vector2.right;
        mJumping = false;
        mGetHit = false;
        mSliding = false;
        mDashing = false;
        mNormalAttack = 0;
        mStrongAttack = 0;

        mMoveSpeed = 5.0f;
        mJumpForce = 2.0f;

        // Init Floor stuff
        mFloorControllerRef = FindObjectOfType<FloorController>();
        mFloorBoundary = new float[4];
        mSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        mInitialOrderInLayer = (int)(transform.position.y);
        floorBoundaryInitialized = false;

        // Init HealthBar Stuff
        mHealthBarRef = FindObjectOfType<HealthBar>();
    }


    void Update()
    {

        if (!floorBoundaryInitialized)
        {
            // get current boundary
            mFloorControllerRef.GetCurrentFloorBoundary(mFloorBoundary, mFloorIndex, mSpriteRenderer);
            floorBoundaryInitialized = true;
        }

        ResetBoolean();

        if (mNormalAttack > 0 && mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            mNormalAttack = 0;
            mHitting = false;
        }

        if (Input.GetKeyDown("n"))
        {
            Debug.Log("Lose Health");
            mHealthBarRef.LoseHealth(20.0f);
        }
        else if (Input.GetKeyDown("m"))
        {
            Debug.Log("Gain Health");
            mHealthBarRef.GainHealth(15.0f);
        }

        if (Input.GetKeyDown("z"))
        {
            //if (!mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle"))
            mNormalAttack++;
            mHitting = true;
        }
        else if (Input.GetKey("w"))
        {
            mStrongAttack++;
            //mHitting = true;
        }

        if (mJumping && transform.position.y <= 0)
        {
            mJumping = false;
        }
        else if (mGetHit && mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            mGetHit = false;
        }
        else if (mSliding && mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            mSliding = false;
        }

        if (mAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jumping"))
        {

            mFalling = true;
        }
        else
        {
            mFalling = false;
        }

        if (!mGetHit)
        {
            if (Input.GetKey("space"))
            {
                Defend();
            }
            else if (Input.GetKeyDown("a"))
            {
                Jump();
            }
            else
            {
                if (Input.GetButton("Left"))
                {
                    MovingLeft();
                }
                else if (Input.GetButton("Right"))
                {
                    MovingRight();
                }
                if (Input.GetButton("Up"))
                {
                    MovingUp();
                }
                else if (Input.GetButton("Down"))
                {
                    MovingDown();
                    // if u pass the bottom of the floor boundary 
                    if(transform.position.y < mFloorBoundary[Floor.Y_MIN_INDEX]) {
                        int newFloorIndex = mFloorControllerRef.NextFloorDown(mFloorIndex);
                        if(newFloorIndex != mFloorIndex) {
                            mFloorIndex = newFloorIndex;
                            mFloorControllerRef.GetCurrentFloorBoundary(mFloorBoundary, mFloorIndex, mSpriteRenderer);
                            mJumping = true;
                            mFalling = true;
                        }
                    }
                }

                transform.position = new Vector3(Mathf.Clamp(transform.position.x, mFloorBoundary[Floor.X_MIN_INDEX], mFloorBoundary[Floor.X_MAX_INDEX]), Mathf.Clamp(transform.position.y, mFloorBoundary[Floor.Y_MIN_INDEX], mFloorBoundary[Floor.Y_MAX_INDEX]), transform.position.z);
                UpdateOrderInLayer();
            }
        }

        if (Input.GetKeyDown("s"))
        {
            GetHit();
        }
        else if (Input.GetKeyDown("d"))
        {
            GetKnockdown();
        }
        else if (mMoving && Input.GetKeyDown("f"))
        {
            Slide();
        }
        else if (Input.GetKeyDown("q"))
        {
            Dash();
        }

        // if one is not jumping or falling, then they must be on the floor, meaning they must abide by the boundaries.
        if(!mJumping && !mFalling) 
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, mFloorBoundary[Floor.X_MIN_INDEX], mFloorBoundary[Floor.X_MAX_INDEX]), Mathf.Clamp(transform.position.y, mFloorBoundary[Floor.Y_MIN_INDEX], mFloorBoundary[Floor.Y_MAX_INDEX]), transform.position.z);
        }
        UpdateOrderInLayer();

        UpdateAnimator();
    }

    private void Dash()
    {
        mDashing = true;
        FaceDirection(mFacingDirection);
    }

    private void Slide()
    {
        mSliding = true;
        FaceDirection(mFacingDirection);
    }

    private void Jump()
    {
        mJumping = true;
        FaceDirection(mFacingDirection);
        mRigidBody.AddForce(Vector2.up * mJumpForce, ForceMode.Impulse);
    }

    private void GetHit()
    {
        ResetBoolean();
        FaceDirection(mFacingDirection);
        mJumping = false;
        mGetHit = true;
    }

    private void GetKnockdown()
    {
        ResetBoolean();
        FaceDirection(mFacingDirection);
        mGetKnockdown = true;
    }

    private void Defend()
    {
        mDefending = true;
        FaceDirection(mFacingDirection);
    }

    private void MovingLeft()
    {
        transform.Translate(-Vector2.right * mMoveSpeed * Time.deltaTime);
        FaceDirection(-Vector2.right);
        mMoving = true;
        mRunning = true;
        //mWalking = true;
    }

    private void MovingRight()
    {
        transform.Translate(Vector2.right * mMoveSpeed * Time.deltaTime);
        FaceDirection(Vector2.right);
        mMoving = true;
        mRunning = true;
        //mWalking = true;
    }

    private void MovingUp()
    {
        transform.Translate(Vector2.up * mMoveSpeed * Time.deltaTime);
        mMoving = true;
        mRunning = true;
        //mWalking = true;
    }

    private void MovingDown()
    {
        transform.Translate(Vector2.down * mMoveSpeed * Time.deltaTime);
        mMoving = true;
        mRunning = true;
        //mWalking = true;
    }

    private void FaceDirection(Vector2 direction)
    {
        mFacingDirection = direction;
        if (direction == Vector2.right)
        {
            Vector3 newScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            transform.localScale = newScale;
        }
        else
        {
            Vector3 newScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            transform.localScale = newScale;
        }
    }

    private void ResetBoolean()
    {
        mStrongAttack = 0;
        mMoving = false;
        mRunning = false;
        mDefending = false;
        mWalking = false;
        mGetKnockdown = false;
        mDashing = false;
    }

    private void UpdateAnimator()
    {
        mAnimator.SetBool("isMoving", mMoving);
        mAnimator.SetBool("isRunning", mRunning);
        mAnimator.SetBool("isWalking", mWalking);
        mAnimator.SetBool("isDefending", mDefending);
        mAnimator.SetBool("isJumping", mJumping);
        mAnimator.SetBool("isFalling", mFalling);
        mAnimator.SetBool("isHit", mGetHit);
        mAnimator.SetBool("isKnockdown", mGetKnockdown);
        mAnimator.SetInteger("isHitting", mNormalAttack % 6);
        mAnimator.SetInteger("isStrongHitting", mStrongAttack);
        mAnimator.SetBool("isHittingBool", mHitting);
        mAnimator.SetBool("isSliding", mSliding);
        mAnimator.SetBool("isDashing", mDashing);
    }

    private void UpdateOrderInLayer()
    {
        mSpriteRenderer.sortingOrder = mInitialOrderInLayer - (int)(transform.position.y);
    }
}
