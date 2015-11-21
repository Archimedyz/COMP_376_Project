using UnityEngine;
using System.Collections;

public class Fygar : MonoBehaviour
{
	public float Life;

	public GameObject FirePrefab;

	private bool mMoving;
	private bool mBreathFire = false;
	private bool mDead = false;
	private bool mExplode = false;
	
	private bool moveDown = true, moveUp = false;

	private Animator mAnimator;
	private Rigidbody rb;

	public float mVertiMoveSpeed;

	private Transform mTarget;

	private Vector2 mFacingDirection;

	private float destroyTimer = 0.0f;
	private float fireTimer = 0.0f;
	private float nextFire = 5.0f;
	private float timer = 0.0f;
	
	public Vector3 initialPosition;

	private bool canMove = false;

	// Init Floor stuff
	private FloorController mFloorControllerRef;
	public int mFloorIndex;
	public float[] mFloorBoundary;
	private SpriteRenderer mSpriteRenderer;
	private int mInitialOrderInLayer;
	private bool floorBoundaryInitialized;

	void Start ()
	{
		
		mAnimator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		mTarget = GameObject.Find ("Player").transform;

		// Init Floor stuff
		mFloorControllerRef = FindObjectOfType<FloorController> ();
		mFloorBoundary = new float[4];
		mSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer> ();
		mInitialOrderInLayer = (int)(transform.position.y);
		floorBoundaryInitialized = false;
	}

	void Update ()
	{
		ResetBoolean ();

		if (!floorBoundaryInitialized) {
			// get current boundary
			mFloorControllerRef.GetCurrentFloorBoundary (mFloorBoundary, mFloorIndex, mSpriteRenderer);
			floorBoundaryInitialized = true;
		}
		
		if (canMove) {
			if (mExplode) {
				destroyTimer += Time.deltaTime;
				if (destroyTimer >= 0.3f) {
					Destroy (gameObject);
				}
			}

			if (Life <= 0) {
				mDead = true;
			} else {
				timer += Time.deltaTime;
				int a = Random.Range (0, 20);
				if (timer > nextFire && a == 0) {
					BreathFire ();
				}
				if (!mBreathFire) {
					if (transform.position.y - 0.1f <= mFloorBoundary [Floor.Y_MIN_INDEX]) {
						moveUp = true;
						moveDown = false;
					} else if (transform.position.y + 0.1f >= mFloorBoundary [Floor.Y_MAX_INDEX]) {
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
			} 
		} else {
			MovingUp ();
		}

		if (mBreathFire) {
			fireTimer += Time.deltaTime;
			if (fireTimer >= 1.5f) {
				mBreathFire = false;
			}
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



	private void BreathFire ()
	{
		mBreathFire = true;
		timer = 0.0f;
		Instantiate (FirePrefab, new Vector3 (transform.position.x - 1.0f, transform.position.y, transform.position.z), Quaternion.identity);
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
		mAnimator.SetBool ("isFire", mBreathFire);
		mAnimator.SetBool ("isDead", mDead);
		mAnimator.SetBool ("isExploding", mExplode);
	}

	public void SetLife (int life)
	{
		Life = life;
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
