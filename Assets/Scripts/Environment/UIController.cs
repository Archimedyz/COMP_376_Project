using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {


    private HealthBar mHealthBar;

	// Use this for initialization
	void Start () {
	    // init the healthbar to the given amount.
        // There can be a total of 1.95 + 1.95 / 0.05 = 78 pieces of health.
        
        mHealthBar = GetComponentInChildren<HealthBar>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
