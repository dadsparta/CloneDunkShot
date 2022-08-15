using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    public static ScoreHandler instance;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateScore()
    {
        ScoreStateDatabase.Score++;
    }

    public void OnGameOver()
    {
        
    }
}
