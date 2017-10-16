using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simulates Asteroid behaviour.
/// </summary>
/// <author>Dan Singer</author>
[RequireComponent(typeof(VectorMovement))]
public class Asteroid : MonoBehaviour {

    /// <summary>
    /// Note that this is precomputed to a random unit vector at the start if level == 1
    /// </summary>
    public Vector3 Direction { get; private set; }
    public float accelerationMagnitudeMean = 0.0005f;
    public float accelerationStdDev = 0.0001f;
    public int level = 1;

    /// <summary>
    /// How many points is this worth?
    /// </summary>
    public int Value { get; set; }

    public float AccelerationMagnitude { get; private set; }

    private VectorMovement movement;
    private float directionStdDev = 10.0f;


	/// <summary>
    /// Initialize movement. 
    /// </summary>
	void Start () {
        movement = GetComponent<VectorMovement>();
        if (level == 1)
        {
            if (GameManager.Instance.PlayerInstance)
            {
                Vector3 dir = GameManager.Instance.PlayerInstance.transform.position - transform.position;
                Direction = dir.normalized;
                Direction = Quaternion.Euler(0, 0, RandomUtils.Gaussian(0, directionStdDev)) * Direction;
            }
            else
            {
                Direction = new Vector3(1, 0, 0);
                Direction = Quaternion.Euler(0, 0, Random.Range(0, 360.0f)) * Direction;
            }
            AccelerationMagnitude = RandomUtils.Gaussian(accelerationMagnitudeMean, accelerationStdDev);
        }
	}

    /// <summary>
    /// Set the direction and magnitude of acceleration for this asteroid.
    /// </summary>
    public void SetDirectionAndAcceleration(Vector3 direction, float accelerationMagnitude)
    {
        Direction = direction.normalized;
        AccelerationMagnitude = accelerationMagnitude;
    }
	
	// Update is called once per frame
	void Update () {
        movement.acceleration = AccelerationMagnitude * Direction;
        movement.Tick();
	}
}
