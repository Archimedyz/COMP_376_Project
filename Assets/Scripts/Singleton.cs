using UnityEngine;
using System.Collections;

public class Singleton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (GameObject.FindGameObjectsWithTag(gameObject.tag).Length > 1)
        {
            Destroy(gameObject);
        }
        GameObject.DontDestroyOnLoad(gameObject); // we are the master, please don't hurt me
	}
}
