using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour {

    public Tilemap Base;

    public Tilemap Walls;

    public Tilemap Detail;


    public int XSize = 100;

    public int YSize = 100;

    /// <summary>
    /// List of tiles that can be used to generate the map. 
    /// </summary>
    public List<TileStoreObj> Tiles = new List<TileStoreObj>(); 


	// Use this for initialization
	void Start () {
        GenerateMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void GenerateMap()
    {
        var grasses = Tiles.Where(x => x.Type == "Grass");
        var walls = Tiles.Where(x => x.Type == "Wall"); 

        for (int xdx = 0; xdx <= XSize; xdx++)
        {
            for (int ydx = 0; ydx <= YSize; ydx++)
            {
                Base.SetTile(new Vector3Int(xdx, ydx, 0), grasses.ElementAt(Random.Range(0, grasses.Count())).tileBase);
                if (xdx == 0 || ydx == 0 || XSize == xdx || YSize == ydx)
                {
                    Walls.SetTile(new Vector3Int(xdx, ydx, 0), walls.ElementAt(Random.Range(0, walls.Count())).tileBase); 
                }

            }
        }
    }
}
