using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the thrusters on the ship.
/// </summary>
/// <author>Dan Singer</author>
public class Thruster : MonoBehaviour {

    /// <summary>
    /// Make sure that these are children of the gameobject this component is attached to!
    /// </summary>
    private SpriteRenderer[] rockets;


    private float startTime;

    private bool activated = false;
    /// <summary>
    /// Property which will return the state of the rockets, or (de)activate them.
    /// </summary>
    public bool Activated
    {
        get
        {
            return activated;
        }
        set
        {
            activated = value;
            if (activated)
            {
                startTime = Time.time;
            }
            foreach (SpriteRenderer renderer in rockets)
                renderer.enabled = activated;
        }
    }

	// Use this for initialization
	void Start () {
        rockets = GetComponentsInChildren<SpriteRenderer>();
        Activated = false;
	}

    /// <summary>
    /// Make the rockets act like flames.
    /// </summary>
    private void FlameEffect()
    {

    }
	
	// Update is called once per frame
	void Update () {
		if (Activated)
        {
            FlameEffect();
        }
	}
}
