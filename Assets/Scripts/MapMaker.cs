using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEditor;
using UnityEngine;

public class MapMaker : MonoBehaviour {

    public GameObject player;

    public GameObject spawn;
    public GameObject noRoom;
    public GameObject EmptyRoom;
    public GameObject baseEnemyRoom;
    public GameObject holeWall;

    public int modifiableX;
    public int modifiableZ;
    static int X = 11;
    static int Z = 11;
    int startX;
    int startZ;
    int[,] board = {
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 3, 2, 2, 2, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 1, 2, 0, 0, 0, 0 },
            { 0, 0, 2, 4, 2, 2, 2, 0, 0, 0, 0 },
            { 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 2, 4, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };
    GameObject[] rooms;

    void Start() {
        X = modifiableX;
        Z = modifiableZ;
        startX = -10 * (X / 2);
        startZ = -10 * (Z / 2);
        rooms = new GameObject[] { noRoom, spawn, EmptyRoom, baseEnemyRoom, holeWall };
        
        //board = new int[X, Z];
        makeMap();
    }

    void makeMap() {
        //board = new int[,] {
        //    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        //    { 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 0 },
        //    { 0, 0, 0, 0, 3, 2, 2, 2, 0, 0, 0 },
        //    { 0, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0 },
        //    { 0, 0, 0, 0, 0, 1, 2, 0, 0, 0, 0 },
        //    { 0, 0, 2, 4, 2, 2, 2, 0, 0, 0, 0 },
        //    { 0, 0, 2, 2, 2, 2, 2, 0, 0, 0, 0 },
        //    { 0, 0, 0, 0, 0, 2, 4, 0, 0, 0, 0 },
        //    { 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0 },
        //    { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        //};
        for (int i = 0; i < Z; i++)
            for (int j = 0; j < X; j++) {
                Instantiate(rooms[board[i, j]], new Vector3(startX + 10 * j, 0, startZ + 10 * i), new Quaternion(0, 0, 0, 0));
                if (board[i, j] == 1)
                    Instantiate(player, new Vector3(startX + 10 * j, 1, startZ + 10 * i), new Quaternion(0, 0, 0, 0));
            }
    }

    public int[,] getBoard() {
        return board;
    }
}
