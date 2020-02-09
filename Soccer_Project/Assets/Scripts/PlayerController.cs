using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode[] movementKeys;
    public float speed;

    private KeyCode _key;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        var speedMultiplier = speed * Time.deltaTime;
        var direction = new Vector3();
        
        if(Input.GetKey(KeyCode.W)) transform.position += speedMultiplier * transform.forward;
        if(Input.GetKey(KeyCode.A)) transform.position += speedMultiplier * -transform.right;
        if(Input.GetKey(KeyCode.S)) transform.position += speedMultiplier * -transform.forward;
        if(Input.GetKey(KeyCode.D)) transform.position += speedMultiplier * transform.right;

        //transform.position += direction * speedMultiplier;
    }
}
