using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
    Queue<Waypoint> queue = new Queue<Waypoint>();
    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [SerializeField] Waypoint startWaypoint, endWaypoint;
    bool isRunning = true;
    Waypoint searchCenter;

    Vector2Int[] directions = {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    public List<Waypoint> GetPath()
    {
        if (startWaypoint == null || endWaypoint == null)
        {
            print("Waypoint is null error! StartWP is - " + startWaypoint + " - , and EndWP is - " + endWaypoint + " -");
            return null;
        }

        if (path.Count == 0)
        {
            LoadTiles();
            BreathFirstSearch();
            CreatePath();
        }

        return path;
    }

    public void SetStartWaypoint(Waypoint waypoint)
    {
        startWaypoint = waypoint;
    }

    public Waypoint GetStartWaypoint() { return startWaypoint; }

    public void SetEndWaypoint(Waypoint waypoint)
    {
        endWaypoint = waypoint;
    }

    private void LoadTiles()
    {
        Waypoint[] waypoints = FindObjectsOfType<Waypoint>();
        foreach (Waypoint waypoint in waypoints)
        {
            waypoint.isExplored = false;
            var gridPos = waypoint.GetGridPos();
            if (grid.ContainsKey(gridPos))
            {
                Debug.LogWarning("Skipping overlapping tiles " + waypoint);
            }
            else
            {
                grid.Add(gridPos, waypoint);
            }
        }
    }

    private void BreathFirstSearch()
    {
        queue.Enqueue(startWaypoint);

        while (queue.Count > 0 && isRunning)
        {
            searchCenter = queue.Dequeue();
            HaltIfEndFound();
            ExploreNeighbours();

            searchCenter.isExplored = true;
        }
    }

    private void ExploreNeighbours()
    {
        if (!isRunning) { return; }

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbourCoordinates = searchCenter.GetGridPos() + direction;
            if(grid.ContainsKey(neighbourCoordinates))
            {
                QueueNewNeighbour(neighbourCoordinates);
            }
        }
    }

    private void HaltIfEndFound()
    {
        if (searchCenter == endWaypoint)
        {
            isRunning = false;
        }
    }

    private void QueueNewNeighbour(Vector2Int neighbourCoordinates)
    {
        Waypoint neighbour = grid[neighbourCoordinates];
        if (neighbour.isExplored || queue.Contains(neighbour)) { return; }

        queue.Enqueue(neighbour);
        neighbour.exploredFrom = searchCenter;
    }

    private void CreatePath()
    {
        path.Add(endWaypoint);

        if(endWaypoint.exploredFrom == null)
        {
            print("NULL ERROR for " + endWaypoint);
        }

        Waypoint previous = endWaypoint.exploredFrom;
        while(previous != startWaypoint)
        {
            path.Add(previous);
            previous = previous.exploredFrom;
        }
        path.Add(startWaypoint);
        path.Reverse();
    }
}
