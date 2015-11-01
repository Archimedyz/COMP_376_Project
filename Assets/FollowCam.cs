using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

    GameObject mFollowTarget;

	// Use this for initialization
	void Start () {
        mFollowTarget = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        // keep the initial z value, so we can see everything.
        // also, no need to look too far bottom.
        transform.position = new Vector3(mFollowTarget.transform.position.x, mFollowTarget.transform.position.y + 2.0f, transform.position.z);
	}
}
