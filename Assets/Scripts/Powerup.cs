using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic data for a powerup
/// </summary>
[RequireComponent(typeof(Collider))]
public class Powerup : MonoBehaviour {


    public Powerups.PowerupType powerupType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CollisionStarted(Object other)
    {
        Collider coll = (Collider)other;
        Powerups pUps = coll.GetComponent<Powerups>();
        if (pUps)
        {
            pUps.ActivatePowerup(powerupType);
            Destroy(gameObject);

        }
    }
}
