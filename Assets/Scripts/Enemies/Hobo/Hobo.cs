using UnityEngine;
using System.Collections;

public class Hobo : MonoBehaviour
{
	private bool mMoving;
	private bool mHitting;
	private bool mDying;

	private bool mGetHit;

	private Vector2 mFacingDirection;

	public float mHoriMoveSpeed;
	public float mVertiMoveSpeed;

	private Transform mTarget;
	public float mFollowRange;
	public float mFollowSpeed;
	public float mAttackDistance;

	private Animator mAnimator;
	private Rigidbody mRigidBody;

	public float mPushBack;
	private float mInvincibleTimer = 0.0f;
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

	AudioSource strongHit;
	AudioSource normalHit;
	AudioSource knifeHit;

	float audioTimer = 0.0f;

	public Stats mStats;

	UICanvas uiCanvas;
	private Vector3 damagePositionOffset = new Vector3 (0, 0.7f, -1);

	public int expGiven = 15;

	private float staggerTimer = 0f;

	private float hittingTimer = 0.0f;

	void Start ()
	{
		mStats = new Stats (1, 60, 15, 2, 0, new int[] { 20, 4, 2, 0 });
		mRigidBody = GetComponent<Rigidbody> ();
		mAnimator = GetComponent<Animator> ();
		mFacingDirection = Vector2.right;
		mDying = false;

		mTarget = GameObject.Find ("Player").transform;
		
		//expGiven = 15 - 2 * (GameObject.Find ("Player").GetComponent<Player> ().mStats.Level - mStats.Level);

		mFloorControllerRef = FindObjectOfType<FloorController> ();
		mFloorBoundary = new float[4];
		mSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer> ();
		mInitialOrderInLayer = (int)(transform.position.y);
		floorBoundaryInitialized = false;

		AudioSource[] audioSources = GetComponents<AudioSource> ();
		normalHit = audioSources [0];
		strongHit = audioSources [1];
		knifeHit = audioSources [2];

		uiCanvas = (UICanvas)GameObject.FindGameObjectWithTag ("UICanvas").GetComponent<UICanvas> ();
	}

