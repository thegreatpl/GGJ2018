using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileBuilding
{
    public Vector2Int BuildingPosition;

    public Layer Layer; 

    public TileBase tile; 
}

public enum Layer
{
    Base, 
    Wall, 
    Detail
}

