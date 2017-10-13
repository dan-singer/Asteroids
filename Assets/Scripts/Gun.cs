using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles player gun controls and behaviour
/// </summary>
///<author>Dan Singer</author>
public class Gun : MonoBehaviour {


    public Bullet bulletPrefab;

    public float minDurationBetweenShots = 0.1f;

    public bool canHoldToFire = false;

    private float timeFired = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        bool check = canHoldToFire ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");

		if (check)
        {
            if (Time.time > timeFired + minDurationBetweenShots)
            {
                FireBullet();
                timeFired = Time.time;
            }
        }
    }

    private void FireBullet()
    {
        Bullet bullet = Instantiate<Bullet>(bulletPrefab, transform.position, transform.rotation);
        bullet.direction = GetComponent<ShipMovement>().Direction;
    }
}
