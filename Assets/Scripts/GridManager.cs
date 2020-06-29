using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : Manager<GridManager>
{  
    public Tilemap grid;

    protected Graph graph;
    protected Dictionary<Team, int> startPositionPerTeam;

    protected void Awake()
    {
        base.Awake();
        InitializeGraph();
        startPositionPerTeam = new Dictionary<Team, int>();
        startPositionPerTeam.Add(Team.Team1, 0);
        startPositionPerTeam.Add(Team.Team2, graph.Nodes.Count -1);
    }

    public Node GetFreeNode(Team forTeam)
    {
        int startIndex = startPositionPerTeam[forTeam];
        int currentIndex = startIndex;

        while(graph.Nodes[currentIndex].IsOccupied)
        {
            if(startIndex == 0)
            {
                currentIndex++;
                if (currentIndex == graph.Nodes.Count)
                    return null;
            }
            else
            {
                currentIndex--;
                if (currentIndex == -1)
                    return null;
            }
            
        }
        return graph.Nodes[currentIndex];
    }

    public List<Node> GetPath(Node from, Node to)
    {
        return graph.GetShortestPath(from, to);
    }

    public List<Node> GetNodesCloseTo(Node to)
    {
        return graph.Neighbors(to);
    }

    private void InitializeGraph()
    {
        graph = new Graph();

        for (int n = grid.cellBounds.xMin; n < grid.cellBounds.xMax; n++)
        {
            for (int p = grid.cellBounds.yMin; p < grid.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)grid.transform.position.y));
                Vector3 place = grid.CellToWorld(localPlace);
                if (grid.HasTile(localPlace))
                {
                    graph.AddNode(place);
                }
            }
        }

        var allNodes = graph.Nodes;
        foreach (Node from in allNodes)
        {
            foreach (Node to in allNodes)
            {
                if (Vector3.Distance(from.worldPosition, to.worldPosition) < 1f && from != to)
                {
                    graph.AddEdge(from, to);
                }
            }
        }
    }

    public int fromIndex = 0;
    public int toIndex = 0;

    private void OnDrawGizmos()
    {
        if (graph == null)
            return;

        var allEdges = graph.Edges;
        if (allEdges == null)
            return;

        foreach(Edge e in allEdges)
        {
            Debug.DrawLine(e.from.worldPosition, e.to.worldPosition, Color.black, 100);
        }

        var allNodes = graph.Nodes;
        if (allNodes == null)
            return;

        foreach (Node n in allNodes)
        {
            Gizmos.color = n.IsOccupied ? Color.red : Color.green;
            Gizmos.DrawSphere(n.worldPosition, 0.1f);
            
        }

        if (fromIndex >= allNodes.Count || toIndex >= allNodes.Count)
            return;

        List<Node> path = graph.GetShortestPath(allNodes[fromIndex], allNodes[toIndex]);
        if (path.Count > 1)
        {
            for (int i = 1; i < path.Count; i++)
            {
                Debug.DrawLine(path[i - 1].worldPosition, path[i].worldPosition, Color.red, 10);
            }
        }
    }
}
