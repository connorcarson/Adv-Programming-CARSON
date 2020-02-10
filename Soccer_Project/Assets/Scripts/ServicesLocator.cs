using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServicesLocator
{
    public static FiniteStateMachine<AIPlayer> AIPlayerStateMachine;
    public static SoccerBall Ball;
    public static Boundaries BoundaryController;
    public static EventManager EventManager;
    public static GameManager GameManager;
    public static PlayerController PlayerManager;
    public static ScoreController ScoreController;
}
