using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour
{

	float xMax;
	float xMin;
	float yMax;
	float yMin;
	GameObject[] floorPieces;

	const int X_MAX = 0, X_MIN = 1, Y_MAX = 2, Y_MIN = 3;

	// Use this for initialization
	void Start ()
	{
		yMax = transform.position.y + 4.5f;
		yMin = transform.position.y - 4.5f;
		//xMax = ;
		//xMin = ;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void GetBoundary (GameObject g)
	{
		float[] boundary = new float[4];
		boundary [X_MAX] = 0.0f;
		boundary [X_MIN] = 0.0f;
		boundary [Y_MAX] = yMax + (g.transform.lossyScale.y / 2.0f);
		boundary [Y_MIN] = yMin + (g.transform.lossyScale.y / 2.0f);
	}
}
