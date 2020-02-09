using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoccerPlayer : MonoBehaviour
{
    public enum Team
    {
        Blue,
        Orange
    }

    public Team team;
    public float speed;

    void Update()
    {
        MoveTowards(Direction());
    }

    protected virtual Vector3 Direction()
    {
        return new Vector3();
    }

    private void MoveTowards(Vector3 direction)
    {
        var speedMultiplier = speed * Time.deltaTime;
        transform.position += direction * speedMultiplier;
    }
}
