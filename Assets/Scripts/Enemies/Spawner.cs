using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	private bool mGetHit;
	private bool mDying;
	private float dyingTimer = 0.0f;
	
	private Transform mTarget;
	public float appearRange;
	
	private Animator mAnimator;
	
	private float mInvincibleTimer = 0.0f;
	private float kInvincibilityDuration = 0.1f;

	AudioSource strongHit;
	AudioSource normalHit;
	float audioTimer = 0.0f;

	public Stats mStats;
	
	UICanvas uiCanvas;
	private Vector3 damagePositionOffset = new Vector3 (0, 0.7f, 0);
	
	public int expGiven = 100;
	
	private int staggerTimer = 0;

	void Start ()
	{
		mStats = new Stats (1, 70, 18, 2, 0, new int[] { 20, 4, 2, 0 });
		mAnimator = GetComponent<Animator> ();

		mDying = false;

		AudioSource[] audioSources = GetComponents<AudioSource> ();
		normalHit = audioSources [0];
		strongHit = audioSources [1];

		uiCanvas = (UICanvas)GameObject.FindGameObjectWithTag ("UICanvas").GetComponent<UICanvas> ();
	}

	void Update ()
	{
		if (mStats.isDead () && !mDying) {
			Die ();
		}

		UpdateAnimator ();
		
		if (mDying) {
			dyingTimer += Time.deltaTime;
			if (dyingTimer >= 0.8f) {
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
	}

	public void GetHit (Vector2 direction, int damage, bool isCrit)
	{
		Debug.Log ("Allo");

		if (!mGetHit && !mDying) {
			mGetHit = true;
			
			if (GameObject.Find ("Player").GetComponent<Player> ().IsStrongAttack ()) {
				staggerTimer = 25 - (int)(staggerTimer * 0.10f);
				damage = (int)(damage * 1.4f);
				if (!strongHit.isPlaying)
					strongHit.Play ();
			} else {
				staggerTimer = 15 - (int)(staggerTimer * 0.10f);
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

	private void Die ()
	{
		mDying = true;
	}

	private void UpdateAnimator ()
	{
		mAnimator.SetBool ("isDying", mDying);
	}
}
