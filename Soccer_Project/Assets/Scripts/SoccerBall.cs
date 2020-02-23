using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBall : MonoBehaviour
{
    public GoalScored goalScored;

    private Rigidbody _rb;

    void Awake()
    {
        ServicesLocator.Ball = this;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        ServicesLocator.EventManager.Register<GoalScored>(SetPosition);
    }

    private void OnDestroy()
    {
        ServicesLocator.EventManager.Unregister<GoalScored>(SetPosition);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal")) {
            goalScored = new GoalScored(other.name == "Blue_Goal");
            ServicesLocator.EventManager.Fire(goalScored);
        }
    }

    public void SetPosition(AGPEvent e)
    {
        transform.position = new Vector3(0, transform.position.y, 0);
        transform.rotation = Quaternion.identity;
        _rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            transform.SetParent(other.gameObject.transform);
        }
    }
}
