using UnityEngine;

public abstract class Player
{
    public enum Team
    {
        Blue,
        Orange
    }

    protected Team team;
    protected GameObject player;
    private const float speed = 1.0f;

    public void Update()
    {
        MoveTowards(Direction());
    }

    private void MoveTowards(Vector3 direction)
    {
        var speedMultiplier = speed * Time.deltaTime;
        player.transform.position += direction * speedMultiplier;
    }
    
    protected virtual Vector3 Direction()
    {
        return new Vector3();
    }

    protected void AssignTeamColor(Team team)
    {
        //tried using conditional ref expression because rider suggested it - it is weird/confusing, but cleaner.
        var teamMat = (team == Team.Blue)
            ? Resources.Load<Material>("Materials/BlueMat")
            : Resources.Load<Material>("Materials/OrangeMat");

        var renderer = player.GetComponent<Renderer>();
        renderer.material = teamMat;
    }
}

public class UserPlayer : Player
{
    private readonly KeyCode[] _movementKeys = new KeyCode[4];

    protected override Vector3 Direction()
    {
        var direction = new Vector3();
        
        if (Input.GetKey(_movementKeys[0])) direction += player.transform.forward;
        if (Input.GetKey(_movementKeys[1])) direction += -player.transform.right;
        if (Input.GetKey(_movementKeys[2])) direction += -player.transform.forward;
        if (Input.GetKey(_movementKeys[3])) direction += player.transform.right;

        return direction;
    }

    public void AssignKeys(KeyCode up, KeyCode left, KeyCode down, KeyCode right)
    {
        _movementKeys[0] = up;
        _movementKeys[1] = left;
        _movementKeys[2] = down;
        _movementKeys[3] = right;
    }

    public UserPlayer(GameObject playerGameObject, Team teamAssignment)
    {
        player = playerGameObject;
        team = teamAssignment;
        AssignTeamColor(team);
    }
}

public class AIPlayer : Player
{ 
    protected override Vector3 Direction()
    {
        return ServicesLocator.Ball.transform.position - player.transform.position;
    }

    public AIPlayer(GameObject playerGameObject, Team teamAssignment)
    {
        player = playerGameObject;
        team = teamAssignment;
        AssignTeamColor(team);
    }
}

