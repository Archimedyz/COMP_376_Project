using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour
{

	private float xMax;
	private float xMin;
	private float yMax;
	private float yMin;
	private GameObject[] floorPieces;


    public int floorLevel; // 0 is front most.
    public const int X_MAX_INDEX = 0, X_MIN_INDEX = 1, Y_MAX_INDEX = 2, Y_MIN_INDEX = 3;
    public float paddingLeft, paddingRight, paddingTop, paddingBottom; // must be set in unity editor

	// Use this for initialization
	void Start ()
	{
        transform.position = new Vector3(transform.position.x, transform.position.y, floorLevel);

        SpriteRenderer floorSegmentRenderer = GetComponentInChildren<SpriteRenderer>();
        int numberOfSegments = GetComponentsInChildren<SpriteRenderer>().Length;
        
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
        boundary[X_MAX_INDEX] = xMax - (sr.bounds.size.x / 2.0f);
        boundary[X_MIN_INDEX] = xMin + (sr.bounds.size.x / 2.0f);
		boundary[Y_MAX_INDEX] = yMax + (sr.bounds.size.y / 2.0f); // we want the feet to be the point of reference of the vertical boundary
		boundary[Y_MIN_INDEX] = yMin + (sr.bounds.size.y / 2.0f); 
	}
}
