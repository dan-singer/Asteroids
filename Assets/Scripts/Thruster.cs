using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the thrusters on the ship.
/// </summary>
/// <author>Dan Singer</author>
/// <note>Attempt was made at individual rocket scaling, but not completed. See below in block comments. </note>
public class Thruster : MonoBehaviour {

    private float startTime;

    private Vector3 initScale;

    private SpriteRenderer[] rockets;

	// Use this for initialization
	void Start () {
    }

    /// <summary>
    /// Initialize values and enable rocket renderers.
    /// </summary>
    private void OnEnable()
    {
        initScale = Vector3.one;

        if (rockets == null)
        {
            rockets = GetComponentsInChildren<SpriteRenderer>();
        }

        foreach (SpriteRenderer renderer in rockets)
        {
            renderer.enabled = true;
        }
        transform.localScale = initScale;
        startTime = Time.time;

    }

    /// <summary>
    /// Disable rocket renderers
    /// </summary>
    private void OnDisable()
    {
        foreach (SpriteRenderer renderer in rockets)
        {
            renderer.enabled = false;
        }
    }

    /// <summary>
    /// Use sine to create a flaming trail effect. Requires rocket squares to be attached as children.
    /// </summary>
    private void FlameEffect()
    {
        float time = Time.time - startTime;
        float xScalar = (.5f * Mathf.Sin(time * 4)) + 1;
        transform.localScale = new Vector3(initScale.x * xScalar, initScale.y, initScale.z);
    }

    void Update()
    {
        //Although this is called each frame, it won't be called when this is disabled, which is often.
        FlameEffect();
    }


    /*Attempt at scaling from pivot
    /// <summary>
    /// Make the rockets act like flames.
    /// </summary>
    private void FlameEffect()
    {

        float time = Time.time - startTime;
        float xScalar = (.5f * Mathf.Sin(time*4)) + 1;
        for (int i=0; i<rockets.Length; i++)
        {
            Vector3 oldScale = initRocketScales[i];
            Vector3 newScale = new Vector3(oldScale.x * xScalar, oldScale.y, oldScale.z);
            float width = Mathf.Max(rockets[i].bounds.max.x, rockets[i].bounds.max.y);
            Vector3 localPivot = new Vector3(0.5f, 0, 0);
            Vector3 globalPivot = rockets[i].transform.TransformPoint(localPivot);
            Debug.DrawLine(rockets[i].transform.position, globalPivot);
            ScaleFromPivot(newScale, rockets[i].transform, globalPivot);
        }
    }

    /// <summary>
    /// Scale transform relative to a pivot point to newLocalScale
    /// </summary>
    private void ScaleFromPivot(Vector3 newLocalScale, Transform transf, Vector3 pivot)
    {
        Vector3 prevLocalScale = transf.localScale;
        Vector3 localPos = transf.transform.position - pivot;
        transf.localScale = newLocalScale;

        Vector3 multipliers = new Vector3(transf.localScale.x / prevLocalScale.x, transf.localScale.y / prevLocalScale.y, transf.localScale.z / prevLocalScale.z);
        localPos = new Vector3(localPos.x * multipliers.x, localPos.y * multipliers.y, localPos.z * multipliers.z);
        transf.position = pivot + localPos;
    }
    */



}
