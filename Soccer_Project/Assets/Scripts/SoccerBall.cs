using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBall : MonoBehaviour
{
    public GoalScored goalScored;
    // Start is called before the first frame update
    void Awake()
    {
        ServicesLocator.Ball = this;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal")) {
            goalScored = new GoalScored(other.name == "Blue_Goal");
            ServicesLocator.EventManager.Fire(goalScored);
        }
    }
}
