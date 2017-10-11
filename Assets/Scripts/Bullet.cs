using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
