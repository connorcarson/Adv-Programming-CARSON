using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SoccerPlayer
{
    public KeyCode[] movementKeys;
    
    protected override Vector3 Direction()
    {
        var direction = new Vector3();

        if(Input.GetKey(KeyCode.W)) direction += transform.forward;
        if(Input.GetKey(KeyCode.A)) direction += -transform.right;
        if(Input.GetKey(KeyCode.S)) direction += -transform.forward;
        if(Input.GetKey(KeyCode.D)) direction += transform.right;

        return direction;
    }
}