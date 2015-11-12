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

	private Animator mAnimator;
	private Rigidbody rb;

	public float mVertiMoveSpeed;

	public Transform mTarget;

	private Vector2 mFacingDirection;

	private float destroyTimer = 0.0f;
	private float fireTimer = 0.0f;
	private float nextFire = 5.0f;
	private float timer = 0.0f;

	void Start ()
	{
		mAnimator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
	}

	void Update ()
	{
		ResetBoolean ();

		if (mExplode) {
			destroyTimer += Time.deltaTime;
			if (destroyTimer >= 0.3f) {
				Debug.Log ("Destroy");
				Destroy (gameObject);
			}
		}
		
		if (Life <= 0) {
			mDead = true;
		}
		
		if (!mDead && !mExplode) {
			timer += Time.deltaTime;
			if (timer > nextFire) {
				BreathFire ();
			}
			
			if (mTarget.position.x >= transform.position.x)
				FaceDirection (Vector2.right);
			else
				FaceDirection (Vector2.left);
		} else {
			
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
			else {
				rb.isKinematic = false;
				rb.AddForce (Vector2.right * 10, ForceMode.Impulse);
			}
		} 
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.name == "DigDug") {
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
}
