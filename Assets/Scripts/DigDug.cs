using UnityEngine;
using System.Collections;

public class DigDug : MonoBehaviour
{
	public GameObject Tile1;
	public GameObject Tile2;
	public GameObject Tile3;
	public GameObject Tile4;
	public GameObject rock;

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

	public Transform camTransform;

	public float shake = 2.0f;

	public float shakeAmount = 0.05f;
	
	Vector3 originalPos;

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
			float willThrowRocks = Random.Range (0.0f, 100.0f);
			if (willThrowRocks > 99.0f && !mHit && !mThrowRocks) {
				mThrowRocks = true;
			}
			if (mThrowRocks) {
				throwRockTimer += Time.deltaTime;
				ShakeCamera ();
				if (throwRockTimer >= 2.0f) {
					throwRockTimer = 0.0f;
					mThrowRocks = false;
					StartCoroutine (ThrowTiles ());
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
					MovingDown ();
				} else if (moveUp) {
					MovingUp ();
				}
			}
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
}
