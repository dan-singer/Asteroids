using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Manager for all ui elements.
/// </summary>
/// <author>Dan Singer</author>
public class UIManager : MonoBehaviour {


    public Text score;
    public Text lives;

	// Use this for initialization
	void Start () {

        //Hook up event-handlers
        GameManager.Instance.LivesChanged += (int newLives) =>
        {
            lives.text = "Lives: " + newLives;
        };
        GameManager.Instance.ScoreChanged += (int newScore) =>
        {
            score.text = "Score: " + newScore;
        };
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