	void Update ()
	{
		if (!floorBoundaryInitialized) {
			// get current boundary
			mFloorControllerRef.GetCurrentFloorBoundary (mFloorBoundary, mFloorIndex, mSpriteRenderer);
			floorBoundaryInitialized = true;
		}
		
		ResetBoolean ();
		if (staggerTimer > 0f)
			staggerTimer -= 0.6f;
		if (attackTimer > attackTimeWait && !mGetHit && staggerTimer <= 0 && !mDying && mFloorIndex == mTarget.gameObject.GetComponent<Player> ().GetLayerIndex ()) {
			if (Vector2.Distance (transform.position, mTarget.position) <= (mAttackDistance + 0.05)) {
				attackTimer = 0;
				knifeHit.Play ();
				Hit ();
			} else if (Vector2.Distance (transform.position, mTarget.position) <= mFollowRange && Vector2.Distance (transform.position, mTarget.position) > mAttackDistance) {
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

		if (mStats.isDead () && !mDying) {
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
				mInvincibleTimer = 0.0f;
			}
		}

		if (mHitting) {
			hittingTimer += Time.deltaTime;
			if (hittingTimer >= 0.5f) {
				hittingTimer = 0.0f;
				mHitting = false;
			}
		}
		audioTimer += Time.deltaTime;
	}

	private void Die ()
	{
		mDying = true;
	}

	public void GetHit (Vector2 direction, int damage, bool isCrit)
	{
		if (!mGetHit && !mDying) {
			StartCoroutine (Stagger ());
			mRigidBody.isKinematic = false;
			mGetHit = true;
			mRigidBody.velocity = Vector2.zero;
            
			if (GameObject.Find ("Player").GetComponent<Player> ().IsStrongAttack ()) {
				staggerTimer = 25f - (staggerTimer * 0.10f);
				damage = (int)(damage * 1.5f);
				Recoil (direction, 2f);
				if (!strongHit.isPlaying)
					strongHit.Play ();
			} else if (GameObject.Find ("Player").GetComponent<Player> ().IsDashing ()) {
				staggerTimer = 28f - (staggerTimer * 0.10f);
				damage = (int)(damage * 0.35f);
				Recoil (direction, 2f);
				if (audioTimer >= 0.2f) {
					normalHit.Play ();
					audioTimer = 0.0f;
				}
			} else {
				staggerTimer = 15 - (staggerTimer * 0.10f);
				Recoil (direction, mPushBack);
				if (audioTimer >= 0.2f) {
					normalHit.Play ();
					audioTimer = 0.0f;
				}
			}
			mStats.TakeDamage (damage);
			if (isCrit) {
				uiCanvas.CreateDamageLabel (((int)mStats.DamageDealt (damage)).ToString (), (transform.position + damagePositionOffset), UINotification.TYPE.CRIT);
			} else {
				uiCanvas.CreateDamageLabel (((int)mStats.DamageDealt (damage)).ToString (), (transform.position + damagePositionOffset), UINotification.TYPE.HPLOSS);
			}
		}
	}

	private IEnumerator Stagger ()
	{
		bool staggerRight = true;
		float initialPosX = transform.position.x;

		for (float timer = 0f; timer < 2.0f; timer += Time.deltaTime) {
			if (transform.position.x >= (initialPosX + 0.2f)) {
				staggerRight = false;
			} else if (transform.position.x <= (initialPosX - 0.2f)) {
				staggerRight = true;
			}

			if (staggerRight) {
				transform.position += new Vector3 (0.1f, 0f, 0f);
			} else {
				transform.position -= new Vector3 (0.1f, 0f, 0f);
			}
		}
		yield return new WaitForSeconds (0.001f);
	}

	public void Recoil (Vector2 direction, float modifier)
	{
		mRigidBody.AddForce (new Vector2 (direction.x, 0.0f) * modifier, ForceMode.Impulse);
	}

	private void Hit ()
	{
		attackTimer = 0;
		mHitting = true;
	}
	
	private void MovingLeft ()
	{
		transform.Translate (-Vector2.right * mHoriMoveSpeed * Time.deltaTime);
		FaceDirection (Vector2.left);
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
		//mHitting = false;
	}

	private void UpdateAnimator ()
	{
		mAnimator.SetBool ("isMoving", mMoving);
		mAnimator.SetBool ("isHitting", mHitting);
		mAnimator.SetBool ("isDying", mDying);
	}

	public Vector2 GetFacingDirection ()
	{
		return mFacingDirection;
	}

	public void LevelUp ()
	{
		mStats.Exp = 100;
	}

	void OnDestroy ()
	{
		if (GameObject.Find ("Player") != null)
			GameObject.Find ("Player").GetComponent<Player> ().noDamageStreak++;
	}

	// prevent them from entering a wall. 
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Wall") {
			mRigidBody.isKinematic = false; 
		}

		if (other.gameObject.name == "GeneralColliderPlayer" && GetFacingDirection () != mTarget.gameObject.GetComponent<Player> ().GetFacingDirection () && mHitting) {
			if (Input.GetKey (KeyCode.N) || Input.GetKey (KeyCode.V)) {
				if (GetFacingDirection () == Vector2.right)
					transform.position -= new Vector3 (0.5f, 0.0f, 0.0f);
				else
					transform.position += new Vector3 (0.5f, 0.0f, 0.0f);
			}
		}
	}
	void OnTriggerExit (Collider other)
	{
		if (other.gameObject.tag == "Wall") {
			mRigidBody.isKinematic = true; 
		}
	}

	public void SetFloorIndex (int index)
	{
		mFloorIndex = index;
	}
}
