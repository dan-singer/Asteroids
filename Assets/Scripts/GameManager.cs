using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager class for entire game.
/// </summary>
/// <summary>Dan Singer</summary>
public class GameManager : MonoBehaviour {


    //Singleton design pattern since there will only be one GameManager
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    public int lives = 3;

    public int secondsBeforeRespawn = 3;

    public GameObject playerPrefab;

    public Camera mainCamera;

    private GameObject playerInstance;

	// Use this for initialization
	void Start () {

        playerInstance = SpawnPlayer();        
	}


    /// <summary>
    /// Spawns the player in bottom center of screen, and returns a reference to it.
    /// </summary>
    private GameObject SpawnPlayer()
    {
        GameObject player = Instantiate<GameObject>(playerPrefab,
            new Vector3(0, mainCamera.transform.position.y - .8f * mainCamera.GetComponent<Camera>().orthographicSize),
            Quaternion.Euler(0, 0, 90));
       
        CollisionManager.UpdateAllColliders();
        //Subscribe to player's death event
        player.GetComponent<PlayerHealth>().DeathEvent += HandlePlayerDeath;
        return player;
    }

	
	// Update is called once per frame
	void Update () {
		
	}


    /// <summary>
    /// Kill the player, wait a few seconds, then either respawn the player or show the game over screen.
    /// </summary>
    private void HandlePlayerDeath()
    {
        Destroy(playerInstance);
        lives--;
        if (lives > 0)
            StartCoroutine(DeathDelay());
    }
    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(secondsBeforeRespawn);
        playerInstance = SpawnPlayer();
    }
}
