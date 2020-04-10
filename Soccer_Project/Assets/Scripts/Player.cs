using System;
using UnityEngine;
using Random = UnityEngine.Random;
using BehaviorTree;
using UnityEngine.UI;

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

    public virtual void Update()
    {
        //LookAtBall();
    }

    public void MoveTowards(Vector3 direction, float speed)
    {
        var speedMultiplier = speed * Time.deltaTime;
        playerObject.transform.position += direction * speedMultiplier;
    }
    
    public virtual Vector3 Direction()
    {
        return new Vector3();
    }

    public virtual Vector3 Direction(GameObject target)
    {
        return new Vector3();
    }
    
    private void LookAtBall()
    {
        var playerPos = playerObject.transform.position;
        //tried switching the order here to get the players to face the ball, but either way they faced the opposite direction
        var lookVector = (ServicesLocator.Ball.transform.position - playerPos);
        lookVector.x = playerPos.x;
        lookVector.z = playerPos.z;

        //don't understand why but making lookVector negative fixed the aforementioned issue
        var lookRotation = Quaternion.LookRotation(-lookVector);
        
        playerObject.transform.rotation = Quaternion.Slerp(playerObject.transform.rotation, lookRotation, 1);
    }

    protected void AssignTeamColor(Team team)
    {
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

    public override Vector3 Direction()
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

    public override void Update()
    {
        MoveTowards(Direction(), speed);
    }
}
public class AIPlayer : Player
{
    private BehaviorTree.Tree<AIPlayer> _tree;
    public float exhaustionTimer;
    public float timeToExhaustion = 5.0f;
    public float meanderTimer;
    public float timeToNewTarget = 3.0f;
    public bool hasBall;

    public override void Initialize()
    {
        var chaseBallTree = new Tree<AIPlayer>
        (
            new Sequence<AIPlayer>(
                new BallInRange(1.0f),
                new NotExhausted(),
                new ChaseBall(true)
            )
        );

        var approachGoalTree = new Tree<AIPlayer>
        (
            new Sequence<AIPlayer>(
                new HasBall(),
                new MoveTowardsGoal(true)
            )
        );

        var kickBallTree = new Tree<AIPlayer>
        (
            new Sequence<AIPlayer>(
                new HasBall(),
                new FacingGoal(),
                new KickBall(true)
            )
        );

        _tree = new Tree<AIPlayer>
        (
            new Selector<AIPlayer>(
                kickBallTree,
                approachGoalTree,
                chaseBallTree,
                new Meander()
            )
        );
    }

    public override void Update()
    {
        base.Update();
        _tree.Update(this);
    }

    public override Vector3 Direction(GameObject target)
    {
        var direction = target.transform.position - playerObject.transform.position;
        return direction.normalized;
    }

    private void LookAt(GameObject target)
    {
        var playerPos = playerObject.transform.position;
        //tried switching the order here to get the players to face the ball, but either way they faced the opposite direction
        var lookVector = (target.transform.position - playerPos);
        lookVector.x = playerPos.x;
        lookVector.z = playerPos.z;

        //don't understand why but making lookVector negative fixed the aforementioned issue
        var lookRotation = Quaternion.LookRotation(lookVector);
        
        playerObject.transform.rotation = Quaternion.Slerp(playerObject.transform.rotation, lookRotation, 1);
    }

    public void Meander()
    {
        var target = new Vector3(Random.Range(-8, 8), playerObject.transform.position.y, Random.Range(-4, 4));
        MoveTowards(target, 0.75f);
    }
    
    public void ChaseBall(bool chase)
    {
        if (!chase)
        {
            exhaustionTimer = 0.0f; 
            return;
        }
        
        LookAt(ServicesLocator.Ball.gameObject);
        MoveTowards(Direction(ServicesLocator.Ball.gameObject), 1.25f);
        exhaustionTimer += Time.deltaTime;
    }

    public GameObject GetGoal()
    {
        if (team == Team.Blue){ return ServicesLocator.ScoreController.blueGoal; }
        return ServicesLocator.ScoreController.orangeGoal;
    }
    public void MoveTowardsGoal(bool moveTowardsGoal)
    {
        if (!moveTowardsGoal) return;
        
        LookAt(GetGoal());
        MoveTowards(Direction(GetGoal()), 1.25f);
    }

    public void KickBall(bool kick, Vector3 direction, float power)
    {
        if (!kick) return;
        
        var rb = ServicesLocator.Ball.GetComponent<Rigidbody>();
        rb.AddForce(direction * power);
    }
    
