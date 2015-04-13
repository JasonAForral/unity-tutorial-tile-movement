using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

    public int tileX;
    public int tileY;

    public TileMap map;

    public List<Node> currentPath = null;

    void Update ()
    {
        if (null != currentPath)
        {
            for (int currNode = 0; currNode < currentPath.Count - 1; currNode++)
            {
                Vector3 startPoint = map.TileToWorldCoord(currentPath[currNode    ].x, currentPath[currNode    ].y);
                Vector3 endPoint   = map.TileToWorldCoord(currentPath[currNode + 1].x, currentPath[currNode + 1].y);
                Debug.DrawLine(startPoint, endPoint, Color.red);
            }
        }
    }

    //public void SetTile(int x, int y)
    //{

    //}
}
