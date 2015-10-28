using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour
{

	public float xMax;
	public float xMin;
	public float yMax;
	public float yMin;
	GameObject[] floorPieces;

	public const int X_MAX = 0, X_MIN = 1, Y_MAX = 2, Y_MIN = 3;
    public float paddingLeft, paddingRight, paddingTop, paddingBottom; // must be set in unity editor

	// Use this for initialization
	void Start ()
	{

        SpriteRenderer floorSegmentRenderer = GetComponentInChildren<SpriteRenderer>();
        int numberOfSegments = GetComponentsInChildren<SpriteRenderer>().Length;

        Debug.Log("lossyScale: " + transform.lossyScale);
        Debug.Log("localScale: " + transform.localScale);
        Debug.Log("SpriteRenderer.bounds.size: " + floorSegmentRenderer.bounds.size);

        xMax = transform.position.x - paddingRight + (floorSegmentRenderer.bounds.size.x * numberOfSegments / 2.0f);
        xMin = transform.position.x + paddingLeft - (floorSegmentRenderer.bounds.size.x * numberOfSegments / 2.0f);
        yMax = transform.position.y - paddingTop + (floorSegmentRenderer.bounds.size.y / 2.0f);
        yMin = transform.position.y + paddingBottom - (floorSegmentRenderer.bounds.size.y / 2.0f);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void GetBoundary (float[] boundary, SpriteRenderer sr)
	{
        Debug.Log("sr.bounds.size: " + sr.bounds.size);

        boundary[X_MAX] = xMax + (sr.bounds.size.x / 2.0f);
        boundary[X_MIN] = xMin - (sr.bounds.size.x / 2.0f);
		boundary[Y_MAX] = yMax + (sr.bounds.size.y / 2.0f);
		boundary[Y_MIN] = yMin + (sr.bounds.size.y / 2.0f); // we want the feet to be the point of reference of the vertical boundary
	}
}
