using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private int blueTeamScore;
    private int orangeTeamScore;

    private void Start()
    {
        ServicesLocator.EventManager.Register<GoalScored>(e => IncrementScore(ServicesLocator.Ball.goalScored, ServicesLocator.Ball.goalScored.blueTeam));
    }

    private void Update()
    {
        Debug.Log(blueTeamScore);
        Debug.Log(orangeTeamScore);
    }

    private void IncrementScore(AGPEvent e, bool blueTeam)
    {
        if (blueTeam) blueTeamScore++;
        else orangeTeamScore++;
    }
}
