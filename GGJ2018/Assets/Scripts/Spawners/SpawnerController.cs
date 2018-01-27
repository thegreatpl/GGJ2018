using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnerController : MonoBehaviour {

    public MapGenerator MapGenerator; 

    public List<SpawnPoint> SpawnPoints = new List<SpawnPoint>();

    public List<TileBase> DoorTiles;


    public List<GameObject> CivilianPrefabs;


    public List<GameObject> Civilians = new List<GameObject>(); 

    /// <summary>
    /// Chance a civilian will be spawned in a tick. 
    /// </summary>
    public float SpawnChance = 0.1f;


    public int MaxCivilians = 500; 

	// Use this for initialization
	void Start () {
        MapGenerator = GetComponent<MapGenerator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (CivilianPrefabs.Count < 1)
            return; 

        if (Random.value <= SpawnChance && Civilians.Count < MaxCivilians)
        {
            SpawnObject(SpawnPoints.RandomElement(), CivilianPrefabs.RandomElement()); 
        }
	}


    /// <summary>
    /// Spawns the given prefab at the given location. 
    /// </summary>
    /// <param name="spawn"></param>
    /// <param name="prefab"></param>
    public void SpawnObject(SpawnPoint spawn, GameObject prefab)
    {
        var spawnLoc = MapGenerator.Base.GetCellCenterWorld(spawn.SpawnLocation);
        var newObj = Instantiate(prefab, spawnLoc, prefab.transform.rotation);
        spawn.Spawn(newObj);
        Civilians.Add(newObj); 
    }

    /// <summary>
    /// Finds all the doors and creates spawns from them. 
    /// </summary>
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
