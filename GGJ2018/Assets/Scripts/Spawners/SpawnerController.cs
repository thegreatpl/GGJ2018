using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnerController : MonoBehaviour {

    public MapGenerator MapGenerator; 

    public List<SpawnPoint> SpawnPoints = new List<SpawnPoint>();

    public List<TileBase> DoorTiles; 

	// Use this for initialization
	void Start () {
        MapGenerator = GetComponent<MapGenerator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    /// <summary>
    /// Spawns the given prefab at the given location. 
    /// </summary>
    /// <param name="spawn"></param>
    /// <param name="prefab"></param>
    public void SpawnObject(SpawnPoint spawn, GameObject prefab)
    {
        var spawnLoc = MapGenerator.Base.CellToWorld(spawn.SpawnLocation);
        var newObj = Instantiate(prefab, spawnLoc, prefab.transform.rotation);
        spawn.Spawn(newObj); 
    }


    public void FindDoorSpawns()
    {
        var detail = MapGenerator.Walls;
        bool hasTile = false;
        foreach(var d in DoorTiles)
        {
            if (detail.ContainsTile(d))
            {
                hasTile = true;
                break; 
            }
        }
        if (!hasTile)
            return; 

        for(int xdx = 0; xdx < MapGenerator.XSize; xdx++)
        {
            for (int ydx = 0; ydx < MapGenerator.YSize; ydx++)
            {
                var pos = new Vector3Int(xdx, ydx, 0);
                if (detail.HasTile(pos))
                {
                    if (DoorTiles.Contains(detail.GetTile(pos)))
                    {
                        SpawnPoints.Add(new DoorSpawn()
                        {
                            SpawnLocation = new Vector3Int(xdx, ydx - 1, 0),
                            DoorLoc = pos
                        });
                    }
                }
            }
        }
    }
}
