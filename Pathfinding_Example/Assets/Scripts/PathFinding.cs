using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField]
    private Transform _seeker, _target;
    
    private GridManager _gridManager;

    private void Start()
    {
        _gridManager = GetComponent<GridManager>();
    }

    private void Update()
    {
        FindPath(_seeker.position, _target.position);
    }

    private void FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Node startNode = _gridManager.NodeFromWorldPoint(startPosition);
        Node targetNode = _gridManager.NodeFromWorldPoint(targetPosition);
        
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0) {
            
            Node currentNode = openSet[0];
            
            for (var i = 1; i < openSet.Count; i++) {
                
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost) {
                    if(openSet[i].hCost < currentNode.hCost) {
                        currentNode = openSet[i];
                    }
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode) { //found path
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbor in _gridManager.GetNeighbors(currentNode))
            {
                if(!neighbor.walkable || closedSet.Contains(neighbor)) continue;

                int newCostToNeighbor = currentNode.gCost + DistanceBetweenNodes(currentNode, neighbor);

                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = DistanceBetweenNodes(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor)) openSet.Add(neighbor);
                }
            }
        }
    }

    private void RetracePath(Node startNode, Node targetNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        
        path.Reverse();
        
        _gridManager.path = path;
    }
    
    private int DistanceBetweenNodes(Node a, Node b)
    {
        int distX = Mathf.Abs(a.gridX - b.gridX);
        int distY = Mathf.Abs(a.gridY - b.gridY);

        if (distX > distY) return 14 * distY + 10 * (distX - distY);
        
        return 14 * distX + 10 * (distY - distX);
    }
}
