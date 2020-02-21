using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Player
{
    public enum Team
    {
        Blue,
        Orange,
        Referee
    }

    protected Team team;
    public GameObject playerObject;
    public float speed = 1.0f;

    public virtual void Initialize() { }

    public void Update()
    {
        MoveTowards(Direction());
    }

    private void MoveTowards(Vector3 direction)
    {
        var speedMultiplier = speed * Time.deltaTime;
        playerObject.transform.position += direction * speedMultiplier;
    }
    
    protected virtual Vector3 Direction()
    {
        return new Vector3();
    }

    protected void AssignTeamColor(Team team)
    {
        //tried using conditional ref expression because rider suggested it - it is weird/confusing, but cleaner.
        //var teamMat = (team == Team.Blue)
        //    ? Resources.Load<Material>("Materials/BlueMat")
        //    : Resources.Load<Material>("Materials/OrangeMat");
        Material teamMat;
        switch (team)
        {
            case Team.Blue:
                teamMat = Resources.Load<Material>("Materials/BlueMat");
                break;
            case Team.Orange:
                teamMat = Resources.Load<Material>("Materials/OrangeMat");
                break;
            case Team.Referee:
                teamMat = Resources.Load<Material>("Materials/RedMat");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var renderer = playerObject.GetComponent<Renderer>();
        renderer.material = teamMat;
    }

    public virtual void SetPosition()
    {
        playerObject.transform.position = new Vector3(Random.Range(-8, 8), playerObject.transform.position.y, Random.Range(-4, 4));
    }
}

public class UserPlayer : Player
{
    private readonly KeyCode[] _movementKeys = new KeyCode[4];

    protected override Vector3 Direction()
    {
        var direction = new Vector3();
        
        if (Input.GetKey(_movementKeys[0])) direction += playerObject.transform.forward;
        if (Input.GetKey(_movementKeys[1])) direction += -playerObject.transform.right;
        if (Input.GetKey(_movementKeys[2])) direction += -playerObject.transform.forward;
        if (Input.GetKey(_movementKeys[3])) direction += playerObject.transform.right;

        return direction;
    }

    public void AssignKeys(KeyCode up, KeyCode left, KeyCode down, KeyCode right)
    {
        _movementKeys[0] = up;
        _movementKeys[1] = left;
        _movementKeys[2] = down;
        _movementKeys[3] = right;
    }

    public UserPlayer(GameObject playerObjectGameObject, Team teamAssignment, float playerSpeed)
    {
        playerObject = playerObjectGameObject;
        team = teamAssignment;
        speed = playerSpeed;
        AssignTeamColor(team);
    }
}

public class AIPlayer : Player
{
    protected override Vector3 Direction()
    {
        var direction = ServicesLocator.Ball.transform.position - playerObject.transform.position;
        return direction.normalized;
    }

    public AIPlayer(GameObject playerObjectGameObject, Team teamAssignment, float speed)
    {
        playerObject = playerObjectGameObject;
        team = teamAssignment;
        this.speed = speed;
        AssignTeamColor(team);
    }
}

public class Referee : Player
{
    protected override Vector3 Direction()
    {
        var direction = ServicesLocator.Ball.transform.position - (playerObject.transform.position - Vector3.one);
        return direction.normalized;
    }
    
    public override void SetPosition()
    {
        playerObject.transform.position = new Vector3(0, playerObject.transform.position.y, 1);
    }

    public Referee(GameObject playerObjectGameObject, Team teamAssignment, float speed)
    {
        playerObject = playerObjectGameObject;
        team = teamAssignment;
        this.speed = speed;
        AssignTeamColor(team);
    }
}