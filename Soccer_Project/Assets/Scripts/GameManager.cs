using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        ServicesLocator.GameManager = this;
        
        ServicesLocator.EventManager = new EventManager();
        
        ServicesLocator.PlayerManager = new PlayerController();
        ServicesLocator.PlayerManager.Initialize();
        
        ServicesLocator.BoundaryController = new Boundaries();
        ServicesLocator.BoundaryController.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        ServicesLocator.PlayerManager.Update();
    }

    private void LateUpdate()
    {
        ServicesLocator.BoundaryController.Update();
    }
}
