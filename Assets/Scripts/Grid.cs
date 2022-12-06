using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Grid : MonoBehaviour
{
    // Grid Instance
    public Node[,] Instance;

    public List<Node> Path;

    [Header("References")] 
    [SerializeField]
    private Transform start;
    
    [SerializeField]
    private Transform end;
    
    
    [Header("Parameters")]
    [SerializeField]
    private LayerMask obstacleMask;
    
    [SerializeField]
    private Vector2 gridWorldSize;
    
    [SerializeField]
    private float nodeRadius;
    

    [Header("Materials")] 
    [SerializeField]
    private Material mPassable;
    
    [SerializeField]
    private Material mObstructed;
    
    [SerializeField]
    private Material mStart;
    
    [SerializeField]
    private Material mEnd;
    
    [SerializeField]
    private Material mPath;
    
    
    // private fields
    private float nodeDiameter;
    private Vector2Int GridSize;
    
    
    
    // Visualization
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        
        //i wan
        Vector3 flat = new Vector3(1f, 0.3f, 1f);


        Node startNode = GetNodeFromPos(start.position);
        Node endNode = GetNodeFromPos(end.position);
        
        
        // If Instantiated
        if (Instance != null)
            foreach (var node in Instance)
            {
                // Turnary for if passable/obstructed
                Gizmos.color = (node.Passable)?mPassable.color:mObstructed.color;

                
                // Path Node
                if (Path != null && Path.Contains(node))
                    Gizmos.color = mPath.color;
                    
                
                
                // Starting Node
                if (node == startNode)
                    Gizmos.color = mStart.color;
                    
                // Ending Node
                if (node == endNode)
                    Gizmos.color = mEnd.color;
                
                Gizmos.DrawCube(node.WorldPosition, flat * (nodeDiameter-.1f));
            }
    }
    

    
    
    
    
    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        GridSize.x = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        GridSize.y = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        Generate();
    }



    public Node GetNodeFromPos(Vector3 position_)
    {
        Vector2 percent = new Vector2()
        {
            x = (position_.x + gridWorldSize.x/2) / gridWorldSize.x,
            y = (position_.z + gridWorldSize.y/2) / gridWorldSize.y
        };
        percent.x = Mathf.Clamp01(percent.x);
        percent.y = Mathf.Clamp01(percent.y);

        Vector2Int index = new Vector2Int()
        {
            x = Mathf.RoundToInt((GridSize.x - 1) * percent.x),
            y = Mathf.RoundToInt((GridSize.y - 1) * percent.y)
        };
        
        return Instance[index.x, index.y];
    }

    public List<Node> GetNeighbours(Node node_)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++) {
            for (int y = 0; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                Vector2Int check = new Vector2Int()
                {
                     x = node_.GridPosition.x + x,
                     y = node_.GridPosition.y + y
                };

                if (check.x >= 0 && check.x < GridSize.x && check.y >= 0 && check.y < GridSize.y)
                {
                    neighbours.Add(Instance[check.x, check.y]);
                }
                
            }
        }

        return neighbours;
    }



    public void Generate()
    {
        Instance = new Node[GridSize.x, GridSize.y];
        
        // Grab Corner of Grid so we can generate a centered grid.
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;
        
        for (int x = 0; x < GridSize.x; x++) {
            for (int y = 0; y < GridSize.y; y++)
            {
                // Calculate The offset points from the worldBottomLeft variable + the node were on + the node radius so the nodes get placed with the corner lined up to the area.
                Vector3 worldPoint = worldBottomLeft
                                     + Vector3.right * (x * nodeDiameter + nodeRadius)
                                     + Vector3.forward * (y * nodeDiameter + nodeRadius);

                Vector2Int gridPoint = new Vector2Int(x, y);
                
                bool passable = !(Physics.CheckSphere(worldPoint, nodeRadius, obstacleMask));
    
                Instance[x, y] = new Node(worldPoint, gridPoint, passable);
            }
        }
        
        
    }
}
