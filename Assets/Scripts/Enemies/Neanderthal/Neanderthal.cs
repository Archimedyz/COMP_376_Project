using UnityEngine;
using System.Collections;

public class Neanderthal : MonoBehaviour
{
	private bool mMoving;
	private bool mThrowing;
	private bool mDying = false;
	private bool mGetHit;
	private bool mAppearing;
	private float appearingTimer = 0.0f;

	public float mHoriMoveSpeed;
	public float mVertiMoveSpeed;

	private Animator mAnimator;
	private Rigidbody mRigidBody;

	public GameObject projectile;
	private GameObject coconut;

	private Transform mTarget;
	public float mFollowRange;

	public float mPushBack;
	private float mInvincibleTimer;
	private float kInvincibilityDuration = 0.1f;

	public float mAttackDistance;

	public float attackTimeWait;
	private float attackTimer = 0.0f;

	private float dyingTimer = 0.0f;

	// Floor Variables - START

	private FloorController mFloorControllerRef;
	public int mFloorIndex;
	public float[] mFloorBoundary;
	private int mInitialOrderInLayer;
	private bool floorBoundaryInitialized;

	// Floor Variables - END

	AudioSource strongHit;
	AudioSource normalHit;

	float audioTimer = 0.0f;

	private UICanvas uiCanvas;
	private Vector3 damagePositionOffset = new Vector3 (0, 0.9f, -1);

	public Stats mStats;

	public int expGiven = 10;

	void Awake ()
	{
		mAppearing = true;
	}

	void Start ()
	{
		mStats = new Stats (1, 7000, 18, 2, 0, new int[] { 20, 4, 2, 0 });
		mRigidBody = GetComponent<Rigidbody> ();
		mAnimator = GetComponent<Animator> ();
		coconut = null;

		mTarget = GameObject.Find ("Player").transform;

		mFloorControllerRef = FindObjectOfType<FloorController> ();
		mFloorBoundary = new float[4];
		mInitialOrderInLayer = (int)(transform.position.y);
		floorBoundaryInitialized = false;

		AudioSource[] audioSources = GetComponents<AudioSource> ();
		normalHit = audioSources [0];
		strongHit = audioSources [1];

		uiCanvas = (UICanvas)GameObject.FindGameObjectWithTag ("UICanvas").GetComponent<UICanvas> ();
	}

	void Update ()
	{

		if (!floorBoundaryInitialized) {
			// get current boundary
			//mFloorControllerRef.GetCurrentFloorBoundary (mFloorBoundary, mFloorIndex, mSpriteRenderer);
			floorBoundaryInitialized = true;
		}

		ResetBoolean ();

		if (!mAppearing) {
			if (!mGetHit && !mDying && mFloorIndex == mTarget.gameObject.GetComponent<Player> ().GetLayerIndex ()) {
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
			}

			attackTimer += Time.deltaTime;

			if (mStats.isDead () && !mDying) {
				Die ();
			}

			if (mDying) {
				dyingTimer += Time.deltaTime;
				if (dyingTimer >= 1.5f) {
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
			audioTimer += Time.deltaTime;
		} else {
			appearingTimer += Time.deltaTime;
			if (appearingTimer >= 1.0f) {
				mAppearing = false;
				appearingTimer = 0.0f;
			}
		}

		UpdateAnimator ();
	}

	public void GetHit (Vector2 direction, int damage, bool isCrit)
	{
		if (!mGetHit && !mDying) {
			attackTimer = 0;
			mRigidBody.isKinematic = false;
			mGetHit = true;
			mRigidBody.velocity = Vector2.zero;
			if (GameObject.Find ("Player").GetComponent<Player> ().IsStrongAttack ()) {
				damage = (int)(damage * 1.3f);
				Recoil (direction, 2f);
				if (!strongHit.isPlaying)
					strongHit.Play ();
			} else {
				Recoil (direction, mPushBack);
				if (audioTimer >= 0.15f) {
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

	public void Recoil (Vector2 direction, float modifier)
	{
		mRigidBody.AddForce (new Vector2 (direction.x, 0.0f) * modifier, ForceMode.Impulse);
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
			if (mTarget.position.x >= transform.position.x) {
				coconut.gameObject.GetComponent<Coconut> ().SetDirection (Vector2.left);
			} else if (mTarget.position.x < transform.position.x) {
				coconut.gameObject.GetComponent<Coconut> ().SetDirection (Vector2.right);
			}
			coconut.transform.parent = gameObject.transform;
			coconut.gameObject.GetComponent<Coconut> ().damage = mStats.DoDynamicDamage ();
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
	}

	private void UpdateAnimator ()
	{
		mAnimator.SetBool ("isMoving", mMoving);
		mAnimator.SetBool ("isGettingHit", mGetHit);
		mAnimator.SetBool ("isThrowing", mThrowing);
		mAnimator.SetBool ("isDying", mDying);
		mAnimator.SetBool ("isAppearing", mAppearing);
	}

	public void LevelUp ()
	{
		mStats.Exp = 100;
	}
}
