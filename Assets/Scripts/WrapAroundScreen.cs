﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapAroundScreen : MonoBehaviour {

    public Camera mainCam;

    private Vector3 upperRight, lowerLeft;
    private Vector3 myExtents;
    private Vector3 position;

	// Use this for initialization
	void Start () {
        //Figure out where the upper right and lower left of the viewport are
        float vertExtents = mainCam.orthographicSize;
        float horzExtents = mainCam.orthographicSize * mainCam.aspect;
        Vector3 extents = new Vector3(horzExtents, vertExtents, 0);
        upperRight = mainCam.transform.position + extents;
        lowerLeft = mainCam.transform.position - extents;

        //We'll use this to wrap seamlessly
        myExtents = GetComponent<SpriteRenderer>().bounds.extents;
    
        position = transform.position;
    }

    // Update is called once per frame
    void Update () {
        //Make sure we grab the most up-to-date position
        position = transform.position;
        //Check if we're out of the screen
        if (position.x > upperRight.x + myExtents.x)
            position.x = lowerLeft.x - myExtents.x;
        else if (position.x < lowerLeft.x - myExtents.x)
            position.x = upperRight.x + myExtents.x;
        if (position.y > upperRight.y + myExtents.y)
            position.y = lowerLeft.y - myExtents.y;
        else if (position.y < lowerLeft.y - myExtents.y)
            position.y = upperRight.y + myExtents.y;
        //Update the position
        transform.position = position;            
	}
}
