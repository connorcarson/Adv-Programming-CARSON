using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private Vector2 _gridWorldSize;

    [SerializeField] 
    public float _nodeRadius;

    [SerializeField] 
    private LayerMask _unwalkableLayer;
    
    private Node[,] _grid;

    private void Start()
    {
        
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_gridWorldSize.x, 1, _gridWorldSize.y));
    }
}
