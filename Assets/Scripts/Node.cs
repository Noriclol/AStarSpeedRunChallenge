using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 WorldPosition;
    public Vector2Int GridPosition;
    public bool Passable;

    public int G;
    public int H;

    public Node Parent;
    

    public Node(Vector3 worldPosition,Vector2Int gridPosition, bool passable)
    {
        WorldPosition = worldPosition;
        GridPosition = gridPosition;
        Passable = passable;
    }

    public int F
    {
        get
        {
            return G + H;
        }
    }
    
    
    
    
}
