using UnityEngine;
using System.Collections;

public class Coconut : MonoBehaviour
{
	private bool canThrow;
	private bool throwed;
	private float initialPositionX;

	public float mThrowForce;
	public float maxDisplacement;

	private float Timer;

	private Vector2 mDirection;

	private float maxExistTime = 5.0f;

	public int damage = 20;

	private Transform player;

	private float initializedTime;  // when is the coconut instantiated
	private float timeSincePlayerStartedDefending; // did they defend right after coconut was thrown
	private Rigidbody CoconutRGBD;
	private bool redirected;

	void Start ()
	{
		player = GameObject.Find ("Player").transform;

		canThrow = true;
		throwed = false;
		Timer = 0.0f;

		initializedTime = Time.timeSinceLevelLoad; // the exact time the cocunut was created
		CoconutRGBD = GetComponent<Rigidbody>();
		redirected = false;
	}

	void Update ()
	{
		Timer += Time.deltaTime;

		// capture when player started defending
		if(Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.V))
			timeSincePlayerStartedDefending = Time.timeSinceLevelLoad;

		if(!redirected)
			if (Timer >= 0.24f && Timer <= 0.3f) {
				transform.position = new Vector3 (transform.parent.position.x - 0.1f, transform.parent.position.y + 0.5f, transform.position.z);
			} else if (Timer >= 0.12f && Timer < 0.24f) {
				transform.position = new Vector3 (transform.parent.position.x - 0.5f, transform.parent.position.y - 0.1f, transform.position.z);
			} else if (Timer >= 0.3f) {
				if (canThrow)
					Throw ();
				transform.Translate (Vector3.Normalize (player.position - transform.position) * mThrowForce * Time.deltaTime);
			}

		if (throwed) {
			if (transform.position.x - initialPositionX >= maxDisplacement || Timer >= maxExistTime) {
				Destroy (gameObject);
			}
		}
	}

	public void Throw ()
	{
		canThrow = false;
		throwed = true;
		initialPositionX = transform.position.x;
		gameObject.transform.parent = null;
	}

	public void SetDirection (Vector2 direction)
	{
		mDirection = direction;
	}

	public Vector2 GetDirection ()
	{
		return mDirection;
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.name == "Player")   
		{
			if (timeSincePlayerStartedDefending > initializedTime && player.position.x > transform.position.x 
			    						&& player.gameObject.GetComponent<Player>().GetFacingDirection() == Vector2.left)    // redirect it back to enemy
			{
				redirected = true;
				CoconutRGBD.AddForce (10 * Vector3.left, ForceMode.VelocityChange);
			}
			else if (timeSincePlayerStartedDefending > initializedTime && player.position.x < transform.position.x 
			         					&& player.gameObject.GetComponent<Player>().GetFacingDirection() == Vector2.right)    // redirect it back to enemy
			{
				redirected = true;
				CoconutRGBD.AddForce (10 * Vector3.right, ForceMode.VelocityChange);
				
			}else
			{
				if (col.gameObject.GetComponent<Player> ().IsDefending ()) {
					col.gameObject.GetComponent<Player> ().GetBlockDamage (damage / 3);
				} else
					col.gameObject.GetComponent<Player> ().GetKnockdown (mDirection, damage);

				Destroy (gameObject);
			}
		}

		// if coconut hits another enemy do damage to them
		if (col.gameObject.name == "Hobo") {
			col.gameObject.GetComponent<Hobo>().GetHit(GetDirection(), 5, false); 
			Destroy (gameObject);
		}
		if (col.gameObject.name == "Neanderthal") {
			if(Time.timeSinceLevelLoad > initializedTime + 0.5f)
			{
				col.gameObject.GetComponent<Neanderthal>().GetHit(GetDirection(), 5, false); 
				Destroy(gameObject);
			}
		}
	}
}











