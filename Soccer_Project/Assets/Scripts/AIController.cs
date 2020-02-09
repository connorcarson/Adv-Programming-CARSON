using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController
{
    private List<Player> _players;
    private int _numberOfPlayers = 11;
    public void Initialize()
    {
        _players = new List<Player>();
        GeneratePlayers();   
    }

    public void Update()
    {
        foreach (var player in _players)
        {
            player.Update();
        }
    }

    private void GeneratePlayers()
    {
        var userObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/AI_Player"));
        var userPlayer = new UserPlayer(userObject, Player.Team.Blue);
        userPlayer.AssignKeys(KeyCode.K, KeyCode.K, KeyCode.K, KeyCode.K);
        _players.Add(userPlayer);
        
        for (var i = 0; i < _numberOfPlayers - 1; i++)
        {
            var aiObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/AI_Player"));
            _players.Add(new AIPlayer(aiObject, Player.Team.Blue));
        }

        for (var i = 0; i < _numberOfPlayers; i++)
        {
            var aiObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/AI_Player"));
            _players.Add(new AIPlayer(aiObject, Player.Team.Orange));
        }
    }
}
