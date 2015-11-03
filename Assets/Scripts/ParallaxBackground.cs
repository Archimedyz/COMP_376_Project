using UnityEngine;
using System.Collections;

public class ParallaxBackground : MonoBehaviour {

    private ParallaxLayer[] mLayers;
    private Vector3 mInitPosition;
    private float oldX;

	// Use this for initialization
	void Start () {
        mLayers = GetComponentsInChildren<ParallaxLayer>();
        for (int i = 0; i < mLayers.Length; ++i)
        {
            mLayers[i].SetParallaxFactor(0.10f - 0.025f * i);
        }
        mInitPosition = transform.position;
        oldX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, mInitPosition.y, mInitPosition.z);
        if(transform.position.x != oldX) {
            float deltaX = transform.position.x - oldX;
            oldX = transform.position.x;
            foreach(ParallaxLayer layer in mLayers) {
                layer.Move(deltaX);
            }
        }
	}
}
