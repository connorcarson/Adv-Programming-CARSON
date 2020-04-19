using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] 
    private Transform _player;
    
    [SerializeField]
    private Vector2 _gridWorldSize;

    [SerializeField] 
    public float _nodeRadius;

    [SerializeField] 
    private LayerMask _unwalkableLayer;

    public GizmoSettings _gizmoSettings;
    public enum GizmoSettings
    {
        DisplayAllGridGizmos,
        OnlyDisplayGridPathGizmos,
        OnlyDisplaySimplifiedPathGizmos
    }

    [SerializeField] 
    private TerrainType[] walkableRegions;

    private LayerMask walkableMask;

    private Dictionary<int, int> _walkableRegionDictionary = new Dictionary<int, int>();

    private Node[,] _grid;

    private float _nodeDiameter;
    private int _gridWidth, _gridHeight;

    public static GridManager instance;

    public int MaxSize
    {
        get {
            return _gridWidth * _gridHeight;
        }
    }

    private void Awake()
    {
        instance = this;
        
        _nodeDiameter = _nodeRadius * 2;
        _gridWidth = Mathf.RoundToInt(_gridWorldSize.x / _nodeDiameter);
        _gridHeight =  Mathf.RoundToInt(_gridWorldSize.y / _nodeDiameter);

        foreach (TerrainType region in walkableRegions) {
            walkableMask |= region.terrainMask.value;
            _walkableRegionDictionary.Add((int)Math.Log(region.terrainMask.value, 2), region.terrainPenalty);
        }
        
        CreateGrid();
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + _gridWorldSize.x / 2) / _gridWorldSize.x;
        float percentY = (worldPosition.z + _gridWorldSize.y / 2) / _gridWorldSize.y;
        
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((_gridWidth - 1) * percentX);
        int y = Mathf.RoundToInt((_gridHeight - 1) * percentY);

        return _grid[x, y];
    }


    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        for (var x = -1; x <= 1; x++) {
            for (var y = -1; y <= 1; y++) {
                
                if(x == 0 && y == 0) continue;

                var checkX = node.gridX + x;
                var checkY = node.gridY + y;

                if (checkX >= 0 && checkX < _gridWidth && checkY >= 0 && checkY < _gridHeight) {
                    neighbors.Add(_grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }
    

    private void CreateGrid()
    {
        _grid = new Node[_gridWidth, _gridHeight];
        Vector3 worldBottomLeft = transform.position + _gridWorldSize.x/2 * Vector3.left + _gridWorldSize.y/2 * Vector3.back;
        
        for (var x = 0; x < _gridWidth; x++) {
            for (var y = 0; y < _gridHeight; y++) {
                
                Vector3 worldPoint = worldBottomLeft + 
                                     Vector3.right * (x * _nodeDiameter + _nodeRadius) +
                                     Vector3.forward * (y * _nodeDiameter + _nodeRadius);

                bool walkable = !Physics.CheckSphere(worldPoint, _nodeRadius, _unwalkableLayer);

                int movementPenalty = 0;

                if (walkable) {
                    Ray ray = new Ray(worldPoint + Vector3.up * 50.0f, Vector3.down);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100f, walkableMask)) {
                        _walkableRegionDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenalty);
                    }
                }
                
                _grid[x, y] = new Node(walkable, worldPoint, x, y, movementPenalty);
            }
        }
    }

    [System.Serializable] 
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }

    [HideInInspector]
    public List<List<Node>> paths = new List<List<Node>>();
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(_gridWorldSize.x, 1, _gridWorldSize.y));

        if (_gizmoSettings == GizmoSettings.OnlyDisplayGridPathGizmos)
        {
            if (paths == null) return;
            foreach (var path in paths) {
                foreach (Node n in path) {
                    if (path.Contains(n)) Gizmos.color = Color.black;
                        
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (_nodeDiameter - 0.1f));
                }
            }
        }
        else if(_gizmoSettings == GizmoSettings.DisplayAllGridGizmos)
        {
            if (_grid == null) return;
            
            foreach (Node n in _grid) {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (paths != null) {
                    foreach (var path in paths) {
                        if (path.Contains(n)) Gizmos.color = Color.black;
                    }
                }
                
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (_nodeDiameter - 0.1f));
            }
        }
    }
}
