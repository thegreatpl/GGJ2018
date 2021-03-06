﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour {

    public Tilemap Base;

    public Tilemap Walls;

    public Tilemap Detail;

    /// <summary>
    /// X map size. 
    /// </summary>
    public int XSize = 100;

    /// <summary>
    /// Y map size. 
    /// </summary>
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

    /// <summary>
    /// List of building definations. 
    /// </summary>
    public List<BuildingDef> BuildingDefs = new List<BuildingDef>();



    /// <summary>
    /// List of building blueprints. Call .Print on them to print at a position. 
    /// </summary>
    List<Building> Buildings = new List<Building>();

    SpawnerController spawner; 


	// Use this for initialization
	void Start () {
        Building.MapGenerator = this;
        BaseAI.MapGenerator = this; 
        spawner = GetComponent<SpawnerController>(); 

        LoadBuildings(); 
        GenerateMap();
        spawner.SpawnPlayers(4); 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Loads the buildings from the map. 
    /// </summary>
    public void LoadBuildings()
    {
        foreach(var buildingDef in BuildingDefs)
        {
            var building = new Building();
            RectInt mapPos = new RectInt(buildingDef.X, buildingDef.Y, buildingDef.Width, buildingDef.Height);
            building.LoadBuilding(mapPos);
            building.Type = buildingDef.Type;
            Buildings.Add(building); 
        }
    }



    /// <summary>
    /// Generate the basic map. 
    /// </summary>
    void GenerateMap()
    {
        bool gen = true;
        while (gen)
        {
            GenerateBase();
            // CreateRoads();

            GenerateBuildings();
            spawner.FindDoorSpawns();

            if (spawner.SpawnPoints.Count > 0)
                gen = false; 
        }
    }

    /// <summary>
    /// Generates the base lands. 
    /// </summary>
    void GenerateBase()
    {
        var grasses = Tiles.Where(x => x.Type == "Grass");
        var walls = Tiles.Where(x => x.Type == "Wall");
        var cobbles = Tiles.Where(x => x.Type == "Cobble"); 

        var NSWall = Tiles.Where(x => x.Type == "NSWall");
        var EWWall = Tiles.Where(x => x.Type == "EWWall");
        var ESWallDetail = Tiles.Where(x => x.Type == "EWWallDetail");
        var TopLeftWall = Tiles.Where(x => x.Type == "TLWall");
        var TLWallDetail = Tiles.Where(x => x.Type == "TLWallDetail");
        var TRWall = Tiles.Where(x => x.Type == "TRWall");
        var TRWallDetail = Tiles.Where(x => x.Type == "TRWallDetail");
        var BLWall = Tiles.Where(x => x.Type == "BLWall");
        var BLWallDetail = Tiles.Where(x => x.Type == "BLWallDetail");
        var BRWall = Tiles.Where(x => x.Type == "BRWall");
        var BRWallDetail = Tiles.Where(x => x.Type == "BRWallDetail");

        int surroundings = 20; 

        for (int xdx = -surroundings; xdx <= XSize + surroundings; xdx++)
        {
            for (int ydx = -surroundings; ydx <= YSize + surroundings; ydx++)
            {
                if (xdx >= 0 && xdx <= XSize && ydx >= 0 && ydx <= YSize)
                {
                    Base.SetTile(new Vector3Int(xdx, ydx, 0), cobbles.ElementAt(Random.Range(0, cobbles.Count())).tileBase);
                }
                else
                {
                    Base.SetTile(new Vector3Int(xdx, ydx, 0), grasses.ElementAt(Random.Range(0, grasses.Count())).tileBase);
                    continue; 
                }

                if (xdx == 0 || xdx == XSize)
                {
                    Walls.SetTile(new Vector3Int(xdx, ydx, 0), NSWall.ElementAt(Random.Range(0, NSWall.Count())).tileBase);
                }
                if (ydx == 0 || ydx == YSize)
                {
                    Walls.SetTile(new Vector3Int(xdx, ydx, 0), EWWall.ElementAt(Random.Range(0, EWWall.Count())).tileBase);
                    Detail.SetTile(new Vector3Int(xdx, ydx + 1, 0), ESWallDetail.ElementAt(Random.Range(0, ESWallDetail.Count())).tileBase);
                }

                //bottom left. 
                if (ydx == 0 && xdx == 0)
                {
                    Walls.SetTile(new Vector3Int(xdx, ydx, 0), BLWall.ElementAt(Random.Range(0, BLWall.Count())).tileBase);
                    Detail.SetTile(new Vector3Int(xdx, ydx + 1, 0), BLWallDetail.ElementAt(Random.Range(0, BLWallDetail.Count())).tileBase);
                }
                //bottom right
                else if (ydx == 0 && xdx == XSize)
                {
                    Walls.SetTile(new Vector3Int(xdx, ydx, 0), BRWall.ElementAt(Random.Range(0, BRWall.Count())).tileBase);
                    Detail.SetTile(new Vector3Int(xdx, ydx + 1, 0), BRWallDetail.ElementAt(Random.Range(0, BRWallDetail.Count())).tileBase);

                }
                //top left
                else if (ydx == YSize && xdx == 0)
                {
                    Walls.SetTile(new Vector3Int(xdx, ydx, 0), TopLeftWall.ElementAt(Random.Range(0, TopLeftWall.Count())).tileBase);
                    Detail.SetTile(new Vector3Int(xdx, ydx + 1, 0), TLWallDetail.ElementAt(Random.Range(0, TLWallDetail.Count())).tileBase);

                }
                //top right. 
                else if (ydx == YSize && xdx == XSize)
                {
                    Walls.SetTile(new Vector3Int(xdx, ydx, 0), TRWall.ElementAt(Random.Range(0, TRWall.Count())).tileBase);
                    Detail.SetTile(new Vector3Int(xdx, ydx + 1, 0), TRWallDetail.ElementAt(Random.Range(0, TRWallDetail.Count())).tileBase);

                }

                //if (xdx == 0 || ydx == 0 || XSize == xdx || YSize == ydx)
                //{
                //    Walls.SetTile(new Vector3Int(xdx, ydx, 0), walls.ElementAt(Random.Range(0, walls.Count())).tileBase); 
                //}

            }
        }
    }

    /// <summary>
    /// Generates a bunch of buildings. 
    /// </summary>
    void GenerateBuildings()
    {
        Vector2Int center = new Vector2Int(XSize / 2, YSize / 2);
        int marketSize = 50; 
        int left = center.x - Random.Range(0, marketSize / 2);
        int right = center.x + Random.Range(0,marketSize / 2);
        int up = center.y + Random.Range(0, marketSize / 2);
        int down = center.y - Random.Range(0, marketSize / 2);
        FillSpaceWithBuildings(new RectInt(left, down, right - left, up - down), "market");
        List<Vector2Int> taken = new List<Vector2Int>(); 
        for(int xdx = left; xdx < right; xdx ++)
        {
            for (int ydx = down; ydx < up; ydx++)
            {
                taken.Add(new Vector2Int(xdx, ydx)); 
            }
        }

        FillSpaceWithBuildings(new RectInt(1, 3, XSize - 1, YSize - 3), "house", taken: taken); 

    }

    /// <summary>
    /// Attempts to fill an area with buildings. 
    /// </summary>
    /// <param name="area"></param>
    /// <param name="type"></param>
    /// <param name="attempts"></param>
    /// <param name="taken"></param>
    void FillSpaceWithBuildings(RectInt area, string type, int attempts = 1000, List<Vector2Int> taken = null)
    {
        if (taken == null)
            taken = new List<Vector2Int>();

        var available = Buildings.Where(x => x.Type == type);
        if (available.Count() < 1)
            return; 

        for (int idx = 0; idx < attempts; idx++)
        {
            int xTry = Random.Range(area.xMin, area.xMax);
            int yTry = Random.Range(area.yMin, area.yMax);
            var building = available.RandomElement();

            bool intersected = false; 
            var footprint = new List<Vector2Int>(); 
            for (int xdx = xTry - 1; xdx <= xTry + building.Width + 1; xdx++)
            {
                for (int ydx = yTry - 1; ydx <= yTry + building.Height + 1; ydx++)
                {
                    var vect = new Vector2Int(xdx, ydx);
                    if (taken.Contains(vect) || vect.x > area.xMax || vect.y > area.yMax)
                    {
                        intersected = true;
                        break; 
                    }

                    footprint.Add(vect); 
                }
                if (intersected)
                    break; 
            }
            if (intersected)
                continue;

            building.PrintBuilding(xTry, yTry);
            taken.AddRange(footprint); 
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
