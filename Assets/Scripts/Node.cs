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
    

    public Node(Vector3 worldPosition_,Vector2Int gridPosition_, bool passable_)
    {
        WorldPosition = worldPosition_;
        GridPosition = gridPosition_;
        Passable = passable_;
    }

    public int F
    {
        get
        {
            return G + H;
        }
    }
    
    
    
    
}
