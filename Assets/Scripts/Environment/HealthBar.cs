using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{

	[SerializeField]
	GameObject
		mHealthFragmentPrefab;

	private GameObject[] mHealth;

	private const float healthDisplayOffset = -1.95f;
	private const int maxHeathFragments = 79; // SO CLOSE TO 80
	private int mHealthIndex;
	private float maxHealthValue = 100.0f;
	private float mHealthValue;

	// Use this for initialization
	void Start ()
	{
		mHealth = new GameObject[maxHeathFragments];
		for (int i = 0; i < maxHeathFragments; ++i) {
			mHealth [i] = Instantiate (mHealthFragmentPrefab, new Vector3 (), Quaternion.identity) as GameObject;
			mHealth [i].transform.parent = gameObject.transform;
			mHealth [i].transform.localScale = transform.parent.localScale;
			mHealth [i].transform.localPosition = new Vector3 (healthDisplayOffset + (i * 0.05f), 0.0f, 0.0f);
		}
		mHealthIndex = maxHeathFragments - 1;

		mHealthValue = maxHealthValue;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void LoseHealth (float hpLoss)
	{
		// determine how may health fragments the hpLoss represents.
		int oldFragments = (int)((mHealthValue / maxHealthValue) * maxHeathFragments);
		mHealthValue = Mathf.Clamp (mHealthValue - hpLoss, 0.0f, maxHealthValue);
		int newFragments = (int)((mHealthValue / maxHealthValue) * maxHeathFragments);
        
		for (int i = oldFragments; i > newFragments; --i) {
			mHealth[i - 1].SetActive (false);
            --mHealthIndex;
		}
	}

	public void GainHealth (float hpGain)
	{
		// determine how may health fragments the hpLoss represents.
		int oldFragments = (int)((mHealthValue / maxHealthValue) * maxHeathFragments);
		mHealthValue = Mathf.Clamp (mHealthValue + hpGain, 0.0f, maxHealthValue);
		int newFragments = (int)((mHealthValue / maxHealthValue) * maxHeathFragments);

		for (int i = oldFragments; i < newFragments; ++i) {
            mHealth[i].SetActive(true);
            ++mHealthIndex;
		}
		
	}

	public float GetHealth ()
	{
		return mHealthValue;
	}

	public float GetMaxHealth ()
	{
		return maxHealthValue;
	}

	public void SetHealth (float healthValue)
	{
        if (healthValue > mHealthValue) {
			GainHealth (healthValue - mHealthValue);
		} else {
			LoseHealth (mHealthValue - healthValue);
		}
	}

	public void SetMaxHealth (float newMax)
	{
		if ((int)newMax == (int)maxHealthValue)
			return;
		bool moreHealth = newMax > maxHealthValue;
        float oldMax = maxHealthValue;
        Debug.Log(oldMax + " <> " + newMax);
		maxHealthValue = newMax;
        mHealthValue += 2 * (newMax - oldMax);
		if (moreHealth) {
            Debug.Log("Lose Health - " + mHealthValue);
			LoseHealth (newMax - oldMax);
        } else {
            Debug.Log("Gain Health - " + mHealthValue);
			GainHealth (oldMax - newMax);
        }
	}
}
