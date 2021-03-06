﻿using UnityEngine;
using System.Collections;

public class FloorController : MonoBehaviour {

    private Floor[] mFloors;

	// Use this for initialization
    void Start()
    {
        // get all the floors in the current level.
        mFloors = GetComponentsInChildren<Floor>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GetCurrentFloorBoundary(float[] boundary, int floorIndex, SpriteRenderer sr)
    {
        if(floorIndex >= 0 && floorIndex < mFloors.Length)
        {
            mFloors[floorIndex].GetBoundary(boundary, sr);
        }
        else{        
            mFloors[0].GetBoundary(boundary, sr);
        }
    }

    // returns the index of the next floor on a higher "layer" to which the object will be bounded.
    // returns the lowest index if can't go further up.
    public int NextFloorUp(int currFloorIndex)
    {
        if(mFloors.Length > 1 && currFloorIndex > 0 && currFloorIndex < mFloors.Length) {
            return currFloorIndex - 1;
        }
        return 0;
    }

    // returns the index of the next floor on a lower "layer" to which the object will be bounded.
    // returns the highest index if can't go further down.
    public int NextFloorDown(int currFloorIndex)
    {
        if (mFloors.Length > 1 && currFloorIndex >= 0 && currFloorIndex < mFloors.Length - 1)
        {
            return currFloorIndex + 1;
        }
        return mFloors.Length - 1;
    }
}
