﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains info on each of the collders as well as optimization methods for collision detection.
/// </summary>
/// <author>Dan Singer</author>
public class CollisionManager : MonoBehaviour {


    /// <summary>
    /// Singleton pattern for all of the colliders
    /// </summary>
    public static Collider[] AllColliders { get; private set; }

    /// <summary>
    /// Graph representing collision checks this frame
    /// </summary>
    public static Dictionary<Collider, HashSet<Collider>> CollisionChecksThisFrame { get; private set; }


    private static bool readyToUpdateColliders = false;

    /// <summary>
    /// Update the list of all the colliders.
    /// Be sure to call this when instanitating or destroying anything.
    /// </summary>
    public static void UpdateAllColliders()
    {
        readyToUpdateColliders = true;
    }


    /// <summary>
    /// Inform the manager that a collision check has occured between object A and B.
    /// </summary>
    public static void ReportCollisionCheckThisFrame(Collider A, Collider B)
    {
        if (!CollisionChecksThisFrame.ContainsKey(A))
        {
            CollisionChecksThisFrame[A] = new HashSet<Collider>() { B };
        }
        else
        {
            if (!CollisionChecksThisFrame[A].Contains(B))
                CollisionChecksThisFrame[A].Add(B);
        }
        if (!CollisionChecksThisFrame.ContainsKey(B))
        {
            CollisionChecksThisFrame[B] = new HashSet<Collider>() { A };
        }
        else
        {
            if (!CollisionChecksThisFrame[B].Contains(A))
                CollisionChecksThisFrame[B].Add(A);
        }
    }


    /// <summary>
    /// Checks to make sure that this pairing of objects has not already been checked this frame.
    /// </summary>
    public static bool WasCollCheckAlreadyPerformed(Collider A, Collider B)
    {
        bool occured = false;
        if (CollisionChecksThisFrame.ContainsKey(A))
        {
            if (CollisionChecksThisFrame[A].Contains(B))
                occured = true;
        }
        return occured;
    }

    void Start () {
        AllColliders = Object.FindObjectsOfType<Collider>();
        CollisionChecksThisFrame = new Dictionary<Collider, HashSet<Collider>>();
    }
	
	void LateUpdate () {
        //Since colliders will be adding to this in the update, we'll use this manager object to clear them after the update
        CollisionChecksThisFrame.Clear();

        if (readyToUpdateColliders)
        {
            AllColliders = Object.FindObjectsOfType<Collider>();
            readyToUpdateColliders = false;
        }
    }
}
