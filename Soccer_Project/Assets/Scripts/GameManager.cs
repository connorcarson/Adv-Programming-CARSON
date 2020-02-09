using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ServicesLocator.GameManager = this;
        ServicesLocator.PlayerManager = new PlayerController();
        ServicesLocator.PlayerManager.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        ServicesLocator.PlayerManager.Update();
    }
}
