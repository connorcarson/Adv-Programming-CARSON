using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX, gridY;
    
    public int gCost, hCost;
    public int fCost
    {
        get { return gCost + hCost; }
    }
    
    public Node parent;

    private int _heapIndex;

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int HeapIndex {
        get {
            return _heapIndex;
        }
        set {
            _heapIndex = value;
        }
    }

    public int CompareTo(Node toCompare)
    {
        int compare = fCost.CompareTo(toCompare.fCost);
        if (compare == 0) {
            compare = hCost.CompareTo(toCompare.hCost);
        }

        return -compare;
    }
}
