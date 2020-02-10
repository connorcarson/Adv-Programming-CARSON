using UnityEngine;

public abstract class Player
{
    public enum Team
    {
        Blue,
        Orange
    }

    protected Team team;
    public GameObject playerObject;
    private const float speed = 1.0f;

    public virtual void Initialize() { }

    public virtual void Update()
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
        var teamMat = (team == Team.Blue)
            ? Resources.Load<Material>("Materials/BlueMat")
            : Resources.Load<Material>("Materials/OrangeMat");

        var renderer = playerObject.GetComponent<Renderer>();
        renderer.material = teamMat;
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

    public UserPlayer(GameObject playerObjectGameObject, Team teamAssignment)
    {
        playerObject = playerObjectGameObject;
        team = teamAssignment;
        AssignTeamColor(team);
    }
}

public class AIPlayer : Player
{
    //I don't think this goes here
    //private FiniteStateMachine<AIPlayer> _AIPlayerStateMachine;
    protected override Vector3 Direction()
    {
        var direction = ServicesLocator.Ball.transform.position - playerObject.transform.position;
        return direction.normalized;
    }

    //Attempted to utilize state machine here
    //public override void Initialize()
    //{
    //    _AIPlayerStateMachine.TransitionTo<FiniteStateMachine<AIPlayer>.ChaseBall>();
    //}
    
    //public override void Update()
    //{
    //    base.Update();
    //    _AIPlayerStateMachine.Update();
    
    //    var distance = (ServicesLocator.Ball.transform.position - playerObject.transform.position).magnitude;
    //    if(distance < 0.1f) _AIPlayerStateMachine.TransitionTo<FiniteStateMachine<AIPlayer>.HasBall>();
    //}

    public AIPlayer(GameObject playerObjectGameObject, Team teamAssignment)
    {
        playerObject = playerObjectGameObject;
        team = teamAssignment;
        AssignTeamColor(team);
        //_AIPlayerStateMachine = new FiniteStateMachine<AIPlayer>(this);
    }
}

