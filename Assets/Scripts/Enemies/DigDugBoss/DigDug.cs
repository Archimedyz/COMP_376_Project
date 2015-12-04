using UnityEngine;
using System.Collections;

public class DigDug : MonoBehaviour
{
	public float randomAttackTime;

	private int hitTime = 0;

	private HealthBar mHealthBarRef;
	private float healthDivider;
	private float healthLost = 0.0f;

	public GameObject rock;
	public GameObject dig;
	public GameObject dug;
	public GameObject hose;
	private DigDugStory storyScript;

	private GameObject hoseInstance = null;

	private GameObject[] title;
	private bool throwTitle = false;
	private int nextThrow = 0;
	private bool finishThrowTitle = true;

	public float tileRangeX;
	public float tileRangeY;

	private Animator mAnimator;
	private SpriteRenderer sr;

	private bool mMoving;
	private bool mDead = false;
	private bool mThrowing = false;
	private bool mPumping = false;
	private bool mHit;
	private bool mThrowRocks;

	private bool canMove = false;

	private bool invincible = false;
	private float maxInvincibleTimer = 2.0f;
	private float invincibleTimer = 0.0f;

	private float hitTimer = 0.0f;
	private float throwRockTimer = 0.0f;

	private int maxLife = 3;

	public float mVertiMoveSpeed;

	private bool moveDown = true, moveUp = false;

	private int difficulty = 6;

	private Transform camTransform;
	public float shake = 2.0f;
	public float shakeAmount = 0.05f;
	
	Vector3 originalPos;

	private float pumpingTimer = 0.0f;
	private float maxPumpingTimer = 2.5f;

	private Transform player;

	// Floor Variables - START

	private FloorController mFloorControllerRef;
	public int mFloorIndex;
	public float[] mFloorBoundary;
	private SpriteRenderer mSpriteRenderer;
	private bool floorBoundaryInitialized;

	// Floor Variables - END

	private AudioSource pumping;

	bool inStory = true;

	void Awake ()
	{
		camTransform = GameObject.Find ("Main Camera").transform;
	}	

	void OnEnable ()
	{
		originalPos = camTransform.localPosition;
	}

	void Start ()
	{
		mAnimator = GetComponent<Animator> ();
		sr = transform.GetChild (0).GetComponent<SpriteRenderer> ();	
		title = new GameObject[6];
		player = GameObject.Find ("Player").transform;

		// Init Floor stuff
		mFloorControllerRef = FindObjectOfType<FloorController> ();
		mFloorBoundary = new float[4];
		mSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer> ();
		floorBoundaryInitialized = false;

		mHealthBarRef = GameObject.FindGameObjectWithTag ("BossHealth").GetComponent<HealthBar> ();
		mHealthBarRef.transform.position += new Vector3 (0f, 0f, -30f);
		storyScript = GameObject.Find ("BossScript").GetComponent<DigDugStory> ();

		healthDivider = mHealthBarRef.GetMaxHealth () / 4;

		AudioSource[] audioSources = GetComponents<AudioSource> ();
		pumping = audioSources [0];
	}

