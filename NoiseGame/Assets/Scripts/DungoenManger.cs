using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public enum Tile
{
    Wall,
    Ground,
    Start,
    Edge,

}

public class DungoenManger : MonoBehaviour
{
    private static DungoenManger _instance;

    public static DungoenManger Instance { get { return _instance; } }

    //private Dictionary<string, GameObject> walls = new Dictionary<string, GameObject>();

        //thread safe
    private Dictionary<string, GameObject> spawnedMap = new Dictionary<string, GameObject>();
    private Dictionary<string, Tile> mapBasic = new Dictionary<string, Tile>();
    private Dictionary<Tile, GameObject> tileGameObject = new Dictionary<Tile, GameObject>();

    public int seed = 10;
    public int xMax = 100;
    public int yMax = 10;
    public float worldScale;
    public float noiseScale;
    public Noise n;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
        n = new Noise();
        xMax = 100;
        yMax = 100;
        worldScale = 4;
        noiseScale = 8;
        seed = Random.Range(1, 1000);
        //seed = Random.Range(1, 1000);
        player = GameObject.FindGameObjectWithTag("Player");

        SetUpMapObjects();

        CreateBasicMap();
        ValidateMap();
        SpawnMap();

    }



    public void DeleteMap()
    {
        //foreach(GameObject tile in mapBasic)
    }

    private void SetUpMapObjects()
    {
        tileGameObject[Tile.Ground] = Resources.Load("Prefabs/Ground") as GameObject;
        tileGameObject[Tile.Wall] = Resources.Load("Prefabs/Wall") as GameObject;
        tileGameObject[Tile.Start] = Resources.Load("Prefabs/Ground") as GameObject;
        tileGameObject[Tile.Edge] = Resources.Load("Prefabs/Wall") as GameObject;

    }

    private void ValidateMap()
    {
        IsPlayerStuck();
    }

    private bool IsPlayerStuck()
    {
        /* if (map.Count != 0)
         {

             for ()
         }*/

        return true;
    }

    //creating basic map not spawning any game objects yet
    private void CreateBasicMap()
    { 
        for(float x=-xMax; x < xMax; x++)
        {
            for(float y = -yMax; y < yMax; y++)
            {
                float perlin = Perlin(x, y);
                AddTile(x, y, perlin);
            }
        }
        // start for the player
        mapBasic["0,0"] = Tile.Start;
    }

    private void SpawnMap()
    {
        for (float x = -xMax; x < xMax; x++)
        {
            for (float y = -yMax; y < yMax; y++)
            {
                Tile currentTile = mapBasic[CorrdinatiesToString(x,y)];
                GameObject tileObject = tileGameObject[currentTile];
                Instantiate(tileObject);
                tileObject.transform.position = new Vector3(x / worldScale, y / worldScale, 0);
                CreateTileName(x, y, tileObject, currentTile);
                spawnedMap.Add(CorrdinatiesToString(x, y), tileObject);
            }
        }
    }

    private string CorrdinatiesToString(float x, float y)
    {
        return x + "," + y;
    }

    private void CreateTileName(float x, float y, GameObject tileObject, Tile currentTile)
    {
        string name = "Tile";
        if (currentTile == Tile.Edge)
        {
            name = "Edge";
        }
        else if (currentTile == Tile.Ground)
        {
            name = "Ground";
        } else if(currentTile == Tile.Start)
        {
            name = "Start";
        } else if (currentTile == Tile.Wall)
        {
            name = "Wall";
        }

        //change tile object name
        tileObject.name = name+": "+ (x / worldScale) + ", " + (y / worldScale);
    }


    private void AddTile(float x, float y, float perlin)
    {
        if (IsEdge(x, y))
        {
            mapBasic.Add(CorrdinatiesToString(x, y), Tile.Wall);
        }
        else if (ShouldCreateWalls(perlin))
        {
            mapBasic.Add(CorrdinatiesToString(x, y), Tile.Wall);
        }
        else
        {
            mapBasic.Add(CorrdinatiesToString(x, y), Tile.Ground);
        }
    }

    private bool IsEdge(float x, float y)
    {
        return x == xMax-1 || x == -xMax || y == -yMax || y == yMax-1;
    }

    private bool ShouldCreateWalls(float perlin)
    {
        return perlin < 0.45;
    }

    private float Perlin(float x, float y)
    {
        float xPerlin = x / xMax * noiseScale;
        float yPerlin = y / yMax * noiseScale;
        float perlin = Mathf.PerlinNoise(xPerlin + seed, yPerlin + seed);
        return perlin;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

    }
}
