using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour for a bullet.
/// </summary>
/// <author>Dan Singer</author>
[RequireComponent(typeof(VectorMovement))]
public class Bullet : MonoBehaviour {

    public float speed = .1f;
    public Vector3 direction;

    private VectorMovement movement;

	// Use this for initialization
	void Start () {
        movement = GetComponent<VectorMovement>();
	}
	
	// Update is called once per frame
	void Update () {
        direction.Normalize();

        movement.velocity = speed * direction;
        movement.Tick();

	}

    /// <summary>
    /// Handle collision events for the Bullet
    /// </summary>
    private void CollisionStarted(Object other)
    {
        if (! ((Collider)other).GetComponent<ShipMovement>())
        {
            Destroy(gameObject);
            CollisionManager.UpdateAllColliders();
        }
    }
}
