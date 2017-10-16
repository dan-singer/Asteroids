using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Singleton behaviour which manages audio.
/// </summary>
/// <author>Dan Singer</author>
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour {

    public AudioClip audDie, audFire, audGameOver, audHit, audHover, audOK;

    private AudioSource audioSource;

    private static AudioManager instance;
    /// <summary>
    /// Singleton pattern.
    /// </summary>
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
            }
            return instance;
        }
    }
    /// <summary>
    /// Initialize and subscribe to events.
    /// </summary>
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);


        //Whenever we load a new scene...
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode lsm) =>
        {
            //Subscribe to various GameManager events and play audio based on them.
            GameManager.Instance.NewPlayer += (GameObject player) =>
            {
                player.GetComponent<Gun>().BulletFired += () => PlayClip(audFire);
            };
            GameManager.Instance.ScoreChanged += (int score) =>
            {
                if (score != 0)
                    PlayClip(audHit);
            };
            GameManager.Instance.PlayerDied += () => PlayClip(audDie);
            GameManager.Instance.PlayerGotGameover += () => PlayClip(audGameOver);
            GameManager.Instance.GetComponent<Powerups>().PowerupBegan += (Powerups.PowerupType type) => PlayClip(audHover);
            GameManager.Instance.GetComponent<Powerups>().PowerupEnded += (Powerups.PowerupType type) => PlayClip(audOK);

        };
        

    }

    /// <summary>
    /// Play the audio clip.
    /// </summary>
    public void PlayClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip, audioSource.volume);
    }


}
