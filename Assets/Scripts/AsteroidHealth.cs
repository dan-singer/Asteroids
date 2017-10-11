using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles health for enemies (asteroids, etc.)
/// </summary>
/// <author>Dan Singer</author>
public class AsteroidHealth : MonoBehaviour {

    public int health = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Die();
        }
	}

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

    private void Die()
    {
        Asteroid asteroid = GetComponent<Asteroid>();
        if (asteroid.level == 1)
        {
            float maxAngle = 45.0f;
            float curAngle = -maxAngle;
            int toSpawn = 2;
            int divisions = toSpawn - 1;
            float scale = 1 / (Mathf.Pow(2, asteroid.level));
            for (int i=0; i<toSpawn; i++)
            {
                GameObject duplicate = Instantiate<GameObject>(gameObject);
                Asteroid astDup = duplicate.GetComponent<Asteroid>();
                astDup.level = asteroid.level + 1;

                astDup.transform.localScale = new Vector3(scale, scale, scale);

                Vector3 dir = Quaternion.Euler(0, 0, curAngle) * asteroid.Direction;
                astDup.SetDirectionAndAcceleration(dir, asteroid.AccelerationMagnitude/2);
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
