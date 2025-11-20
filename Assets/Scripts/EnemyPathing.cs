using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEditor;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    Queue<Vector2Int> path;
    Vector2Int nextRoom;
    bool seePlayer;
    bool hasPath;
    int[,] board;
    bool startNextRoom;
    public float enemySpeed;
    Rigidbody rb;
    
    void Start()
    {
        seePlayer = false;
        hasPath = false;
        startNextRoom = true;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!seePlayer && !hasPath)
        {
            makePath();
            hasPath = true;
        }
        if (!seePlayer && hasPath)
        {
            followPath();
        }
    }

    void makePath()
    {
        board = GameObject.Find("GameManager").GetComponent<MapMaker>().getBoard();
        List<Vector2Int> openRooms = new List<Vector2Int>();
        for (int i = 0; i < board.GetLength(0); i++)
            for (int j = 0; j < board.GetLength(1); j++)
                if (board[i, j] > 1)
                    openRooms.Add(new Vector2Int(i, j));


        List<Vector2Int> pathList = BFS(openRooms[Random.Range(0, openRooms.Count)]);

        path = new Queue<Vector2Int>();
        foreach (var step in pathList)
            path.Enqueue(step);
    }

    void followPath()
    {
        if (startNextRoom)
        {
            nextRoom = path.Dequeue();
            startNextRoom = false;
        } else
        {
            if (getRoomFromPos(new Vector2(transform.position.x, transform.position.z)) == nextRoom)
            {
                if (path.Count == 0)
                    hasPath = false;
                startNextRoom = true;
            } else
            {
                transform.LookAt(new Vector3(getPosFromRoom(nextRoom).x, transform.position.y, getPosFromRoom(nextRoom).y));
                rb.AddForce(new Vector3(enemySpeed * Time.deltaTime, 0, 0));
            }
        }
    }

    Vector2Int getRoomFromPos(Vector2 location)
    {
        return new Vector2Int(Mathf.RoundToInt(location.x / 10) + 5, Mathf.RoundToInt(location.y / 10) + 5);
    }

    Vector2 getPosFromRoom(Vector2Int room)
    {
        return new Vector2((room.x - 5) * 10, (room.y - 5) * 10);
    }

    List<Vector2Int> BFS(Vector2Int target)
    {
        int width = board.GetLength(0);
        int height = board.GetLength(1);

        bool[,] visited = new bool[width, height];
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        Vector2Int start = getRoomFromPos(new Vector2(transform.position.x, transform.position.z));

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(start);
        visited[start.x, start.y] = true;

        Vector2Int[] dirs = {
            new Vector2Int( 1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int( 0, 1),
            new Vector2Int( 0,-1)
        };

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current == target)
                return ReconstructPath(cameFrom, start, target);

            foreach (var d in dirs)
            {
                var next = current + d;

                if (next.x < 0 || next.x >= width || next.y < 0 || next.y >= height)
                    continue;

                if (board[next.x, next.y] <= 1)
                    continue;

                if (!visited[next.x, next.y])
                {
                    visited[next.x, next.y] = true;
                    cameFrom[next] = current;
                    queue.Enqueue(next);
                }
            }
        }
        return null;
    }

    List<Vector2Int> ReconstructPath(
        Dictionary<Vector2Int, Vector2Int> cameFrom,
        Vector2Int start,
        Vector2Int end)
    {
        List<Vector2Int> pathL = new List<Vector2Int>();
        Vector2Int current = end;

        while (current != start)
        {
            pathL.Add(current);
            current = cameFrom[current];
        }

        pathL.Add(start);
        pathL.Reverse();
        return pathL;
    }
}
