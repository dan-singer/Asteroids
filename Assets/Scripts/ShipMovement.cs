using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Ship input and movement
/// </summary>
/// <author>Dan Singer</author>
[RequireComponent(typeof(VectorMovement))]
public class ShipMovement : MonoBehaviour {

    public Vector3 Direction { get; private set; }
    private VectorMovement movement;
    private float initAccelMag;
    private float initMaxSpeed;

    public float accelerationMagnitude = 0.01f;

    public float turnVelocity = 2f;

    public float airResistanceFactor = 0.9f;

    private Thruster thruster;

	/// <summary>
    /// Initialize values.
    /// </summary>
	void Start () {
        float angle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        //Initialize direction based on the starting angle of the ship.
        Direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        initAccelMag = accelerationMagnitude;
        movement = GetComponent<VectorMovement>();
        thruster = GetComponentInChildren<Thruster>();
        accelerationMagnitude = 0;
	}
	
	/// <summary>
    /// Manage ship movement.
    /// </summary>
	void Update () {

        Direction = Quaternion.Euler(0, 0, -Input.GetAxis("Horizontal") * turnVelocity) * Direction;

        //Look in the direction we're facing
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        //When to accelerate
        if (Input.GetButton("Thrust"))
        {
            accelerationMagnitude = initAccelMag;
            movement.maxSpeed = movement.InitMaxSpeed;
            thruster.enabled = true;
        }
        else
        {
            accelerationMagnitude = 0;
            if (movement.velocity.sqrMagnitude > Mathf.Pow((movement.maxSpeed / 2), 2))
                movement.velocity *= airResistanceFactor;
            thruster.enabled = false;
        }

        movement.acceleration = Direction * accelerationMagnitude;

        //Update the movement component each frame.
        movement.Tick();
	}
}
