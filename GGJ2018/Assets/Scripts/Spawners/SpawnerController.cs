﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class SpawnerController : MonoBehaviour {

    public MapGenerator MapGenerator; 

    public List<SpawnPoint> SpawnPoints = new List<SpawnPoint>();

    public List<TileBase> DoorTiles;



  //  public List<GameObject> CivilianPrefabs;

    public int CiviliansCount { get { return Civilians.Count; } }

    [HideInInspector]
    public List<GameObject> Civilians = new List<GameObject>();

    /// <summary>
    /// Zombies list. 
    /// </summary>
   // public List<ZombieSpawner> Zombies = new List<ZombieSpawner>();

    public int ZombiesCount { get { return LivingZombies.Count; } }

    /// <summary>
    /// List of all the zombies. 
    /// </summary>
    [HideInInspector]
    public List<GameObject> LivingZombies = new List<GameObject>();

    /// <summary>
    /// The different player animations. 
    /// </summary>
    public List<RuntimeAnimationControllerStore> Animations = new List<RuntimeAnimationControllerStore>();

    /// <summary>
    /// The player Prefab. 
    /// </summary>
    public GameObject PlayerPrefab;

    public GameObject ZombiePrefab;

    public GameObject CivilianPrefab; 

    /// <summary>
    /// List of living players. 
    /// </summary>
    public List<GameObject> Players = new List<GameObject>(); 

    /// <summary>
    /// Chance a civilian will be spawned in a tick. 
    /// </summary>
    public float SpawnChance = 0.1f;


    public List<PlayerControls> PControlls = new List<PlayerControls>(); 


    public int MaxCivilians = 500;

    /// <summary>
    /// Which button resets the scene. 
    /// </summary>
    public string ReloadButton = "Reset";

    /// <summary>
    /// Return to main menu. 
    /// </summary>
    public string MainMenuButton = "Back"; 

    // Use this for initialization
    void Start () {
        MapGenerator = GetComponent<MapGenerator>();
        InfectBulletScript.SpawnerController = this;
        BaseAI.SpawnerController = this;
        //EntityMovement.SoundFXManager = SoundFXManager.GetComponent<SoundFXManager>(); 
	}
	
	// Update is called once per frame
	void Update () {
        //Unity overloads the == operator so destroyed objects equal null. 
        LivingZombies.RemoveAll(x => x == null);
        Civilians.RemoveAll(x => x == null);
        Players.RemoveAll(x => x == null);

        CheckReload(); 

        if (CivilianPrefab == null)
            return; 

        if (Random.value <= SpawnChance && Civilians.Count < MaxCivilians)
        {
            
            SpawnObject(SpawnPoints.RandomElement(), CivilianPrefab, Animations.Where(x => x.Type == EntityType.Civilian).RandomElement().AnimatorController); 
        }
	}


    /// <summary>
    /// Spawns the given prefab at the given location. 
    /// </summary>
    /// <param name="spawn"></param>
    /// <param name="prefab"></param>
    public void SpawnObject(SpawnPoint spawn, GameObject prefab, RuntimeAnimatorController controller = null)
    {
        var spawnLoc = MapGenerator.Base.GetCellCenterWorld(spawn.SpawnLocation);
        var newObj = Instantiate(prefab, spawnLoc, prefab.transform.rotation);
        if (controller != null)
        {
            newObj.GetComponent<EntityMovement>()?.SetAnimator(controller); 
        }


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

    /// <summary>
    /// Spawns a zombie at the position of the civilian. 
    /// </summary>
    /// <param name="faction"></param>
    /// <param name="civilian"></param>
    public void SpawnZombie(int faction, GameObject civilian)
    {
        var pos = civilian.transform.position;
        Destroy(civilian);
        SpawnZombie(faction, pos); 
    }

    /// <summary>
    /// Spawns a zombie at the position given. 
    /// </summary>
    /// <param name="faction"></param>
    /// <param name="position"></param>
    public void SpawnZombie (int faction, Vector3 position)
    {
        var zombie = Animations.Where(x => x.Type == EntityType.Zombie && x.Faction == faction).RandomElement();

        var zom = Instantiate(ZombiePrefab, position, ZombiePrefab.transform.rotation);
        var owner = zom.GetComponent<EntityOwnership>();
        owner.Faction = faction;
        zom.GetComponent<EntityMovement>()?.SetAnimator(zombie.AnimatorController); 
        LivingZombies.Add(zom);

        SoundFXManager.GamesSoundFXManager.PlaySound(SFXType.Transform); 
    }

    /// <summary>
    /// Spawns in the players. 
    /// </summary>
    /// <param name="playerNo"></param>
    public void SpawnPlayers(int playerNo)
    {
        for (int idx = 1; idx <= playerNo; idx++)
        {
            SpawnPlayer(idx); 
        }
        //insert camera stuff here? 
    }

    /// <summary>
    /// Spawns in the players. 
    /// </summary>
    /// <param name="faction"></param>
    void SpawnPlayer(int faction)
    {
        var animation = Animations.FirstOrDefault(x => x.Faction == faction);
        if (animation == null)
            throw new System.Exception($"Someone done fucked up and there is not a player animation for player {faction}");

        var playercont = PControlls.FirstOrDefault(x => x.PlayerNo == faction); 

        var spawn = SpawnPoints.RandomElement();
        var player = Instantiate(PlayerPrefab, MapGenerator.Base.GetCellCenterWorld(spawn.SpawnLocation), PlayerPrefab.transform.rotation);
        player.GetComponent<EntityMovement>()?.SetAnimator(animation.AnimatorController);
        player.GetComponent<EntityOwnership>().Faction = faction;

        player.GetComponent<PlyControl>()?.ApplyPlayerControlls(playercont); 

        player.GetComponentInChildren<Camera>().rect= GetCamRect(faction);
        Players.Add(player); 

    }

    private Rect GetCamRect (int plyNum)
    {
        switch(plyNum)
        {
            case 1:
                return new Rect(0 ,0.5f, 0.49f, 0.49f);

            case 2:
                return new Rect(0.5f, 0.5f, 0.49f, 0.49f);

            case 3:
                return new Rect(0, 0, 0.49f, 0.49f);

            case 4:
                return new Rect(0.5f, 0, 0.49f, 0.49f);
        }

        return new Rect(0, 0, 0.1f, 0.1f);
    }


    public void CheckReload()
    {
        if (Input.GetButtonUp(ReloadButton) && Players.Count < 2)
        {
            Reload(); 
        }
        else if (Input.GetButtonUp(MainMenuButton))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    /// <summary>
    /// Reloads the current scene. 
    /// </summary>
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
}
