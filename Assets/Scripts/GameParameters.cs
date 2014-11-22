﻿using UnityEngine;
using System.Collections;

public class GameParameters : MonoBehaviour {

    public int playerLives;
    public int difficulty;
    private long playerScore;
    private const long maxScore = 999999999999L;
    public Vector3 lastCheckpoint;
    public float speedMultiplier = 1;

    public int endlessModeTurrets = 0;
    public bool endlessMode = false;

    /* parameters for HUD */
    public GUIText livesString;
    public GUIText scoreString;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        lastCheckpoint = Vector3.zero;

        difficulty = 1;
        playerScore = 0;
        playerLives = 5;

        UpdateScore(0);
        SetLivesText();
    }

    /* update the player's score - return true if score has changed */
    public bool UpdateScore(long points)
    {
        long oldScore = playerScore;
        playerScore += points;
        scoreString.text = playerScore.ToString(); // update the display string
        return (playerScore != oldScore);
    }

    /* return the player's score */
    public long getPlayerScore()
    {
        return playerScore;
    }

    public void SetLivesText()
    {
        livesString.text = playerLives.ToString();
    }

}
