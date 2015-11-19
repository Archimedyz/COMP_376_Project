using UnityEngine;
using System.Collections;

public class ParallaxLayer : MonoBehaviour {

    Vector3 mInitPosition;
    public float parallaxFactor;
	// Use this for initialization
	void Start () {
        mInitPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Move(float deltaX)
    {
        transform.Translate(new Vector3(-deltaX, 0.0f, 0.0f) * parallaxFactor);
    }

    public void SetParallaxFactor(float factor)
    {
        parallaxFactor = factor;
    }
}
