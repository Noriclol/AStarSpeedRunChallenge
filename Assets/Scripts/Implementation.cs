using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Implementation : MonoBehaviour
{
    
    [Header("References")] 
    [SerializeField]
    private Transform start;
    
    [SerializeField]
    private Transform end;
    
    [SerializeField]
    private Grid grid;


    private void LateUpdate()
    {
        FindPath();
    }


    public void FindPath()
    {
        Node startNode = grid.GetNodeFromPos(start.position);
        Node endNode = grid.GetNodeFromPos(end.position);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].F < current.F || openSet[i].F == current.F && openSet[i].H < current.H)
                    current = openSet[i];
                
            }

            openSet.Remove(current);
            closedSet.Add(current);

            if (current == endNode)
            {
                RetracePath(startNode, endNode);
                return;
            }

            foreach (var neighbour in grid.GetNeighbours(current))
            {
                if (!neighbour.Passable || closedSet.Contains(neighbour))
                    continue;

                int newMovementCostToNeighbour = current.G + GetDistance(current, neighbour);

                if (newMovementCostToNeighbour < current.G || !openSet.Contains(neighbour))
                {
                    neighbour.G = newMovementCostToNeighbour;
                    neighbour.H = GetDistance(neighbour, endNode);
                    neighbour.Parent = current;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                    
                }

            }
            
            
        }
    }

    void RetracePath(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node current = end;

        while (current != start)
        {
            path.Add(current);
            current = current.Parent;
        }
        path.Reverse();
        grid.Path = path;
    }
    

    public int GetDistance(Node a, Node b)
    {
        Vector2Int aPos = a.GridPosition;
        Vector2Int bPos = b.GridPosition;

        Vector2Int delta = new Vector2Int()
        {
            x = Mathf.Abs(aPos.x - bPos.x),
            y = Mathf.Abs(aPos.y - bPos.y)
        };
        
        if (delta.x > delta.y)
        {
            return 14 * delta.y + 10 * (delta.x - delta.y);
        }
        return 14 * delta.x + 10 * (delta.y - delta.x);
    }

    private Node FindLowestFCost()
    {
        return null;
    }
    
    
    
}
