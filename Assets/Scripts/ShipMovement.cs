using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Ship input and movement
/// </summary>
/// <author>Dan Singer</author>
[RequireComponent(typeof(VectorMovement))]
public class ShipMovement : MonoBehaviour {

    private Vector3 direction;
    private VectorMovement movement;
    private float initAccelMag;
    private float initMaxSpeed;

    public float accelerationMagnitude = 0.01f;

    public float turnVelocity = 2f;

    public float airResistanceFactor = 0.9f;

	// Use this for initialization
	void Start () {
        direction = new Vector3(1, 0, 0);
        initAccelMag = accelerationMagnitude;
        movement = GetComponent<VectorMovement>();
        accelerationMagnitude = 0;
	}
	
	// Update is called once per frame
	void Update () {

        direction = Quaternion.Euler(0, 0, -Input.GetAxis("Horizontal") * turnVelocity) * direction;

        //Look in the direction we're facing
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        //When to accelerate
        if (Input.GetButton("Thrust"))
        {
            accelerationMagnitude = initAccelMag;
            movement.maxSpeed = movement.InitMaxSpeed;
        }
        else
        {
            accelerationMagnitude = 0;
            if (movement.velocity.sqrMagnitude > Mathf.Pow((movement.maxSpeed / 2), 2))
                movement.velocity *= airResistanceFactor;
        }

        movement.acceleration = direction * accelerationMagnitude;


        movement.Tick();
	}
}
