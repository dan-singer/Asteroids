using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component which allows for movement based on position, velocity, and acceleration.
/// </summary>
/// <author>Dan Singer</author>
public class VectorMovement : MonoBehaviour {

    public Vector3 velocity;
    public Vector3 acceleration;
    public float maxSpeed = 1f;
    public bool autoTick = false;

    public float InitMaxSpeed { get; private set; }

    // Use this for initialization
    void Start () {
        InitMaxSpeed = maxSpeed;
	}
	
    /// <summary>
    /// Update position based on velocity and/or acceleration
    /// </summary>
	public void Tick() {
        velocity += acceleration;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity;
	}

    private void Update()
    {
        if (autoTick)
            Tick();
    }
}
