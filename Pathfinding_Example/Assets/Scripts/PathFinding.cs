using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private GridManager _gridManager;

    private void Start()
    {
        _gridManager = GetComponent<GridManager>();
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
                
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) {
                    
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode) return; //found path

            foreach (Node n in _gridManager.GetNeighbors(currentNode))
            {
                if(!n.walkable || closedSet.Contains(n)) continue;
                
                
            }
        }
    }

    private void DistanceBetweenNodes(Node a, Node b)
    {
        
    }
}
