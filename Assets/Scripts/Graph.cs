using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph
{
    private List<Node> nodes;
    private List<Edge> edges;

    public List<Node> Nodes => nodes;
    public List<Edge> Edges => edges;

    public Graph()
    {
        nodes = new List<Node>();
        edges = new List<Edge>();
    }

    public bool Adjacent(Node from, Node to)
    {
        foreach(Edge e in edges)
        {
            if (e.from == from && e.to == to)
                return true;
        }
        return false;
    }

    public List<Node> Neighbors(Node from)
    {
        List<Node> result = new List<Node>();

        foreach (Edge e in edges)
        {
            if (e.from == from)
                result.Add(e.to);
        }
        return result;
    }

    public void AddNode(Vector3 worldPosition)
    {
        nodes.Add(new Node(nodes.Count, worldPosition));
    }

    public void AddEdge(Node from, Node to)
    {
        edges.Add(new Edge(from, to, 1));
    }

    public float Distance(Node from, Node to)
    {
        foreach (Edge e in edges)
        {
            if (e.from == from && e.to == to)
                return e.GetWeight();
        }

        return Mathf.Infinity;
    }

    public virtual List<Node> GetShortestPath(Node start, Node end)
    {
        List<Node> path = new List<Node>();

        // If the start and end are same node, we can return the start node
        if (start == end)
        {
            path.Add(start);
            return path;
        }

        // The list of unvisited nodes
        List<Node> unvisited = new List<Node>();

        // Previous nodes in optimal path from source
        Dictionary<Node, Node> previous = new Dictionary<Node, Node>();

        // The calculated distances, set all to Infinity at start, except the start Node
        Dictionary<Node, float> distances = new Dictionary<Node, float>();

        for (int i = 0; i < nodes.Count; i++)
        {
            Node node = nodes[i];
            unvisited.Add(node);

            // Setting the node distance to Infinity
            distances.Add(node, float.MaxValue);
        }

        // Set the starting Node distance to zero
        distances[start] = 0f;
        while (unvisited.Count != 0)
        {            
            // Getting the Node with smallest distance
            unvisited = unvisited.OrderBy(node => distances[node]).ToList();
            Node current = unvisited[0];
            unvisited.Remove(current);

            // When the current node is equal to the end node, then we can break and return the path
            if (current == end)
            {
                // Construct the shortest path
                while (previous.ContainsKey(current))
                {
                    // Insert the node onto the final result
                     path.Insert(0, current);
                    //Traverse from start to end
                    current = previous[current];
                }

                //Insert the source onto the final result
                path.Insert(0, current);
                break;
            }

            // Looping through the Node connections (neighbors) and where the connection (neighbor) is available at unvisited list
            foreach(Node neighbor in Neighbors(current))
            {
                // Getting the distance between the current node and the connection (neighbor)
                float length = Vector3.Distance(current.worldPosition, neighbor.worldPosition);

                // The distance from start node to this connection (neighbor) of current node
                float alt = distances[current] + length;

                // A shorter path to the connection (neighbor) has been found
                if (alt < distances[neighbor])
                {
                    distances[neighbor] = alt;
                    previous[neighbor] = current;
                }
            }
        }
        return path;
    }
}

public class Node
{
    public int index;
    public Vector3 worldPosition;

    private bool occupied = false;

    public Node(int index, Vector3 worldPosition)
    {
        this.index = index;
        this.worldPosition = worldPosition;
        occupied = false;
    }

    public void SetOccupied(bool val)
    {
        occupied = val;
    }

    public bool IsOccupied => occupied;
}

public class Edge
{
    public Node from;
    public Node to;

    private float weight;

    public Edge(Node from, Node to, float weight)
    {
        this.from = from;
        this.to = to;
        this.weight = weight;
    }

    public float GetWeight()
    {
        if (to.IsOccupied)
            return Mathf.Infinity;

        return weight;
    }
}