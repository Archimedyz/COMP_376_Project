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

	private bool mMoving;
	private bool mDead = false;
	private bool mThrowing = false;
	private bool mPumping = false;
	private bool mHit;
	private bool mThrowRocks;

	private float hitTimer = 0.0f;
	private float throwRockTimer = 0.0f;

	void Start ()
	{
		mAnimator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
	}

	void Update ()
	{
		ResetBoolean ();

		if (Input.GetKey ("z")) {
			mThrowRocks = true;
		}
		if (mThrowRocks) {
			throwRockTimer += Time.deltaTime;
			if (throwRockTimer >= 2.0f) {
				throwRockTimer = 0.0f;
				mThrowRocks = false;
				StartCoroutine (ThrowTiles ());
			}
		}

		if (mHit) {
			hitTimer += Time.deltaTime;
			if (hitTimer >= 1.0f) {
				mHit = false;
			}
		} else {
			hitTimer = 0.0f;
		}

		UpdateAnimator ();
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Enemy") {
			mHit = true;
			UpdateAnimator ();
		}
	}

	private IEnumerator ThrowTiles ()
	{
		for (int i = 0; i < 5; i++) {
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
}
