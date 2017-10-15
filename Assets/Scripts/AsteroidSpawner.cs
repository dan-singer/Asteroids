using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behaviour responsible for spawning asteroids into the scene.
/// </summary>
/// <author>Dan Singer</author>
public class AsteroidSpawner : MonoBehaviour {

    public GameObject asteroidPrefab;
    public Camera mainCamera;

    public int[] asteroidWorth = { 20, 50 };

    public int asteroidsToSpawnAtStart = 3;
    public int startingSpawnDeviation = 1;

    public float meanDurationBetweenSpawns = 4;
    public float durationBetweenSpawnsStdDev = 1;

    public int maxAsteroidLevel = 2;

    public int scoreRequiredToIncreaseAsteroidLevel = 500;

    /// <summary>
    /// Pool of asteroid sprites to randomly pick from when spawning
    /// </summary>
    public Sprite[] asteroidSpritePool;

    private float lastTimeSpawned;
    private float durationBetweenSpawns;
    private int timesLevelIncreased = 0;

	// Use this for initializaton
	void Start () {
        if (!mainCamera)
            mainCamera = Camera.main;

        lastTimeSpawned = Time.time;
        durationBetweenSpawns = RandomUtils.Gaussian(meanDurationBetweenSpawns, durationBetweenSpawnsStdDev);

        //Spawn some asteroids at the start
        int toSpawn = asteroidsToSpawnAtStart + Random.Range(-startingSpawnDeviation, startingSpawnDeviation + 1);
        for (int i = 0; i < toSpawn; i++)
            Spawn();

        //When score gets bigger than a multiple, allow future asteroids to further subdivide.
        GameManager.Instance.ScoreChanged += (int score) =>
        {
            if (maxAsteroidLevel >= asteroidWorth.Length)
                return;
            if (score > scoreRequiredToIncreaseAsteroidLevel * (timesLevelIncreased+1))
            {
                maxAsteroidLevel++;
                timesLevelIncreased++;
            }
        };
	}
	
	// Update is called once per frame
	void Update () {

        if (Time.time > lastTimeSpawned + durationBetweenSpawns)
        {
            Spawn();
            durationBetweenSpawns = RandomUtils.Gaussian(meanDurationBetweenSpawns, durationBetweenSpawnsStdDev);
            lastTimeSpawned = Time.time;
        }
    }

    /// <summary>
    /// Spawn an asteroid randomly along the edges of the screen.
    /// </summary>
    private void Spawn()
    {
        Sprite asteroidSprite = asteroidSpritePool[Random.Range(0, asteroidSpritePool.Length)];

        float vertExtents = mainCamera.orthographicSize;
        float horzExtents = mainCamera.orthographicSize * mainCamera.aspect;
        Vector3 extents = new Vector3(horzExtents, vertExtents, 0);
        extents = new Vector3(extents.x + asteroidSprite.bounds.extents.x, extents.y + asteroidSprite.bounds.extents.y, 0);
        //Figure our where the corners of the screen are
        Vector3 bl = mainCamera.transform.position - extents, br = mainCamera.transform.position + extents - new Vector3(0, extents.y * 2, 0);
        Vector3 ul = mainCamera.transform.position - extents + new Vector3(0, extents.y * 2, 0), ur = mainCamera.transform.position + extents;

        //Generate four "lines" representing possible spawn points
        Vector3[,] lines = new Vector3[4,2]{
            { bl, ul},
            { bl, br},
            { br, ur},
            { ul, ur}
        };

        //Now, choose one randomly
        int rand = Random.Range(0, lines.GetLength(0));
        Vector3 left = lines[rand, 0], right = lines[rand, 1];

        //Linearly interpolate to a random spot on the line
        float randSpot = Random.Range(0, 1.0f);

        Vector3 spawnPos = Vector3.Lerp(left, right, randSpot);
        spawnPos.z = 0;

        //Finally, spawn the asteroid
        GameObject asteroidGo = Instantiate<GameObject>(asteroidPrefab, spawnPos, Quaternion.identity);
        asteroidGo.GetComponent<SpriteRenderer>().sprite = asteroidSprite;
        Asteroid asteroid = asteroidGo.GetComponent<Asteroid>();
        SetAsteroidWorth(asteroid);
        asteroidGo.GetComponent<AsteroidHealth>().Spawner = this;

        CollisionManager.UpdateAllColliders();
    }

    public void SetAsteroidWorth(Asteroid asteroid)
    {
        asteroid.Value = asteroidWorth[asteroid.level - 1];
    }
}
