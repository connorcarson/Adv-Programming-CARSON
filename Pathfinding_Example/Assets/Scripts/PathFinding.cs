using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Diagnostics;
using System.Numerics;
using Debug = UnityEngine.Debug;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PathFinding : MonoBehaviour
{
    private PathRequestManager _requestManager;

    private void Awake()
    {
        _requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        StartCoroutine(FindPath(startPosition, targetPosition));
    }
    
    IEnumerator FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        
        Node startNode = GridManager.instance.NodeFromWorldPoint(startPosition);
        Node targetNode = GridManager.instance.NodeFromWorldPoint(targetPosition);

        if (startNode.walkable && targetNode.walkable) {
            Heap<Node> openSet = new Heap<Node>(GridManager.instance.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {

                Node currentNode = openSet.RemoveFirst();

                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    //found path
                    sw.Stop();
                    Debug.Log("Path found: " + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbor in GridManager.instance.GetNeighbors(currentNode))
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor)) continue;

                    int newCostToNeighbor = currentNode.gCost + DistanceBetweenNodes(currentNode, neighbor) +
                                            neighbor.movementPenalty;

                    if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newCostToNeighbor;
                        neighbor.hCost = DistanceBetweenNodes(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                        else
                        {
                            openSet.UpdateItem(neighbor);
                        }

                    }
                }
            }
        }

        yield return null;

        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        _requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    private Vector3[] RetracePath(Node startNode, Node targetNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = targetNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        GridManager.instance.paths.Add(path);
        
        Vector3[] waypoints = SimplifiedPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifiedPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (var i = 1; i < path.Count; i++) {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            
            if(directionNew != directionOld) waypoints.Add(path[i].worldPosition);
            
            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }
    
    private int DistanceBetweenNodes(Node a, Node b)
    {
        int distX = Mathf.Abs(a.gridX - b.gridX);
        int distY = Mathf.Abs(a.gridY - b.gridY);

        if (distX > distY) return 14 * distY + 10 * (distX - distY);
        
        return 14 * distX + 10 * (distY - distX);
    }
}
