using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Building
{
    public static MapGenerator MapGenerator;  

    /// <summary>
    /// The type of building. 
    /// </summary>
    public string Type;


    public int Width;

    public int Height;

    /// <summary>
    /// The tiles for this building. 
    /// </summary>
    public List<TileBuilding> Tiles = new List<TileBuilding>();

    /// <summary>
    /// Loads the building. 
    /// </summary>
    /// <param name="rect"></param>
    public void LoadBuilding(RectInt rect)
    {
        Width = rect.width;
        Height = rect.height;
        int x = 0; 
        for (int xdx = rect.xMin; xdx < rect.xMax; xdx++)
        {
            int y = 0; 
            for (int ydx = rect.yMin; ydx < rect.yMax; ydx++)
            {
                var loc = new Vector3Int(xdx, ydx, 0);
                

                if (MapGenerator.Base.HasTile(loc))
                {
                    var tileb = MapGenerator.Base.GetTile(loc);
                    Tiles.Add(new TileBuilding()
                    {
                        BuildingPosition = new Vector2Int(x, y),
                        Layer = Layer.Base,
                        tile = tileb
                    });
                }
                if (MapGenerator.Walls.HasTile(loc))
                {
                    var tilew = MapGenerator.Walls.GetTile(loc);
                    Tiles.Add(new TileBuilding()
                    {
                        BuildingPosition = new Vector2Int(x, y),
                        Layer = Layer.Wall,
                        tile = tilew
                    });
                }

                if (MapGenerator.Detail.HasTile(loc))
                {
                    var tiled = MapGenerator.Detail.GetTile(loc);
                    Tiles.Add(new TileBuilding()
                    {
                        BuildingPosition = new Vector2Int(x, y),
                        Layer = Layer.Detail,
                        tile = tiled
                    });
                }


                y++; 
            }
            x++; 
        }
    }

    /// <summary>
    /// Prints this building. 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void PrintBuilding(int x, int y)
    {
        foreach(var tile in Tiles)
        {
            var pos = new Vector3Int(tile.BuildingPosition.x + x, tile.BuildingPosition.y + y, 0);
            switch(tile.Layer)
            {
                case Layer.Base:
                    MapGenerator.Base.SetTile(pos, tile.tile);
                    break;
                case Layer.Detail:
                    MapGenerator.Detail.SetTile(pos, tile.tile);
                    break;

                case Layer.Wall:
                    MapGenerator.Walls.SetTile(pos, tile.tile);
                    break; 
            }
        }
    }
}

