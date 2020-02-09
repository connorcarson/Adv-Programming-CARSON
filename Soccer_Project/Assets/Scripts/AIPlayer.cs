using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : SoccerPlayer
{ 
    protected override Vector3 Direction()
    {
        return ServicesLocator.Ball.transform.position - transform.position;
    }
}
