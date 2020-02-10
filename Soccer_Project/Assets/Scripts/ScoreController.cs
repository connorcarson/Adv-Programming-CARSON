using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private int _score;

    private void Start()
    {
        ServicesLocator.EventManager.Register<GoalScored>(IncrementScore);
    }

    private void Update()
    {
        Debug.Log(_score);
    }

    private void IncrementScore(AGPEvent e)
    {
        _score++;
    }
}
