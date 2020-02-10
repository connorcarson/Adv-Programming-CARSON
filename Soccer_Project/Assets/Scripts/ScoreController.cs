using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public TextMeshProUGUI blueScoreTMP;
    public TextMeshProUGUI orangeScoreTMP;
    
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
    }

    private void OnDestroy()
    {
        ServicesLocator.EventManager.Unregister<GoalScored>(e => IncrementScore(ServicesLocator.Ball.goalScored.blueTeam));
    }

    private void UpdateScoreUI()
    {
        blueScoreTMP.text = "blue: " + _blueTeamScore;
        orangeScoreTMP.text = "orange: " + _orangeTeamScore;
    }
    
    private void IncrementScore(bool blueTeam)
    {
        if (blueTeam) _blueTeamScore++;
        else _orangeTeamScore++;
    }
}
