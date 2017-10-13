using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour which causes a sprite to flash
/// </summary>
/// <author>Dan Singer</author>
public class Flash : MonoBehaviour {

    public Color flashColor;
    public float hertz = 1;

    private float durationBetweenColorChange;
    private float prevTime;
    private SpriteRenderer spriteRend;
    private Color startColor;

	// Use this for initialization
	void OnEnable () {
        float period = 1 / hertz;
        durationBetweenColorChange = period / 2;
        prevTime = Time.time;
        spriteRend = GetComponent<SpriteRenderer>();

        startColor = spriteRend.color;
        spriteRend.color = flashColor;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > prevTime + durationBetweenColorChange)
        {
            spriteRend.color = spriteRend.color == startColor ? flashColor : startColor;
            prevTime = Time.time;
        }
	}

    private void OnDisable()
    {
        spriteRend.color = startColor;
    }
}
