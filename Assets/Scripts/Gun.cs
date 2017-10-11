﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles player gun controls and behaviour
/// </summary>
///<author>Dan Singer</author>
public class Gun : MonoBehaviour {


    public Bullet bulletPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1"))
        {
            FireBullet();
        }
	}

    private void FireBullet()
    {
        Bullet bullet = Instantiate<Bullet>(bulletPrefab);
        bullet.direction = GetComponent<ShipMovement>().Direction;
    }
}