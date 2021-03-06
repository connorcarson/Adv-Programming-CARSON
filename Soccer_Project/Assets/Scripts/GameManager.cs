﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject gameOverScreen;

    private bool _gameBegun;
    
    void Awake()
    {
        ServicesLocator.GameManager = this;
        ServicesLocator.EventManager = new EventManager();
        ServicesLocator.PlayerManager = new PlayerController();
        ServicesLocator.BoundaryController = new Boundaries();
        
        ServicesLocator.EventManager.Register<GameStarted>(ServicesLocator.PlayerManager.Initialize);
    }

    private void Start()
    {
        ServicesLocator.EventManager.Register<GameStarted>(DisableTitleScreen);
        ServicesLocator.EventManager.Register<GameStarted>((AGPEvent e) => { _gameBegun = true; });
        
        ServicesLocator.EventManager.Register<GameWon>(EnableGameOverScreen);
        
        ServicesLocator.BoundaryController.Initialize();
    }

    public void OnDestroy()
    {
        ServicesLocator.EventManager.Unregister<GameStarted>(ServicesLocator.PlayerManager.Initialize);
        ServicesLocator.EventManager.Unregister<GameStarted>(DisableTitleScreen);

        ServicesLocator.EventManager.Unregister<GameWon>(EnableGameOverScreen);
    }

    // Update is called once per frame
    void Update()
    {
        if(_gameBegun) ServicesLocator.PlayerManager.Update();
    }

    private void LateUpdate()
    {
        if(_gameBegun) ServicesLocator.BoundaryController.Update();
    }

    public void StartButton()
    {
        ServicesLocator.EventManager.Fire(new GameStarted());
    }

    public void PlayAgainButton()
    {
        SceneManager.LoadScene(0);
    }

    private void DisableTitleScreen(AGPEvent e)
    {
        titleScreen.SetActive(false);
    }

    private void EnableGameOverScreen(AGPEvent e)
    {
        gameOverScreen.SetActive(true);
    }
    
}
