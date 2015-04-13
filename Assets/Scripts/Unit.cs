using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

    public int tileX;
    public int tileY;

    public float animationSpeed = 1f;
    bool inMotion = false;

    public TileMap map;

    public List<Node> currentPath = null;

    public int moveSpeed = 2;
    float remainingMovement = 2;

    void Update ()
    {
        if (null != currentPath)
        {
            for (int currNode = 0; currNode < currentPath.Count - 1; currNode++)
            {
                // draw path with debug lines
                Vector3 startPoint = map.TileToWorldCoord(currentPath[currNode].x, currentPath[currNode].y) + Vector3.down / 4;
                Vector3 endPoint = map.TileToWorldCoord(currentPath[currNode + 1].x, currentPath[currNode + 1].y) + Vector3.down / 4;
                Debug.DrawLine(startPoint, endPoint, Color.red);
            }
        }

        // is position close enough for next movement step?
        if (Vector3.Distance(transform.position, map.TileToWorldCoord(tileX, tileY)) < 0.01f){
            inMotion = false;
            AdvancePathing();
        }

        // Smoothly animate towards the correct map tile.
        //transform.position = Vector3.Lerp(transform.position, map.TileToWorldCoord(tileX, tileY), animationSpeed * Time.deltaTime);
        if (inMotion)
        {
            transform.LookAt(map.TileToWorldCoord(tileX, tileY), Vector3.up);
            transform.Translate(Vector3.forward * animationSpeed * Time.deltaTime);
        }
    }

    // Advances our pathfinding progress by one tile.
    void AdvancePathing()
    {
        if (null == currentPath)
                return;
        if (remainingMovement <= 0)
            return;

        // get cost from next tile.
        remainingMovement -= map.CostToEnterTile(currentPath[1].x, currentPath[1].y);

        // Teleport us to our correct "current" position, in case we
        // haven't finished the animation yet.
        transform.position = map.TileToWorldCoord(tileX, tileY);

        // now grab the new first node and move us to that position
        tileX = currentPath[1].x;
        tileY = currentPath[1].y;

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
    
    // The "Next Turn" button calls this.
	public void NextTurn() {
		// Make sure to wrap-up any outstanding movement left over.
		while(currentPath!=null && remainingMovement > 0) {
			AdvancePathing();
		}

		// Reset our available movement points.
		remainingMovement = moveSpeed;
        inMotion = true;
	}
}
