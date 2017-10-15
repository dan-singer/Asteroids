using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// Manager for all ui elements.
/// </summary>
/// <author>Dan Singer</author>
public class UIManager : MonoBehaviour {


    public Text score;
    public Text lives;
    public Text powerup;
    public GameObject gameOverPanel;

    // Use this for initialization
    void Start() {

        //Hook up event-handlers
        GameManager.Instance.LivesChanged += (int newLives) =>
        {
            lives.text = "Lives: " + newLives;
        };
        GameManager.Instance.ScoreChanged += (int newScore) =>
        {
            score.text = "Score: " + newScore;
        };
        GameManager.Instance.PlayerGotGameover += () =>
        {
            gameOverPanel.SetActive(true);
        };
        GameManager.Instance.GetComponent<Powerups>().PowerupBegan += (Powerups.PowerupType type) =>
        {
            powerup.text = GameManager.CamelToSpaced(type.ToString()); 
        };
        GameManager.Instance.GetComponent<Powerups>().PowerupEnded += (Powerups.PowerupType type) =>
        {
            powerup.text = "";
        };
    }
	
	// Update is called once per frame
	void Update () {
		
	}



}
