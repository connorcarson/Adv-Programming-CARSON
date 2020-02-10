using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        ServicesLocator.GameManager = this;
        
        ServicesLocator.EventManager = new EventManager();
        
        ServicesLocator.PlayerManager = new PlayerController();

        ServicesLocator.BoundaryController = new Boundaries();
    }

    private void Start()
    {
        ServicesLocator.PlayerManager.Initialize();
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
