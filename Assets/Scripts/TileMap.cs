using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileMap : MonoBehaviour {

    public GameObject selectedUnit;

	public TileType[] tileTypes;

    //public Unit[] units;

    int[,] tiles;
    Node[,] graph;

    public int mapSizeX;
    public int mapSizeY;

    void Start()
    {
        GenerateMapData();
        GenerateMapVisuals();
        //GeneratePathfindingGraph();
        
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

    class Node
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
            return Vector2.Distance
                (
                new Vector2(x, y),
                new Vector2(n.x, n.y)
                );
        }

    }

    void GeneratePathfindingGraph()
    {
        graph = new Node[mapSizeX, mapSizeY];

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeX; y++)
            {
                graph[x, y].x = x;
                graph[x, y].y = y;

                //// 4 way connected
                if (0 < x)
                    graph[x, y].neighbors.Add(graph[x - 1, y]);
                if (mapSizeX - 1 > x)
                    graph[x, y].neighbors.Add(graph[x + 1, y]);
                if (0 < y)
                    graph[x, y].neighbors.Add(graph[x, y - 1]);
                if (mapSizeY - 1 > y)
                    graph[x, y].neighbors.Add(graph[x, y + 1]);
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
            }
        }
    }

    public Vector3 TileToWorldCoord(int x, int y)
    {
        return new Vector3(x, 2, y);
    }

    public void MoveSelectedUnitTo(int x, int y)
    {
        
        selectedUnit.GetComponent<Unit>().tileX = x;
        selectedUnit.GetComponent<Unit>().tileY = y;
        selectedUnit.transform.position = TileToWorldCoord(x, y);
        selectedUnit.transform.rotation = Quaternion.identity;
        
        /*
        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        // setup the queue - unchecked nodes
        List<Node> unvisited = new List<Node>();

        Node source = graph[
            selectedUnit.GetComponent<Unit>().tileX,
            selectedUnit.GetComponent<Unit>().tileY
            ];
        dist[source] = 0;
        prev[source] = null;

        foreach (Node v in graph)
        {
            if (v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }
            unvisited.Add(v);
        }

        while(unvisited.Count > 0)
        {
            // not fast but short
            // consitter priority of other self sorting optimized
            Node u = unvisited.OrderBy(n => dist[n]).First();
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

        */
    }

}
