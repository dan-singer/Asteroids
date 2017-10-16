using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle health for the player
/// </summary>
/// <author>Dan Singer</author>
[RequireComponent(typeof(Collider))]
public class PlayerHealth : MonoBehaviour {


    public int health = 1;

    public bool canTakeDamage = true;

    /// <summary>
    /// Called when player dies.
    /// </summary>
    public event Action DeathEvent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    /// <summary>
    /// Handle collision events for the Player
    /// </summary>
    private void CollisionStarted(UnityEngine.Object other)
    {
        Collider coll = (Collider)other;
        if (coll.GetComponent<Asteroid>() && canTakeDamage)
        {
            health--;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    /// <summary>
    /// Broadcast the death event, and allow another object to handle it.
    /// </summary>
    private void Die()
    {
        if (DeathEvent != null)
            DeathEvent();
    }
}
