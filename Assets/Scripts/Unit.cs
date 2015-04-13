using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

    public int tileX;
    public int tileY;

    public TileMap map;

    public List<Node> currentPath = null;

    int moveSpeed = 2;

    void Update ()
    {
        if (null != currentPath)
        {
            for (int currNode = 0; currNode < currentPath.Count - 1; currNode++)
            {
                Vector3 startPoint = map.TileToWorldCoord(currentPath[currNode].x, currentPath[currNode].y) + Vector3.down / 4;
                Vector3 endPoint = map.TileToWorldCoord(currentPath[currNode + 1].x, currentPath[currNode + 1].y) + Vector3.down / 4;
                Debug.DrawLine(startPoint, endPoint, Color.red);
            }
        }
    }

    public void MoveNextTile()
    {
        float remainingMovement = moveSpeed;
        while (0 < remainingMovement)
        {
            if (null == currentPath)
                return;
            // get cost from next tile.
            remainingMovement -= map.CostToEnterTile(currentPath[1].x, currentPath[1].y);

            // now grab the new first node and move us to that position
            tileX = currentPath[1].x;
            tileY = currentPath[1].y;
            transform.position = map.TileToWorldCoord(tileX, tileY); // update world position


            // remove the old current/first node fromt he path.
            currentPath.RemoveAt(0);

            if (1 == currentPath.Count)
            {
                // we only have one tile left in the path so that tile must be our ultimate destination,
                // and we're standign on it,
                // so let's just clear our pathfinding info.
                currentPath = null;
            }
        }
    }
}
