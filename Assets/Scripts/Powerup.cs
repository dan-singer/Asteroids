using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic data for a powerup
/// </summary>
/// <author>Dan Singer</author>
[RequireComponent(typeof(Collider))]
public class Powerup : MonoBehaviour {


    public Powerups.PowerupType powerupType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Handle collision events for Powerups.
    /// </summary>
    /// <param name="other"></param>
    private void CollisionStarted(Object other)
    {
        Collider coll = (Collider)other;
        ShipMovement ship = coll.GetComponent<ShipMovement>();
        if (ship)
        {
            GameManager.Instance.GetComponent<Powerups>().ActivatePowerup(powerupType);
            Destroy(gameObject);

        }
    }
}
