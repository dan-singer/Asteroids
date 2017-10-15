using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; //We're using System, so clear up the ambiguous ref

/// <summary>
/// Spawns Powerup Pickups throughout the game
/// </summary>
public class PowerupSpawner : MonoBehaviour {

    public float minSpawnDuration, maxSpawnDuration;
    public GameObject powerupPrefab;
    public Camera mainCamera;

    private float prevTime;
    private float spawnDuration;

    private Dictionary<Powerups.PowerupType, Color> powerupColors;


	// Use this for initialization
	void Start () {
        prevTime = Time.time;
        spawnDuration = Random.Range(minSpawnDuration, maxSpawnDuration);
        //Create a mapping of the poweruptype to the color the pickup will be
        powerupColors = new Dictionary<Powerups.PowerupType, Color>()
        {
            {Powerups.PowerupType.RapidFire, Color.red },
            {Powerups.PowerupType.Invincibility, Color.blue }
        };
	}
	
	// Update is called once per frame
	void Update () {
        
		if (Time.time > prevTime + spawnDuration)
        {
            Spawn();
            spawnDuration = Random.Range(minSpawnDuration, maxSpawnDuration);
            prevTime = Time.time;
        }
	}

    private void Spawn()
    {
        SpriteRenderer pRend = powerupPrefab.GetComponent<SpriteRenderer>();
        float vertExtents = mainCamera.orthographicSize;
        float horzExtents = mainCamera.orthographicSize * mainCamera.aspect;
        Vector3 extents = new Vector3(horzExtents, vertExtents, 0);
        extents = new Vector3(extents.x - pRend.bounds.extents.x, extents.y - pRend.bounds.extents.y, 0);
        //Figure our where the corners of the screen are
        float minX = mainCamera.transform.position.x - extents.x;
        float maxX = mainCamera.transform.position.x + extents.x;
        float minY = mainCamera.transform.position.y - extents.y;
        float maxY = mainCamera.transform.position.y + extents.y;
        //Use that to figure out our spawn point
        Vector3 spawnPt = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));
        //Now spawn the powerup
        GameObject powerup = Instantiate<GameObject>(powerupPrefab, spawnPt, Quaternion.identity);
        //Figure out the powerup type
        int numTypes = Enum.GetNames(typeof(Powerups.PowerupType)).Length;
        Powerups.PowerupType type = (Powerups.PowerupType)Random.Range(0, numTypes);
        //Figure out the color
        Color powerColor = powerupColors[type];
        //Assign the values
        powerup.GetComponent<Powerup>().powerupType = type;
        powerup.GetComponent<SpriteRenderer>().color = powerColor;

    }
}
