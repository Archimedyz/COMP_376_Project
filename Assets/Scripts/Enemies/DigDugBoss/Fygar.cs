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
	private bool floorBoundaryInitialized;

	private AudioSource pop;
	private AudioSource strongHit;
	private AudioSource normalHit;
	private float audioTimer = 0.0f;

	void Start ()
	{
		
		mAnimator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		mTarget = GameObject.Find ("Player").transform;

		// Init Floor stuff
		mFloorControllerRef = FindObjectOfType<FloorController> ();
		mFloorBoundary = new float[4];
		mSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer> ();
		floorBoundaryInitialized = false;

		AudioSource[] audioSources = GetComponents<AudioSource> ();
		pop = audioSources [0];
		normalHit = audioSources [1];
		strongHit = audioSources [2];
	}

	void Update ()
	{
		ResetBoolean ();

		if (!floorBoundaryInitialized) {
			// get current boundary
			mFloorControllerRef.GetCurrentFloorBoundary (mFloorBoundary, mFloorIndex, mSpriteRenderer);
			floorBoundaryInitialized = true;
		}

		if (mExplode) {
			destroyTimer += Time.deltaTime;
			if (destroyTimer >= 0.3f) {
				Destroy (gameObject);
			}
		}

		if (transform.position.x >= 50) {
			Destroy (gameObject);
		}
		
		if (canMove) {

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
		audioTimer += Time.deltaTime;
	}

	//TODO change damage
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == "FightCollider" && canMove) {
			if (!mDead) {
				Life -= 50;
				if (audioTimer >= 0.2f) {
					normalHit.Play ();
					audioTimer = 0.0f;
				}
			} else if (GameObject.Find ("Player").GetComponent<Player> ().IsStrongAttack ()) {
				if (!strongHit.isPlaying)
					strongHit.Play ();
				rb.isKinematic = false;
				rb.AddForce (Vector2.right * 10, ForceMode.Impulse);
			}
		} else if (col.gameObject.name == "DigDug") {
			pop.Play ();
			rb.isKinematic = true;
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
	
	public float GetFloorBoundaryY ()
	{
		return mFloorBoundary [Floor.Y_MAX_INDEX];
	}

	public void SetSpeed (float speed)
	{
		mVertiMoveSpeed = speed;
	}
}
