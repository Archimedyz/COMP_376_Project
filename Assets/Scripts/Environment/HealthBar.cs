using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

    [SerializeField]
    GameObject mHealthFragmentPrefab;

    private GameObject[] mHealth;

    private const float healthDisplayOffset = -1.95f;
    private const int maxHeathFragments = 79; // SO CLOSE TO 80
    private int mHealthIndex;
    private float maxHealthValue;
    private float mHealthValue;

	// Use this for initialization
	void Start () {
        mHealth = new GameObject[maxHeathFragments];
        for (int i = 0; i < maxHeathFragments; ++i)
        {
            mHealth[i] = Instantiate(mHealthFragmentPrefab, new Vector3(healthDisplayOffset + (i * 0.05f) + transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
            mHealth[i].transform.parent = gameObject.transform;
        }
        mHealthIndex = maxHeathFragments - 1;

        maxHealthValue = 100.0f;
        mHealthValue = maxHealthValue;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoseHealth(float hpLoss)
    {
        // determine how may health fragments the hpLoss represents.
        int oldFragments = (int)((mHealthValue / maxHealthValue) * maxHeathFragments);
        mHealthValue = Mathf.Clamp(mHealthValue - hpLoss, 0.0f, maxHealthValue);
        int newFragments = (int)((mHealthValue / maxHealthValue) * maxHeathFragments);
        
        int diffFragments = oldFragments - newFragments;


        for (int i = 0; i < diffFragments; ++i)
        {
            mHealth[mHealthIndex - i].SetActive(false);
        }
        mHealthIndex -= diffFragments;
    }

    public void GainHealth(float hpGain)
    {
        // determine how may health fragments the hpLoss represents.
        int oldFragments = (int)((mHealthValue / maxHealthValue) * maxHeathFragments);
        mHealthValue = Mathf.Clamp(mHealthValue + hpGain, 0.0f, maxHealthValue);
        int newFragments = (int)((mHealthValue / maxHealthValue) * maxHeathFragments);

        int diffFragments = newFragments - oldFragments;


        for (int i = 0; i < diffFragments; ++i)
        {
            mHealth[mHealthIndex + i + 1].SetActive(true);
        }
        mHealthIndex += diffFragments;
    }

    public float GetHealth()
    {
        return mHealthValue;
    }

    public void SetHealth(float healthValue)
    {
        if(healthValue > mHealthValue) {
            LoseHealth(healthValue - mHealthValue);
        }
        else
        {
            GainHealth(mHealthValue - healthValue);
        }
    }
}
