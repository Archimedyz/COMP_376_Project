using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public GameObject neanderthalPrefab;
	public int neanderthalNumber;
	public GameObject hoboPrefab;
	public int hoboNumber;

	private GameObject[] neanderthalHolder;
	private GameObject[] hoboHolder;

	public float instantiationWait;
	private float appearTimer = 0.0f;

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
	AudioSource dissapearing;
	AudioSource hoboAppearing;
	AudioSource neanderthalAppearing;
	float audioTimer = 0.0f;

	public Stats mStats;
	
	UICanvas uiCanvas;
	private Vector3 damagePositionOffset = new Vector3 (0, 1f, -1);
	
	public int expGiven = 100;
	
	private int staggerTimer = 0;

	private bool firstTime = true;

	void Start ()
	{
		neanderthalHolder = new GameObject[neanderthalNumber];
		hoboHolder = new GameObject[hoboNumber];

		mStats = new Stats (1, 70, 18, 2, 0, new int[] { 20, 4, 2, 0 });
		mAnimator = GetComponent<Animator> ();

		mDying = false;

		mTarget = GameObject.Find ("Player").transform;

		AudioSource[] audioSources = GetComponents<AudioSource> ();
		normalHit = audioSources [0];
		strongHit = audioSources [1];
		dissapearing = audioSources [2];
		hoboAppearing = audioSources [3];
		neanderthalAppearing = audioSources [4];

		uiCanvas = (UICanvas)GameObject.FindGameObjectWithTag ("UICanvas").GetComponent<UICanvas> ();
	}

	void Update ()
	{
		if (mStats.isDead () && !mDying) {
			Die ();
		}

		if (!mStats.isDead () && !mDying && !mGetHit) {
			if (Vector3.Distance (transform.position, mTarget.position) <= appearRange) {
				if (appearTimer >= instantiationWait || firstTime) {
					firstTime = false;
					for (int i = 0; i < hoboNumber; i++) {
						if (hoboHolder [i] == null) {
							hoboAppearing.Play ();
							appearTimer = 0f;
							hoboHolder [i] = Instantiate (hoboPrefab, transform.position + new Vector3 (Random.Range (-2f, 2f), -1f, -1f), Quaternion.identity) as GameObject;
							break;
						}
					}
					if (appearTimer > 0f) {
						for (int i = 0; i < neanderthalNumber; i++) {
							if (neanderthalHolder [i] == null) {
								neanderthalAppearing.Play ();
								appearTimer = 0f;
								neanderthalHolder [i] = Instantiate (neanderthalPrefab, transform.position + new Vector3 (Random.Range (-2f, 2f), -1f, 0f), Quaternion.identity) as GameObject;
								break;
							}
						}
					}
				}
			}
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

		if (!isFull ())
			appearTimer += Time.deltaTime;
	}

	private bool isFull ()
	{
		for (int i = 0; i < hoboNumber; i++) {
			if (hoboHolder [i] == null) {
				return false;
			}
		}

		for (int i = 0; i < neanderthalNumber; i++) {
			if (neanderthalHolder [i] == null) {
				return false;
			}
		}

		return true;
	}

	public void GetHit (Vector2 direction, int damage, bool isCrit)
	{
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
		dissapearing.Play ();
	}

	private void UpdateAnimator ()
	{
		mAnimator.SetBool ("isDying", mDying);
	}
}
