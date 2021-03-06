﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles health for the asteroid.
/// </summary>
/// <author>Dan Singer</author>
public class AsteroidHealth : MonoBehaviour {

    public int health = 1;
    public AsteroidSpawner Spawner { get; set; }

    /// <summary>
    /// Handle collision events for the Asteroid
    /// </summary>
    /// <param name="other"></param>
    private void CollisionStarted(Object other)
    {
        Collider coll = (Collider)other;
        if (coll.GetComponent<Bullet>())
        {
            health--;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    /// <summary>
    /// Destroy this asteroid, and split into more pieces if its level is lower than Spawner.maxAsteroidLevel
    /// </summary>
    private void Die()
    {
        Asteroid asteroid = GetComponent<Asteroid>();
        GameManager.Instance.Score += asteroid.Value;
        if (asteroid.level < Spawner.maxAsteroidLevel)
        {
            float maxAngle = 45.0f;
            float curAngle = -maxAngle;
            int toSpawn = 2;
            int divisions = toSpawn - 1;
            float scale = 1 / (Mathf.Pow(2, asteroid.level));
            //Here, we precisely spawn two asteroids 90 degrees apart from each other.
            for (int i=0; i<toSpawn; i++)
            {
                Vector3 dir = Quaternion.Euler(0, 0, curAngle) * asteroid.Direction;

                //Offset used to make sure half asteroids don't touch
                float scalar = Mathf.Abs( (GetComponent<Collider>().Radius / 2) / Mathf.Sin(curAngle * Mathf.Deg2Rad));
                Vector3 offset = dir * scalar;

                GameObject duplicate = Instantiate<GameObject>(gameObject, transform.position + offset, transform.rotation);
                duplicate.GetComponent<AsteroidHealth>().Spawner = Spawner;
                Asteroid astDup = duplicate.GetComponent<Asteroid>();
                astDup.level = asteroid.level + 1;

                astDup.transform.localScale = new Vector3(scale, scale, scale);
                astDup.SetDirectionAndAcceleration(dir, asteroid.AccelerationMagnitude/2);
                //Set asteroid's point value based on it's level
                Spawner.SetAsteroidWorth(astDup); 

                duplicate.GetComponent<VectorMovement>().maxSpeed = asteroid.GetComponent<VectorMovement>().maxSpeed / 2;


                curAngle += (maxAngle * 2) / divisions;

            }
            Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        CollisionManager.UpdateAllColliders();
    }

}