	void Update ()
	{

		if (!floorBoundaryInitialized) {
			// get current boundary
			mFloorControllerRef.GetCurrentFloorBoundary (mFloorBoundary, mFloorIndex, mSpriteRenderer);
			floorBoundaryInitialized = true;
		}

		ResetBoolean ();

		if (mHit) {
			hitTimer += Time.deltaTime;
			if (hitTimer >= 1.0f) {
				hitTimer = 0.0f;
				mHit = false;
			}
		}

			
		if (inStory) {
			if (mPumping) {
				pumpingTimer += Time.deltaTime;
				if (pumpingTimer >= maxPumpingTimer) {
					pumping.Stop ();
					mThrowing = false;
					mPumping = false;
					pumpingTimer = 0.0f;
					Destroy (hoseInstance);
				}
			}
		} else if (canMove && !mHit) {
			if (mPumping) {
				pumpingTimer += Time.deltaTime;
				if (pumpingTimer >= maxPumpingTimer) {
					pumping.Stop ();
					mThrowing = false;
					mPumping = false;
					pumpingTimer = 0.0f;
					Destroy (hoseInstance);
				}
			} else if (transform.position.x - player.position.x < 2.0f) {
				if (player.position.y > transform.position.y) {
					moveUp = true;
					moveDown = false;
				} else if (player.position.y < transform.position.y) {
					moveUp = false;
					moveDown = true;
				} else {
					moveUp = false;
					moveDown = false;
				}
				
				if (moveDown) {
					MovingDown (10);
				} else if (moveUp) {
					MovingUp (10);
				} 

				if (Mathf.Abs (player.position.y - transform.position.y) <= 0.5) {
					Pumping ();
				}
			} else {
				float specialAttack = Random.Range (0.0f, 100.0f);
				if (specialAttack > randomAttackTime) {
					int whichAttack = Random.Range (0, 2);
					if (whichAttack == 0 && !mHit && !mThrowRocks)
						mThrowRocks = true;
					else if (whichAttack == 1 && !mHit && finishThrowTitle)
						StartCoroutine (CreateTitle ());
				}
				if (mThrowRocks) {
					throwRockTimer += Time.deltaTime;
					originalPos = camTransform.localPosition;
					ShakeCamera ();
					if (throwRockTimer >= 2.0f) {
						throwRockTimer = 0.0f;
						mThrowRocks = false;
						StartCoroutine (ThrowTiles ());
					}
				} else {
					if (transform.position.y - 0.1f <= mFloorBoundary [Floor.Y_MIN_INDEX]) {
						moveUp = true;
						moveDown = false;
					} else if (transform.position.y + 0.1f >= mFloorBoundary [Floor.Y_MAX_INDEX]) {
						moveUp = false;
						moveDown = true;
					}
			
					if (moveDown) {
						MovingDown (1);
					} else if (moveUp) {
						MovingUp (1);
					}
				}
			}		
			if (throwTitle) {
				title [nextThrow].GetComponent<Title> ().SetLaunch ();
				throwTitle = false;
				if (nextThrow == 6) {
					finishThrowTitle = true;
				}
			}
			
			if (invincible) {
				invincibleTimer += Time.deltaTime;
				if (invincibleTimer > maxInvincibleTimer) {
					invincibleTimer = 0.0f;
					invincible = false;
				}
			}
		}
		UpdateAnimator ();
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Enemy" && !invincible) {
			ResetBoolean ();
			mHit = true;
			invincible = true;
			UpdateAnimator ();
			if (col.gameObject.name.Substring (0, 5) == "Pooka") {
				mHealthBarRef.LoseHealth (10);
				healthLost += 10;
			} else 	if (col.gameObject.name.Substring (0, 5) == "Fygar") {
				mHealthBarRef.LoseHealth (20);
				healthLost += 20;
			}

			if (mHealthBarRef.GetHealth () <= 0.0f) {
				Reset ();
				ResetBoolean ();
				mDead = true;
				storyScript.DigDugDie ();
			} else if (healthLost >= healthDivider) {
				healthLost = 0.0f;
				hitTime ++;
				storyScript.MoveDigDug (hitTime);
			}
		}
	}

	public void Pumping ()
	{
		pumping.Play ();
		hoseInstance = Instantiate (hose, transform.position, Quaternion.identity) as GameObject;
		mThrowing = true;
		mPumping = true;
		if (inStory)
			UpdateAnimator ();
	}

	private void ShakeCamera ()
	{
		if (shake > 0.0f) {
			camTransform.localPosition = originalPos + Random.insideUnitSphere / 10.0f * shakeAmount;
			
			shake -= Time.deltaTime;
		} else {
			shake = 2.0f;
			camTransform.localPosition = originalPos;
		}
	}

	private void MovingUp (int multiplier)
	{
		transform.Translate (new Vector2 (0.3f, 1.0f) * mVertiMoveSpeed * multiplier * Time.deltaTime);
		mMoving = true;
	}
	
	private void MovingDown (int multiplier)
	{
		transform.Translate (new Vector2 (-0.3f, -1.0f) * mVertiMoveSpeed * multiplier * Time.deltaTime);
		mMoving = true;
	}

	private IEnumerator ThrowTiles ()
	{
		for (int i = 0; i < difficulty; i++) {
			Instantiate (rock, new Vector3 (Random.Range (transform.position.x - 12, transform.position.x - 2), Random.Range (mFloorBoundary [Floor.Y_MIN_INDEX] - (mSpriteRenderer.bounds.size.y / 2.0f), mFloorBoundary [Floor.Y_MAX_INDEX] - (mSpriteRenderer.bounds.size.y / 2.0f)), -1.0f), Quaternion.identity);
			yield return new WaitForSeconds (0.5f);
		}
	}

	private IEnumerator CreateTitle ()
	{
		if (title [0] != null) {
			for (int i = 0; i < title.Length; i++) {
				Destroy (title [i]);
			}
		}

		for (int i = 0; i < title.Length; i++) {
			if (i % 2 == 0)
				title [i] = Instantiate (dig, new Vector3 (transform.position.x + 4.5f, 1.65f - (i * 1f), -1f), Quaternion.identity) as GameObject;
			else
				title [i] = Instantiate (dug, new Vector3 (transform.position.x + 4.6f, 1.65f - (i * 1f), -1f), Quaternion.identity) as GameObject;
		}

		for (int i = 0; i < title.Length; i++) {
			GameObject temp = title [i];
			int random = Random.Range (i, title.Length);
			title [i] = title [random];
			title [random] = temp;
		}
		yield return new WaitForSeconds (1.0f);
		throwTitle = true;
		finishThrowTitle = false;
	}

	private void ResetBoolean ()
	{
		mMoving = false;
	}

	private void UpdateAnimator ()
	{
		mAnimator.SetBool ("isMoving", mMoving);
		mAnimator.SetBool ("isDead", mDead);
		mAnimator.SetBool ("isThrowing", mThrowing);
		mAnimator.SetBool ("isPumping", mPumping);
		mAnimator.SetBool ("isHit", mHit);
		mAnimator.SetBool ("isThrowRock", mThrowRocks);
	}

	public int GetLives ()
	{
		return maxLife;
	}

	public void SetCanMove (bool a)
	{
		canMove = a;
	}

	public void IncreaseDifficulty ()
	{
		difficulty += 2;
		mVertiMoveSpeed += 0.25f;
	}

	public void SetThrowTitle ()
	{
		if (nextThrow < 5) {
			throwTitle = true;
			nextThrow++;
		}
	}

	public void SetInStory (bool a)
	{
		inStory = a;
	}

	public bool isPumping ()
	{
		return mPumping;
	}

	public void Reset ()
	{
		for (int i = 0; i < title.Length; i++) {
			Destroy (title [i]);
		}
		throwTitle = false;
		mThrowRocks = false;
		
		finishThrowTitle = true;
		ResetBoolean ();
		Destroy (hoseInstance);
	}

	public void SetColor ()
	{
		sr.color -= new Color (0f, 0.33f, 0.33f, 0f);
	}

	public void SetHealthBar ()
	{
		mHealthBarRef.transform.position += new Vector3 (0f, 0f, 30f);
	}
}
