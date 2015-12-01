using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	private Text levelText;

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
	private bool mDying = false;
	private float mGroundY;
	private bool moveRight = false;
	private bool moveUp = false;

	private bool dead = false;
	private float dyingTimer = 0.0f;

	private bool canMove = true;
	private bool canWalk = false;
	private float mWalkSpeed = 10.0f;

	private bool mInflate;
	private float inflateTimer = 0.0f;
	private float maxInflateTimer = 2.0f;

	private bool mHitting;
	private float hitWait = 0.005f;
	private float hitTimer;

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

	private bool inStory = false;

	private float target;
	private float startTime;
	private float journeyLength;

	private UICanvas uiCanvas;
	private Vector3 damagePosition = new Vector3 (0, 0.7f, 0);
	private Vector3 expPosition = new Vector3 (0.1f, 0.7f, 0);

	// Shadow GameObject
	GameObject mShadow;

	private AudioSource deflating;
	private AudioSource running;
	private AudioSource health;

	public int timerForSlide;
	[SerializeField]
	private float
		DashSpeedModifier = 1.15f;
	private int dashTime = 0;
	private int dashRecovery = 10;
	private bool mDashEnd = false;

	public int noDamageStreak = 0;
	private int maxStreak = 0;


	void Start ()
	{
		mAnimator = GetComponent<Animator> ();
		mRigidBody = GetComponent<Rigidbody> ();
		InitStats (1, 50, 10, 5, 5);
		mFacingDirection = Vector2.right;
		mJumping = false;
		mSliding = false;
		mDashing = false;
		mNormalAttack = -1;
		mStrongAttack = 0;
		mGroundY = transform.position.y;

		mMoveSpeedX = 4.0f;
		mMoveSpeedY = 2.5f;
		mJumpForce = 6.5f;
		mGravityScale = 0.8f;
		hitTimer = hitWait;

		// Init Floor stuff
		mFloorControllerRef = FindObjectOfType<FloorController> ();
		mFloorBoundary = new float[4];
		mSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer> ();
		mInitialOrderInLayer = (int)(transform.position.y);
		floorBoundaryInitialized = false;

		// before using the healthbar, check if stats need to be set:
		if (PlayerPrefs.GetInt ("set_player_stats") == 1) {
			GameObject.FindGameObjectWithTag ("GameController").SendMessage ("SetStats", mStats);
		}

		// Init HealthBar Stuff
		mHealthBarRef = GameObject.FindGameObjectWithTag ("PlayerHealth").GetComponent<HealthBar> ();
		mHealthBarRef.SetMaxHealth (mStats.MaxHp);
		mHealthBarRef.SetHealth (mStats.Hp);

		//AudioSource[] audioSources = GetComponents<AudioSource> ();
		uiCanvas = (UICanvas)GameObject.FindGameObjectWithTag ("UICanvas").GetComponent<UICanvas> ();

		// get the shadow. it's the last element in the childern.
		SpriteRenderer[] childSpriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer> ();
		mShadow = childSpriteRenderers [childSpriteRenderers.Length - 1].gameObject;

		//Audio init
		AudioSource[] audioSources = GetComponents<AudioSource> ();
		deflating = audioSources [0];
		running = audioSources [1];
		health = audioSources [2];

		timerForSlide = 0;

		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
		if (mStats.Level <= 1) {
			levelText.text = "F";
			levelText.color = new Color (1, 0, 0);
		} else {
			SetLevelLabel ();
		}
	}
	/// <summary>
	/// Main flow of the player
	/// </summary>
	void FixedUpdate ()
	{
		if (!dead && mHealthBarRef.GetHealth () <= 0) {
			Die ();
		}

		if (mDying) {
			dyingTimer += Time.deltaTime;
			if (dyingTimer >= 1.45f) {
				mDying = false;
				dyingTimer = 0f;
			}
		}

		if (mNormalAttack >= 0 && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
			mNormalAttack = -1;
			mHitting = false;
		}

		if (!floorBoundaryInitialized) {
			// get current boundary
			mFloorControllerRef.GetCurrentFloorBoundary (mFloorBoundary, mFloorIndex, mSpriteRenderer);
			floorBoundaryInitialized = true;
		}
		if (mInflate) {
			canMove = false;
			inflateTimer += Time.deltaTime;
			if (inflateTimer >= maxInflateTimer) {
				if (transform.position.x > (target + 0.1f)) {
					if (!deflating.isPlaying)
						deflating.Play ();
					float distCovered = (Time.time - startTime) * 0.5f;
					float fracJourney = distCovered / journeyLength;
					transform.position = Vector3.Lerp (transform.position, new Vector3 (target, transform.position.y, transform.position.z), fracJourney);
				} else {
					deflating.Stop ();
					mInflate = false;
					inflateTimer = 0.0f;
				}
			}
		}
		//Teleport back up
		if (transform.position.y <= mGroundY && mJumping) {
			mRigidBody.useGravity = false;
			mRigidBody.velocity = new Vector3 (0, 0, 0);
			transform.position = new Vector3 (transform.position.x, mGroundY, transform.position.z);
			mJumping = false;
		}

		//get out of jumping?! / slide / Dash / Handle dash recovery
		if (mJumping && mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Idle") && Mathf.Approximately (transform.position.y, mGroundY)) {
			mJumping = false;
		} else if (mSliding && !mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Sliding")) {
			mSliding = false;
		} else if (mDashing && dashTime > 35) {
			mDashing = false;
			dashRecovery = 0;
		}
		if (dashRecovery < 10 && !mDashing)
			dashRecovery++;

		//Instruction priority
		if (inStory) {
			ResetBoolean ();
			if (moveRight) {
				MovingRight ();
			} else if (moveUp) {
				MovingUp ();
			}
			if (canWalk) {
				if (Input.GetButton ("Left")) {
					WalkLeft ();
				} else if (Input.GetButton ("Right")) {
					WalkRight ();
				}
				if (Input.GetButton ("Up")) {
					WalkUp ();
				} else if (Input.GetButton ("Down")) {
					WalkDown ();
				}
			}
			transform.localPosition = new Vector3 (
                Mathf.Clamp (transform.localPosition.x, mFloorBoundary [Floor.X_MIN_INDEX], mFloorBoundary [Floor.X_MAX_INDEX]),
                Mathf.Clamp (transform.localPosition.y, mFloorBoundary [Floor.Y_MIN_INDEX], mFloorBoundary [Floor.Y_MAX_INDEX]),
                 transform.localPosition.z
			);
			mGroundY = transform.position.y;
		} else if (mDashing) {
			ResetBoolean ();
			DashHandler ();
		} else if (mJumping) {
			ResetBoolean ();
			//Jump attack
			MovementHandler ();
		} else {
			transform.position = new Vector3 (transform.position.x, transform.position.y, -1f);
			transform.rotation = Quaternion.Euler (0f, 0f, 0f);
			if (!ActionHandler ())
			if (CanMove ()) {
				bool isJumping = mJumping;
				ResetBoolean ();
				MovementHandler ();
				JumpHandler ();
			}
		}

		if (mRunning)
			timerForSlide++;
		else
			timerForSlide = 0;

		mShadow.transform.position = new Vector3 (transform.position.x, (mGroundY - mSpriteRenderer.bounds.size.y / 2), transform.position.z);

		CheckFalling ();

		// if one is not jumping or falling, then they must be on the floor, meaning they must abide by the boundaries.
		if (!mJumping && !mFalling) {
			transform.position = new Vector3 (Mathf.Clamp (transform.position.x, mFloorBoundary [Floor.X_MIN_INDEX], mFloorBoundary [Floor.X_MAX_INDEX]), Mathf.Clamp (transform.position.y, mFloorBoundary [Floor.Y_MIN_INDEX], mFloorBoundary [Floor.Y_MAX_INDEX]), transform.position.z);
		}
		UpdateOrderInLayer ();

		if (mGetHit) {
			mInvincibleTimer += Time.deltaTime;
			if (mInvincibleTimer >= kInvincibilityDuration) {
				mGetHit = false;
				mInvincibleTimer = 0.0f;
			}
		}

		if (mGetKnockdown) {
			mInvincibleTimer += Time.deltaTime;
			if (mInvincibleTimer >= kKnockdownInvincibilityDuration) {
				mGetKnockdown = false;
				mInvincibleTimer = 0.0f;
			}
		}
		hitTimer += Time.deltaTime;
		UpdateAnimator ();
	}

	private void MovementHandler ()
	{
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		if (!Mathf.Approximately (vertical, 0.0f) || !Mathf.Approximately (horizontal, 0.0f)) {
			Vector3 direction = new Vector3 (horizontal, vertical, 0.0f);
			direction = Vector3.ClampMagnitude (direction, 1.0f);
			if (direction.x > 0.00f) {
				FaceDirection (Vector2.right);
			} else {
				FaceDirection (Vector2.left);
			}

			direction.x = direction.x * mMoveSpeedX;
			direction.y = direction.y * mMoveSpeedY;

			//Jump Check
			if (mJumping) {
				direction.x /= 1.2f;
				direction.y /= 2.0f;
				mGroundY += direction.y * Time.deltaTime;
				direction.y = 0;
			}
			transform.Translate (direction * Time.deltaTime, Space.World);
			if (!mJumping) {
				mGroundY = transform.position.y;
			}
			mMoving = true;
			mWalking = inStory;
			mRunning = (!inStory);

			// if u pass the bottom of the floor boundary 
			if (!mJumping && mGroundY < mFloorBoundary [Floor.Y_MIN_INDEX] && Input.GetKey (KeyCode.Space)) {
				int newFloorIndex = mFloorControllerRef.NextFloorDown (mFloorIndex);
				if (newFloorIndex != mFloorIndex) {
					mFloorIndex = newFloorIndex;
					mFloorControllerRef.GetCurrentFloorBoundary (mFloorBoundary, mFloorIndex, mSpriteRenderer);
					mJumping = true;
					mFalling = true;
					mGroundY = mFloorBoundary [Floor.Y_MAX_INDEX];
					mRigidBody.useGravity = true;
				}
			} else if (!mJumping && mGroundY > mFloorBoundary [Floor.Y_MAX_INDEX] && Input.GetKey (KeyCode.Space)) {
				int newFloorIndex = mFloorControllerRef.NextFloorUp (mFloorIndex);
				if (newFloorIndex != mFloorIndex) {
					// check if the player has even reached the next level in terms of animation.
					float[] newFloorBoundary = new float[4];
					mFloorControllerRef.GetCurrentFloorBoundary (newFloorBoundary, newFloorIndex, mSpriteRenderer);

					//if (transform.position.y > newFloorBoundary [Floor.Y_MIN_INDEX]) {
					mFloorIndex = newFloorIndex;
					mFloorBoundary = newFloorBoundary;
					mGroundY = mFloorBoundary [Floor.Y_MIN_INDEX];
					mJumping = true;
					//}
				}
			}

			mGroundY = Mathf.Clamp (mGroundY, mFloorBoundary [Floor.Y_MIN_INDEX], mFloorBoundary [Floor.Y_MAX_INDEX]);
		}
	}

	private bool ActionHandler ()
	{
		if (Input.GetKey (KeyCode.V)) {
			Defend ();
			mMoving = false;
			mRunning = false;
			return true;
		} else {
			//Attack
			if (Input.GetKeyDown (KeyCode.Z) && hitTimer >= hitWait && timerForSlide < 15) {
				hitTimer = 0.0f;
				dashRecovery = -15;
				mNormalAttack++;
				mAnimator.SetInteger ("isHitting", mNormalAttack % 6);
				if (mNormalAttack > -1)
					mHitting = true;
				else
					mHitting = false;
				return true;
			} else if (Input.GetKeyDown (KeyCode.Z) && timerForSlide > 15) {
				Slide ();
				dashRecovery = -30;
				timerForSlide = 0;
				return true;
			} else if (Input.GetKey (KeyCode.X)) {
				mStrongAttack++;
				mMoving = false;
				mRunning = false;
				return true;
			} else if (Input.GetKeyDown (KeyCode.C) && !mDashing && dashRecovery == 10) {//DashDuration
				mDashing = true;
				dashTime = 0;
				transform.Translate (GetFacingDirection () * mMoveSpeedX * DashSpeedModifier * Time.deltaTime, Space.World);
				return true;
			}
			return false;
		}
	}

	private bool JumpHandler ()
	{
		//Change side
		if (Input.GetKeyDown (KeyCode.Space) && Mathf.Approximately (mRigidBody.velocity.y, 0.0f) && !mJumping) {
			mGroundY = transform.position.y;
			Jump ();
		}
		return mJumping;
	}

	private void DashHandler ()
	{
		mDashing = true;
		dashTime++;
		transform.Translate (GetFacingDirection () * mMoveSpeedX * DashSpeedModifier * Time.deltaTime, Space.World);
	}

	public void PlayRunSound ()
	{
		if (mRunning && !mJumping) {
			if (!running.isPlaying) {
				running.Play ();
			}
		} else {
			if (running.isPlaying) {
				running.Stop ();
			}
		}
	}

	public void SetCanWalk (bool a)
	{
		canWalk = a;
	}

	public void WalkRight ()
	{
		transform.Translate (Vector2.right * mWalkSpeed * Time.deltaTime);
		FaceDirection (Vector2.right);
		mMoving = true;
		mWalking = true;
	}

	public void WalkLeft ()
	{
		transform.Translate (-Vector2.right * mWalkSpeed * Time.deltaTime);
		FaceDirection (-Vector2.right);
		mMoving = true;
		mWalking = true;
	}

	public void WalkUp ()
	{
		transform.Translate (Vector2.up * mWalkSpeed * Time.deltaTime);
		mMoving = true;
		mWalking = true;
	}

	public void WalkDown ()
	{
		transform.Translate (Vector2.down * mWalkSpeed * Time.deltaTime);
		mMoving = true;
		mWalking = true;
	}

	public void SetMoveRight (bool a)
	{
		moveRight = a;
		mRunning = a;
	}

	public void SetMoveUp (bool a)
	{
		moveUp = a;
		mWalking = a;
	}

	private void MovingRight ()
	{
		transform.position += new Vector3 (0.075f, 0f, 0f);
		FaceDirection (Vector2.right);
		mMoving = true;
		mRunning = true;
	}

	private void MovingUp ()
	{
		transform.position += new Vector3 (0f, 0.01f, 0f);
		FaceDirection (Vector2.right);
		mMoving = true;
		mWalking = true;
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

	private void Die ()
	{
		StartCoroutine (GoToMainMenu ());
		Time.timeScale = 0.3f;
		dead = true;
		mDying = true;
	}

	private IEnumerator GoToMainMenu ()
	{
		yield return new WaitForSeconds (3.0f);
		GameObject.Find ("GameMaster").SendMessage ("MainMenu");
	}

	public void GetHit (Vector2 direction, int damage)
	{
		if (!mGetHit && !mGetKnockdown && !mInflate) {
			mGetHit = true;
			maxStreak = getNoDamageStreak ();
			noDamageStreak = 0;
			mStats.TakeDamage (damage);
			mHealthBarRef.SetHealth (mStats.Hp);
			uiCanvas.CreateDamageLabel (((int)mStats.DamageDealt (damage)).ToString (), (transform.position + damagePosition), UINotification.TYPE.HPLOSS);
			mRigidBody.isKinematic = false;
			mRigidBody.velocity = Vector3.zero;
			mRigidBody.AddForce (new Vector2 (direction.x, 0.0f) * mHitPushBack, ForceMode.Impulse);
		}
	}

	public void GetKnockdown (Vector2 direction, int damage)
	{
		if (!mGetHit && !mGetKnockdown && !mInflate) {
			mGetKnockdown = true;
			maxStreak = getNoDamageStreak ();
			noDamageStreak = 0;
			mStats.TakeDamage (damage);
			mHealthBarRef.SetHealth (mStats.Hp);
			uiCanvas.CreateDamageLabel (((int)mStats.DamageDealt (damage)).ToString (), (transform.position + damagePosition), UINotification.TYPE.HPLOSS);
			mRigidBody.isKinematic = false;
			mRigidBody.velocity = Vector3.zero;
			mRigidBody.AddForce (new Vector2 (-direction.x, 0.0f) * mKnockdownPushBack, ForceMode.Impulse);
			mJumping = true;
			mFalling = false;
		}
	}

	public void GetBlockDamage (int damage)
	{
		mStats.TakeDamage (damage);
		mHealthBarRef.SetHealth (mStats.Hp);
		uiCanvas.CreateDamageLabel (((int)mStats.DamageDealt (damage)).ToString (), (transform.position + damagePosition), UINotification.TYPE.HPLOSS);
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
		bool cond4 = !mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Dying");
		bool cond5 = !mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("Dead");
		return cond1 && cond2 & cond3 && cond4 && cond5 && canMove;
	}

	private void ResetBoolean ()
	{
		mStrongAttack = 0;
		mMoving = false;
		mRunning = false;
		mDefending = false;
		mWalking = false;
		mDashing = false;
		mHitting = false;
		mDashEnd = false;
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
		mAnimator.SetInteger ("isStrongHitting", mStrongAttack);
		mAnimator.SetBool ("isSliding", mSliding);
		mAnimator.SetBool ("isDashing", mDashing);
		mAnimator.SetBool ("isDashEnd", mDashEnd);
		mAnimator.SetBool ("isInflating", mInflate);
		mAnimator.SetBool ("isHittingBool", mHitting);
		mAnimator.SetBool ("isDying", mDying);
		mAnimator.SetBool ("Dead", dead);
	}

	private void CheckFalling ()
	{
		mFalling = (mRigidBody.velocity.y < 0.0f);
	}

	private void UpdateOrderInLayer ()
	{
		mSpriteRenderer.sortingOrder = mInitialOrderInLayer - (int)(mGroundY);
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

	public void AddExperience (int exp)
	{
		if (mStats.Level < 8) {
			mStats.Exp += exp;
			uiCanvas.CreateDamageLabel (exp.ToString () + "exp", (transform.position + damagePosition), UINotification.TYPE.EXP);
			if (mStats.IsLevelUp) {
				uiCanvas.CreateDamageLabel ("LEVEL UP!", (transform.position + damagePosition / 1.5f), UINotification.TYPE.LVLUP);
				SetLevelLabel ();
				mStats.IsLevelUp = false;
				mHealthBarRef.SetMaxHealth (mStats.MaxHp);
				SpeedStatIncreases ();
			}
		}
	}

	public void SetLevelLabel ()
	{
		switch (mStats.Level) {
		case 2:
			levelText.text = "E";
			levelText.color = new Color (1f, 0.25f, 0f);
			break;
		case 3:
			levelText.text = "D";
			levelText.color = new Color (1f, 0.40f, 0f);
			break;
		case 4:
			levelText.text = "C";
			levelText.color = new Color (1f, 1f, 0f);
			break;
		case 5:
			levelText.text = "B";
			levelText.color = new Color (0.5f, 1f, 0f);
			break;
		case 6:
			levelText.text = "A";
			levelText.color = new Color (0f, 1f, 0f);
			break;
		}
	}

	/// <summary>
	/// Initializes player stats
	/// </summary>
	/// <param name="lvl"></param>
	/// <param name="hp"></param>
	/// <param name="str"></param>
	/// <param name="def"></param>
	/// <param name="speed"></param>
	public void InitStats (int lvl, float hp, int str, int def, int speed)
	{
		mStats = new Stats (lvl, hp, str, def, speed, Stats.playerRate);
		SpeedStatIncreases ();
	}

	private void SpeedStatIncreases ()
	{
		mMoveSpeedX = 4 + mStats.Spd * 0.1f;
		mMoveSpeedX = 3 + mStats.Spd * 0.1f;
		mAnimator.speed = 1 + (mStats.Spd / 50.0f);
	}

	public bool IsStrongAttack ()
	{
		return mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("StrongAttackPhase2");
	}

	public bool IsDefending ()
	{
		return mAnimator.GetCurrentAnimatorStateInfo (0).IsName ("DefendingPhase2");
	}

	public bool IsDashing ()
	{
		return mDashing;
	}

	public int getNoDamageStreak ()
	{
		if (noDamageStreak > maxStreak)
			return noDamageStreak;
		else
			return maxStreak;
	}

	public float GetFootY ()
	{
		return mGroundY - (mSpriteRenderer.bounds.size.y / 2.0f);
	}

	public void SetInStory (bool a)
	{
		inStory = a;
	}

	public void GetHealth (int a)
	{
		health.Play ();
		mHealthBarRef.GainHealth (a);
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Hose" && !mInflate) {
			mInflate = true;
			mRunning = false;
			if (inStory)
				target = -6;
			else
				target = -5;
			startTime = Time.time;
			journeyLength = Mathf.Abs (transform.position.x) + Mathf.Abs (target);
		}
	}
	void OnCollisionEnter (Collision col)
	{
		if (mDashing) {
			if (col.gameObject.name.ToLower ().StartsWith ("hobo")) {
				//Stun
				(col.gameObject.GetComponent<Hobo> ()).GetHit (Vector2.zero, mStats.DoDynamicDamage (), mStats.wasCrit);
				mDashing = false;
				mDashEnd = true;
				dashRecovery = -60;
			} else {
				if (mDashing && col.gameObject.name.ToLower ().StartsWith ("neanderthal")) {
					//Stun
					(col.gameObject.GetComponent<Neanderthal> ()).GetHit (Vector2.zero, mStats.DoDynamicDamage (), mStats.wasCrit);
					mDashing = false;
					mDashEnd = true;
					dashRecovery = -60;
				}
			}
		}
	}
}
