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
    /// Width of the road. 
    /// </summary>
    public int RoadWidth = 3;

    public int MaxRoadLength = 10; 

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

    /// <summary>
    /// Generate the basic map. 
    /// </summary>
    void GenerateMap()
    {
        GenerateBase();
        CreateRoads(); 
    }

    /// <summary>
    /// Generates the base lands. 
    /// </summary>
    void GenerateBase()
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

    List<Vector3Int> RoadCores = new List<Vector3Int>(); 

    /// <summary>
    /// Creates a bunch of roads. 
    /// </summary>
    void CreateRoads()
    {
        var roads = Tiles.Where(x => x.Type == "Road");

        var lotEdges = new List<Vector2Int>();

        var roadTiles = new List<Vector3Int>();

        Vector3Int center = new Vector3Int(XSize/2,YSize/2, 0);

        RoadCores.Add(center);
        var road = roads.ElementAt(Random.Range(0, roads.Count())); 
        CreateAvenue(center, XSize / 2, RoadWidth, Direction.East, road.tileBase);
        CreateAvenue(center, XSize / 2, RoadWidth, Direction.West, road.tileBase);
        CreateAvenue(center, YSize / 2, RoadWidth, Direction.North, road.tileBase);
        CreateAvenue(center, YSize / 2, RoadWidth, Direction.South, road.tileBase);


        roadTiles.AddRange(GenerateSquare(center, new Vector2Int(20, 20)));

        


    }

    void CreateAvenue(Vector3Int start, int length, int width, Direction direction, TileBase tile)
    {
        int xTop, yTop, xBottom, yBottom;
        int halfWidth = (width / 2);
        switch(direction)
        {
            case Direction.East:
                xTop = start.x + length;
                xBottom = start.x;
                yTop = start.y + halfWidth;
                yBottom = start.y - halfWidth;
                break;
            case Direction.West:
                xBottom = start.x - length;
                xTop = start.x;
                yTop = start.y + halfWidth;
                yBottom = start.y - halfWidth;
                break;
            case Direction.North:
                yTop = start.y + length;
                yBottom = start.y;
                xTop = start.x + halfWidth;
                xBottom = start.x - halfWidth;
                break;
            case Direction.South:
                yBottom = start.y - length;
                yTop = start.y;
                xTop = start.x + halfWidth;
                xBottom = start.x - halfWidth;
                break;
            default:
                return; 
        }

        for (; xBottom <= xTop; xBottom++)
        {
            for (int ydx = yBottom; ydx <= yTop; ydx++)
            {
                Base.SetTile(new Vector3Int(xBottom, ydx, 0), tile); 
            }
        }
    }

    

    /// <summary>
    /// Generates a square. 
    /// </summary>
    /// <param name="center"></param>
    /// <returns></returns>
    List<Vector3Int> GenerateSquare(Vector3Int center, Vector2Int bounds)
    {
        //generate basic square for now. 

        var roadTiles = Tiles.Where(x => x.Type == "Road");

        var chosen = roadTiles.ElementAt(Random.Range(0, roadTiles.Count()));

        List<Vector3Int> contained = new List<Vector3Int>();

        Base.SetTile(center, chosen.tileBase);
        contained.Add(center); 

        int left = Random.Range(0, bounds.x / 2);
        int right = Random.Range(0, bounds.x / 2);
        int up = Random.Range(0, bounds.y / 2);
        int down = Random.Range(0, bounds.y / 2);

        for(int xdx = center.x - left; xdx <= center.x + right; xdx++)
        {
            for (int ydx = center.y - down; ydx <= center.y + up; ydx++)
            {
                var t = new Vector3Int(xdx, ydx, 0);
                Base.SetTile(t, chosen.tileBase);
                contained.Add(t); 
            }
        }


        return contained; 
    }
}
