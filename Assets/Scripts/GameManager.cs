using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

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

    public int startingLives = 3;
    public int secondsBeforeRespawn = 3;
    public GameObject playerPrefab;
    public Camera mainCamera;

    public GameObject PlayerInstance { get; private set; }

    //Events
    public event Action<int> LivesChanged;
    public event Action<int> ScoreChanged;
    public event Action PlayerDied;
    public event Action PlayerGotGameover;

    private int lives;
    public int Lives
    {
        get
        {
            return lives;
        }
        set
        {
            lives = value;
            if (LivesChanged != null)
                LivesChanged(lives);
        }
    }

    private int score;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            if (ScoreChanged != null)
                ScoreChanged(score);
        }
    }



    // Use this for initialization
    void Start () {

        if (SceneManager.GetActiveScene().name != "main")
            return;

        PlayerInstance = SpawnPlayer();
        //Give player some invincibility 
        GetComponent<Powerups>().ActivatePowerup(Powerups.PowerupType.Invincibility);

        Lives = startingLives;
        Score = 0; 
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
        Destroy(PlayerInstance);
        Lives--;
        if (PlayerDied != null)
            PlayerDied();
        if (Lives > 0)
            StartCoroutine(DeathDelay());
        else
        {
            if (PlayerGotGameover != null)
                PlayerGotGameover();
        }
    }
    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(secondsBeforeRespawn);
        PlayerInstance = SpawnPlayer();
        //Give player some invincibility 
        GetComponent<Powerups>().ActivatePowerup(Powerups.PowerupType.Invincibility);
    }


    //Canvas Methods
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void Quit()
    {
        Application.Quit();
    }

    //Static Utility Methods
    /// <summary>
    /// Provided a string in camelCase, return it with a space in between
    /// </summary>
    public static string CamelToSpaced(string camel)
    {
        string spaced = camel;
        int offset = 0;
        for (int i = 1; i < camel.Length; i++)
        {
            if (Char.IsUpper(camel[i])) {
                spaced = spaced.Insert(i + offset, " ");
                offset++;
            }
        }
        return spaced;
    }
}
