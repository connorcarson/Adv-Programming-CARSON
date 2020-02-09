using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    public List<Player> _players;
    private int _numberOfPlayers = 2;
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
        var userObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        userObject.transform.position = new Vector3(Random.Range(-8, 8), userObject.transform.position.y,Random.Range(-4, 4));
        var userPlayer = new UserPlayer(userObject, Player.Team.Blue);
        userPlayer.AssignKeys(KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D);
        _players.Add(userPlayer);
        
        for (var i = 0; i < _numberOfPlayers - 1; i++)
        {
            var aiObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            aiObject.transform.position = new Vector3(Random.Range(-8, 8), aiObject.transform.position.y, Random.Range(-4, 4));
            _players.Add(new AIPlayer(aiObject, Player.Team.Blue));
        }

        for (var i = 0; i < _numberOfPlayers; i++)
        {
            var aiObject = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
            aiObject.transform.position = new Vector3(Random.Range(-8, 8), aiObject.transform.position.y, Random.Range(-4, 4));
            _players.Add(new AIPlayer(aiObject, Player.Team.Orange));
        }
    }
}
