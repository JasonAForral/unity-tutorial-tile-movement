using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TileClickHandle : MonoBehaviour {

    public int tileX;
    public int tileY;
    //public bool isWalkable;

    public TileMap map;

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        //if (isWalkable)
        map.GeneratePathTo(tileX, tileY);
    }
    
}