using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    public List<Player> _players;
    private Player _referee;
    private int _numberOfPlayers = 2;
    public void Initialize(AGPEvent e)
    {
        _players = new List<Player>();
        GeneratePlayers();
        _referee.Initialize();
        ServicesLocator.EventManager.Register<GoalScored>(ResetPlayers);
        
        //foreach (var player in _players)
        //{
        //    player.Initialize();
        //}
    }

    public void Update()
    {
        foreach (var player in _players)
        {
            player.Update();
        }
        _referee.Update();
    }

    public void OnDestroy()
    {
        ServicesLocator.EventManager.Unregister<GoalScored>(ResetPlayers);
    }

    private void GeneratePlayers()
    {
        var userObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        
        var userPlayer = new UserPlayer(userObject, Player.Team.Blue, 1.5f);
        userPlayer.SetPosition();
        userPlayer.AssignKeys(KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D);
        _players.Add(userPlayer);
        
        for (var i = 0; i < _numberOfPlayers - 1; i++)
        {
            var aiObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            
            var aiPlayer = new AIPlayer(aiObject, Player.Team.Blue, 0.8f);
            aiPlayer.SetPosition();
            _players.Add(aiPlayer);
        }

        for (var i = 0; i < _numberOfPlayers; i++)
        {
            var aiObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));

            var aiPlayer = new AIPlayer(aiObject, Player.Team.Orange, 0.8f);
            aiPlayer.SetPosition();
            _players.Add(aiPlayer);
        }

        var refereeObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        
        _referee = new Referee(refereeObject, Player.Team.Referee, 1.5f);
        //refereePlayer.SetPosition();
    }

    private void ResetPlayers(AGPEvent e)
    {
        foreach (var player in _players)
        {
            player.SetPosition();
        }
        
        _referee.SetPosition();
    }
}
