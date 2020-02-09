using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        ServicesLocator.Ball = this;
    }
}
