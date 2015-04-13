using UnityEngine;
using System.Collections;

public class TileClickHandle : MonoBehaviour {

    public int tileX;
    public int tileY;
    //public bool isWalkable;

    public TileMap map;

    void OnMouseDown()
    {
        Debug.Log("Click");
        //if (isWalkable)
        map.GeneratePathTo(tileX, tileY);
    }
    
}