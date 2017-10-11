using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Small behaviour which will destroy this object after a time period
/// </summary>
/// <author>Dan Singer</author>
public class DestroyAfterTime : MonoBehaviour {

    public float lifeTime = 3.0f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
