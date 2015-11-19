using UnityEngine;
using System.Collections;

public class Metro : MonoBehaviour {

    public float mSpeed;
    public bool goingRight;

    SpriteRenderer mSpriteRenderer;

	// Use this for initialization
	void Start () {
	    mSpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate((goingRight ? Vector3.right : Vector3.left) * mSpeed * Time.deltaTime);
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player") {
            Debug.Log("Metro hits player.");
        }
    }

    public void SetOrderInLayer(int orderInLayer) {
        mSpriteRenderer.sortingOrder = orderInLayer;
    }
}
