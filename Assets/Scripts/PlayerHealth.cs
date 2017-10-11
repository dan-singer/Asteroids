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
    public int lives = 3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CollisionStarted(Object other)
    {
        Collider coll = (Collider)other;
        if (coll.GetComponent<Asteroid>())
        {
            health--;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        //TODO implement die method
        print("Killed Player");
    }
}
