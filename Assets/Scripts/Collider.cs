using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a collider that can be used for collision detection
/// </summary>
/// <author>Dan Singer</author>
[RequireComponent(typeof(SpriteRenderer))]
public class Collider : MonoBehaviour {

    public enum Method
    {
        AABB,
        BoundingCircles
    }

    public Method collisionMethod;

    public SpriteRenderer SpriteRenderer { get; private set; }

    private HashSet<Collider> collidingWith;

    /// <summary>
    /// This will be broadcast when a collision occured.
    /// </summary>
    private static string collisionMessage = "CollisionOccurring";
    private static string collisionStartedMessage = "CollisionStarted";
    private static string collisionEndedMessage = "CollisionEnded";

    public float Radius
    {
        get
        {
            return Mathf.Max(SpriteRenderer.bounds.extents.x, SpriteRenderer.bounds.extents.y);
        }
    }

    // Use this for initialization
    void Start() {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        collidingWith = new HashSet<Collider>();
    }

    // Update is called once per frame
    void Update() {

        foreach (Collider coll in CollisionManager.AllColliders)
        {
            bool shouldNotCheck = coll == this || CollisionManager.WasCollCheckAlreadyPerformed(this, coll);
            if (shouldNotCheck)
                continue;

            bool collided;
            if (collisionMethod == Method.AABB)
                collided = AABB(coll);
            else
                collided = BoundingCircle(coll);

            CollisionManager.ReportCollisionCheckThisFrame(this, coll);

            if (collided)
            {
                //Collision began
                if (!collidingWith.Contains(coll))
                {
                    collidingWith.Add(coll);
                    BroadcastCollisionMessage(collisionStartedMessage, coll);
                }
                //Collision occuring
                BroadcastCollisionMessage(collisionMessage,coll);
            }
            else
            {
                //Collision just ended
                if (collidingWith.Contains(coll))
                {
                    collidingWith.Remove(coll);
                    BroadcastCollisionMessage(collisionEndedMessage, coll);
                }
            }
        }
    }

    private void BroadcastCollisionMessage(string msg, Collider other)
    {
        BroadcastMessage(msg, other, SendMessageOptions.DontRequireReceiver);
        other.BroadcastMessage(msg, this, SendMessageOptions.DontRequireReceiver);

    }

    /// <summary>
    /// See if there is a collision using Axis Aligned Bounding Box Collision Test
    /// </summary>
    /// <param name="other">Other collider to check against</param>
    /// <returns>True if collision, false otherwise</returns>
    private bool AABB(Collider other)
    {
        bool test = SpriteRenderer.bounds.max.x > other.SpriteRenderer.bounds.min.x
            && SpriteRenderer.bounds.min.x < other.SpriteRenderer.bounds.max.x
            && SpriteRenderer.bounds.max.y > other.SpriteRenderer.bounds.min.y
            && SpriteRenderer.bounds.min.y < other.SpriteRenderer.bounds.max.y;

        return test;
    }
    /// <summary>
    /// See if there is a collision using Bounding Circle Collision Test
    /// </summary>
    /// <param name="other">Other collider to check against</param>
    /// <returns>True if collision, false otherwise</returns>
    private bool BoundingCircle(Collider other)
    {
        Vector3 line = other.SpriteRenderer.bounds.center - SpriteRenderer.bounds.center;
        float radSum = Radius + other.Radius;
        bool test = line.sqrMagnitude < Mathf.Pow(radSum, 2);
        return test;
    }
}