    public AIPlayer(GameObject playerObjectGameObject, Team teamAssignment, float speed)
    {
        playerObject = playerObjectGameObject;
        team = teamAssignment;
        this.speed = speed;
        AssignTeamColor(team);
    }
}

public class Meander : BehaviorTree.Node<AIPlayer>
{
    public override bool Update(AIPlayer context)
    {
        Debug.Log("Player is aimlessly wandering around...");
        context.Meander();
        return true;
    }
}

public class FacingGoal : BehaviorTree.Node<AIPlayer>
{
    public override bool Update(AIPlayer context)
    {
        var ray = new Ray(context.playerObject.transform.position, context.playerObject.transform.forward);
        var hits = Physics.RaycastAll(ray, 100f);
        
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject == context.GetGoal())
            {
                Debug.Log("Player is facing the goal!");
                return true;
            }
        }

        return false;
    }
}

public class KickBall : BehaviorTree.Node<AIPlayer>
{
    private bool kick;
    
    public KickBall(bool kick)
    {
        this.kick = kick;
    }
    
    public override bool Update(AIPlayer context)
    {
        Debug.Log("Player is kicking the ball!");
        context.KickBall(kick, context.Direction(context.GetGoal()), 1.5f);
        return true;
    }
}

public class HasBall : BehaviorTree.Node<AIPlayer>
{
    public override bool Update(AIPlayer context)
    {
        Debug.Log("Player has the ball: " + ServicesLocator.Ball.transform.IsChildOf(context.playerObject.transform));
        return ServicesLocator.Ball.transform.IsChildOf(context.playerObject.transform);
    }
}

public class MoveTowardsGoal : BehaviorTree.Node<AIPlayer>
{
    private bool moveTowardsGoal;

    public MoveTowardsGoal(bool moveTowardsGoal)
    {
        this.moveTowardsGoal = moveTowardsGoal;
    }
    
    public override bool Update(AIPlayer context)
    {
        Debug.Log("Player is moving towards the goal!");
        context.MoveTowardsGoal(moveTowardsGoal);
        return true;
    }
}

public class BallInRange : BehaviorTree.Node<AIPlayer>
{
    private float range;

    public BallInRange(float range)
    {
        this.range = range;
    }
    
    public override bool Update(AIPlayer context)
    {
        var distance = Vector3.Distance(context.playerObject.transform.position, ServicesLocator.Ball.transform.position);
        Debug.Log("Ball in range: " + (distance < range));
        return distance < range;
    }
}

public class NotExhausted : BehaviorTree.Node<AIPlayer>
{
    public override bool Update(AIPlayer context)
    {
        Debug.Log("Player not exhausted: " + (context.exhaustionTimer < context.timeToExhaustion));
        return context.exhaustionTimer < context.timeToExhaustion;
    }
}
public class ChaseBall : BehaviorTree.Node<AIPlayer>
{ 
    private bool chase;

    public ChaseBall(bool chase)
    {
        this.chase = chase;
    }
    
    public override bool Update(AIPlayer context)
    {
        Debug.Log("Player is chasing the ball!");
        context.ChaseBall(chase);
        return true;
    }
}


public class Referee : Player
{
    private FiniteStateMachine<Referee> _RefereeStateMachine;
    public override void Initialize()
    {
        SetPosition();
        _RefereeStateMachine = new FiniteStateMachine<Referee>(this);
        _RefereeStateMachine.TransitionTo<WatchBall>();
    }

    public override void Update()
    {
        base.Update();
        _RefereeStateMachine.Update();
    }

    public override Vector3 Direction()
    {
        var direction = ServicesLocator.Ball.transform.position - (playerObject.transform.position - Vector3.one);
        return direction.normalized;
    }

    public override void SetPosition()
    {
        playerObject.transform.position = new Vector3(0, playerObject.transform.position.y, 2);
    }

    public Referee(GameObject playerObjectGameObject, Team teamAssignment, float speed)
    {
        playerObject = playerObjectGameObject;
        team = teamAssignment;
        this.speed = speed;
        AssignTeamColor(team);
    }
}

public class RefereeState : FiniteStateMachine<Referee>.State
{
    public override void OnEnter()
    {
        
    }

    public override void Update()
    {
        
    }
}

public class WatchBall : RefereeState
{
    public override void OnEnter()
    {
        
    }

    public override void Update()
    {
        Context.MoveTowards(Context.Direction(), Context.speed);
    }
}

public class BlowWhistle : RefereeState
{
    public override void OnEnter()
    {
        ServicesLocator.AudioManager.Whistle();
    }

    public override void Update()
    {
        foreach (var player in ServicesLocator.PlayerManager._players)
        {
            player.MoveTowards(-player.Direction(), 1.5f);
        }
    }
}