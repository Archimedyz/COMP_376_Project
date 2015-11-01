using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

    [SerializeField]
    GameObject mHealthFragmentPrefab;

    private GameObject[] mHealth;

    private const float healthDisplayOffset = -1.95f;
    private const int maxHeathFragments = 79;

	// Use this for initialization
	void Start () {
        mHealth = new GameObject[maxHeathFragments];
        for (int i = 0; i < maxHeathFragments; ++i)
        {
            mHealth[i] = Instantiate(mHealthFragmentPrefab, new Vector3(healthDisplayOffset + (i * 0.05f) + transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
            mHealth[i].transform.parent = gameObject.transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
