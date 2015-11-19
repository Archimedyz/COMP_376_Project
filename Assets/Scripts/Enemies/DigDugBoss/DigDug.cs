using UnityEngine;
using System.Collections;

public class DigDug : MonoBehaviour
{
	public GameObject Tile1;
	public GameObject Tile2;
	public GameObject Tile3;
	public GameObject Tile4;
	public GameObject rock;
	public GameObject dig;
	public GameObject dug;
	public GameObject hose;

	private GameObject hoseInstance = null;

	private float titleSpeed = 1.0f;
	private GameObject[] title;
	private bool throwTitle = false;
	private int nextThrow = 0;
	private bool allo = false;

	public float tileRangeXMin;
	public float tileRangeXMax;
	public float tileRangeY;

	private Animator mAnimator;
	private Rigidbody rb;
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
	
	public float maxY, minY;
	private bool moveDown = true, moveUp = false;

	private int difficulty = 6;

	private Transform camTransform;
	public float shake = 2.0f;
	public float shakeAmount = 0.05f;
	
	Vector3 originalPos;

	private float pumpingTimer = 0.0f;
	private float maxPumpingTimer = 2.0f;

	private Transform player;

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
		rb = GetComponent<Rigidbody> ();
		sr = transform.GetChild (0).GetComponent<SpriteRenderer> ();	
		title = new GameObject[6];
		player = GameObject.Find ("Player").transform;
	}

	void Update ()
	{
		ResetBoolean ();

		if (mHit) {
			hitTimer += Time.deltaTime;
			if (hitTimer >= 1.0f) {
				hitTimer = 0.0f;
				mHit = false;
				if (maxLife == 2) {
					sr.color = new Color (1f, 0.4f, 0.4f, 1f);
				} else if (maxLife == 1) {
					sr.color = new Color (1f, 0f, 0f, 1f);
				} 
			}
		}

			
		if (canMove && !mHit) {
			if (mPumping) {
				pumpingTimer += Time.deltaTime;
				if (pumpingTimer >= maxPumpingTimer) {
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

				if (Mathf.Abs (player.position.y - transform.position.y) <= 0.1) {
					Pumping ();
				}
			} else {
				float specialAttack = Random.Range (0.0f, 100.0f);
				if (specialAttack > 99.5f) {
					int whichAttack = Random.Range (0, 2);
					if (whichAttack == 0 && !mHit && !mThrowRocks && !allo)
						mThrowRocks = true;
					//else if (whichAttack == 1 && !mHit && !mThrowRocks && !allo)
					//StartCoroutine (CreateTitle ());
				}
				if (mThrowRocks) {
					throwRockTimer += Time.deltaTime;
					ShakeCamera ();
					if (throwRockTimer >= 2.0f) {
						throwRockTimer = 0.0f;
						mThrowRocks = false;
						//StartCoroutine (ThrowTiles ());
					}
				} else {
					if (transform.position.y - 0.1f <= minY) {
						moveUp = true;
						moveDown = false;
					} else if (transform.position.y + 0.1f >= maxY) {
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
		}

		if (throwTitle) {
			title [nextThrow].GetComponent<Title> ().SetLaunch ();
			throwTitle = false;
		}

		if (invincible) {
			invincibleTimer += Time.deltaTime;
			if (invincibleTimer > maxInvincibleTimer) {
				invincibleTimer = 0.0f;
				invincible = false;
			}
		}

		UpdateAnimator ();
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "Enemy" && !invincible) {
			mHit = true;
			invincible = true;
			maxLife --;
			difficulty += 2;
			UpdateAnimator ();
			IncreaseDifficulty ();
			if (maxLife >= 0) {
				GameObject.Find ("Enemies").GetComponent<Boss1Controller> ().IncreaseDifficulty ();
				GameObject.Find ("Enemies").GetComponent<Boss1Controller> ().CreateWave ();
			}
		}
	}

	private void Pumping ()
	{
		hoseInstance = Instantiate (hose, transform.position, Quaternion.identity) as GameObject;
		mThrowing = true;
		mPumping = true;
	}

	private void ShakeCamera ()
	{
		Debug.Log ("Allo");
		if (shake > 0.0f) {
			camTransform.localPosition = originalPos + Random.insideUnitSphere / 10.0f * shakeAmount;
			
			shake -= Time.deltaTime;
		} else {
			shake = 2.0f;
			camTransform.localPosition = originalPos;
		}
		
		Debug.Log (camTransform.localPosition);
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
			int a = Random.Range (0, 6);
			if (a == 0) {
				Instantiate (Tile1, new Vector3 (Random.Range (tileRangeXMin, tileRangeXMax), tileRangeY, -1.0f), Quaternion.identity);
			} else if (a == 1) {
				Instantiate (Tile2, new Vector3 (Random.Range (tileRangeXMin, tileRangeXMax), tileRangeY, -1.0f), Quaternion.identity);
			} else if (a == 2) {
				Instantiate (Tile3, new Vector3 (Random.Range (tileRangeXMin, tileRangeXMax), tileRangeY, -1.0f), Quaternion.identity);
			} else if (a == 3) {
				Instantiate (Tile4, new Vector3 (Random.Range (tileRangeXMin, tileRangeXMax), tileRangeY, -1.0f), Quaternion.identity);
			} else if (a == 4) {
				Instantiate (rock, new Vector3 (Random.Range (tileRangeXMin, tileRangeXMax), tileRangeY, -1.0f), Quaternion.identity);
			} else if (a == 5) {
				Instantiate (rock, new Vector3 (Random.Range (tileRangeXMin, tileRangeXMax), tileRangeY, -1.0f), Quaternion.identity);
			}
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
				title [i] = Instantiate (dig, new Vector3 (9.3f, 3.5f - (i * 1.5f), -1f), Quaternion.identity) as GameObject;
			else
				title [i] = Instantiate (dug, new Vector3 (9.4f, 3.5f - (i * 1.5f), -1f), Quaternion.identity) as GameObject;
		}

		for (int i = 0; i < title.Length; i++) {
			GameObject temp = title [i];
			int random = Random.Range (i, title.Length);
			title [i] = title [random];
			title [random] = temp;
		}
		yield return new WaitForSeconds (1.0f);
		throwTitle = true;
		allo = true;
	}

	private void ThrowTitle ()
	{

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

	private void IncreaseDifficulty ()
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
}
