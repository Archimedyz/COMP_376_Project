using UnityEngine;
using System.Collections;

public class Metro : MonoBehaviour {

    public float mSpeed;
    public bool mGoingRight;
    public float mDistanceToLive;

    float mInitialX;

    SpriteRenderer mSpriteRenderer;

	// Use this for initialization
	void Start () {
	    mSpriteRenderer = GetComponent<SpriteRenderer>();
        mInitialX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate((mGoingRight ? Vector3.right : Vector3.left) * mSpeed * Time.deltaTime);
        if(Mathf.Abs(mInitialX - transform.position.x) >= mDistanceToLive) {
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("YOYO > " + col.gameObject.tag);
        if(col.gameObject.tag == "Player") {
            Debug.Log("Metro hits player.");
        }
    }

    public void SetOrderInLayer(int orderInLayer) {
        Debug.Log("HERE");
        mSpriteRenderer.sortingOrder = orderInLayer;
    }

    public void SetGoingRight(bool goingRight)
    {
        mGoingRight = goingRight;
    }

    public void SetDistanceToLive(float distanceToLive)
    {
        mDistanceToLive = distanceToLive;
    }
}
