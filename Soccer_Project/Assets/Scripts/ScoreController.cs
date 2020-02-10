using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public TextMeshProUGUI blueScoreTMP;
    public TextMeshProUGUI orangeScoreTMP;

    private int _scoreGoal = 2;
    private int _blueTeamScore;
    private int _orangeTeamScore;

    private void Start()
    {
        ServicesLocator.ScoreController = this;
        
        ServicesLocator.EventManager.Register<GoalScored>(e => IncrementScore(ServicesLocator.Ball.goalScored.blueTeam));
    }

    private void Update()
    {
        UpdateScoreUI();
        if(WinScore()) ServicesLocator.EventManager.Fire(new GameWon());
    }

    private void OnDestroy()
    {
        ServicesLocator.EventManager.Unregister<GoalScored>(e => IncrementScore(ServicesLocator.Ball.goalScored.blueTeam));
    }

    private void UpdateScoreUI()
    {
        blueScoreTMP.text = _blueTeamScore.ToString();
        orangeScoreTMP.text = _orangeTeamScore.ToString();
    }
    
    private void IncrementScore(bool blueTeam)
    {
        if (blueTeam) _blueTeamScore++;
        else _orangeTeamScore++;
    }

    private bool WinScore()
    {
        return _blueTeamScore >= _scoreGoal || _orangeTeamScore >= _scoreGoal;
    }
}
