﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TileMap : MonoBehaviour {

    public GameObject selectedUnit;

	public TileType[] tileTypes;

    //public Unit[] units;

    int[,] tiles;
    Node[,] graph;

    // assuming only one unit
    List<Node> currentPath = null;
    
    public int mapSizeX;
    public int mapSizeY;

    void Start()
    {
        GenerateMapData();
        GeneratePathfindingGraph();
        GenerateMapVisuals();

    }

    void GenerateMapData()
    {
        // Allocate our map tiles
        tiles = new int[mapSizeX, mapSizeY];

        // Initialize our map tiles to be grass
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeX; y++)
            {
                tiles[x, y] = 0;
            }
        }

        // Add Swamp area
        for (int x = 3; x < 5; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                tiles[x, y] = 1;
            }
        }
        
        // Let's make a u-shaped mountain range

        tiles[4, 4] = 2;
        tiles[5, 4] = 2;
        tiles[6, 4] = 2;
        tiles[7, 4] = 2;
        tiles[8, 4] = 2;

        tiles[4, 5] = 2;
        tiles[4, 6] = 2;
        tiles[8, 5] = 2;
        tiles[8, 6] = 2;
    }

    public class Node
    {
        public List<Node> neighbors;
        public int x;
        public int y;

        public Node()
        {
            neighbors = new List<Node>();
        }


        public float DistanceTo(Node n)
        {
            return Vector2.Distance(
                new Vector2(x, y),
                new Vector2(n.x, n.y)
                );
        }

        public Node CopyNode()
        {
            Node ode = new Node();
            ode.x = x;
            ode.y = y;
            ode.neighbors = neighbors;
            return ode;
        }

    }


    void GeneratePathfindingGraph()
    {
        // Initialize the array
        graph = new Node[mapSizeX,mapSizeY];

        // initialize each node
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                graph[x, y] = new Node();
                graph[x, y].x = x;
                graph[x, y].y = y;

            }
        }
        
        // calculate neighbors
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                //// 4 way connected
                if (0 < x)
                    graph[x, y].neighbors.Add(graph[x - 1, y]);
                if (mapSizeX - 1 > x)
                    graph[x, y].neighbors.Add(graph[x + 1, y]);
                if (0 < y)
                    graph[x, y].neighbors.Add(graph[x, y - 1]);
                if (mapSizeY - 1 > y)
                    graph[x, y].neighbors.Add(graph[x, y + 1]);

                // 6 way
                // alternate rows can go diagonal
                /*
                 * 
                 * 
                 * */

            }
        }
    }

    void GenerateMapVisuals()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeX; y++)
            {
                TileType tt = tileTypes[tiles[x, y]];
                GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x, 0, y), Quaternion.identity);
                TileClickHandle tch = go.GetComponent<TileClickHandle>();
                tch.tileX = x;
                tch.tileY = y;
                tch.map = this;
                //tch.isWalkable = tileTypes[tiles[x, y]].isWalkable;
                
            }
        }
    }

    public Vector3 TileToWorldCoord(int x, int y)
    {
        return new Vector3(x, 1, y);
    }

    public void GeneratePathTo(int x, int y)
    {
        /*
        selectedUnit.GetComponent<Unit>().tileX = x;
        selectedUnit.GetComponent<Unit>().tileY = y;
        selectedUnit.transform.position = TileToWorldCoord(x, y);
        selectedUnit.transform.rotation = Quaternion.identity;
        */

        currentPath = null;
        Node target = graph[x, y].CopyNode();
        
        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();
        
        // setup the queue - the nodes unchecked
        List<Node> unvisited = new List<Node>();

        Node source = graph[
            selectedUnit.GetComponent<Unit>().tileX,
            selectedUnit.GetComponent<Unit>().tileY
            ];
        dist[source] = 0;
        prev[source] = null;

        // Initialize everything to have INFINITY distance,
        // since we don't know any better right now.
        // also it's possible that some nodes CAN'T be reached form the source,
        // which would make INFINITY a reasonable value

        foreach (Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisited.Add(v);
        }

        while(0 < unvisited.Count)
        {
            // "u" is unvisited node with smallest distance
            Node u = null;

            foreach (Node possibleU in unvisited)
            {
                if (null == u || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            if (u == target)
            {
                break; //exit the while loop
            }

            unvisited.Remove(u);
            foreach (Node v in u.neighbors)
            {
                float alt = dist[u] + u.DistanceTo(v);
                if (alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }

        // if we get here, either we found the shortest route or there is no route at all

        if (null == dist[target])
        {
            // no route between target and source
            return;
        }

        currentPath = new List<Node>();
        Node curr = target;
        while (null != curr)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }

        // right now, current path describes a rout from our target to our source
        // so we need to invert it

        currentPath.Reverse();

    }
}
