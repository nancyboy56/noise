using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public enum Map
{
    Wall,
    Ground,
    Start,

}

public class DungoenManger : MonoBehaviour
{
    private static DungoenManger _instance;

    public static DungoenManger Instance { get { return _instance; } }

    //private Dictionary<string, GameObject> walls = new Dictionary<string, GameObject>();

        //thread safe
    private ConcurrentDictionary<string, GameObject> walls = new ConcurrentDictionary<string, GameObject>();
    private Dictionary<string, GameObject> map = new Dictionary<string, GameObject>();

    public int seed = 10;
    public int xMax = 100;
    public int yMax = 10;
    public float worldScale;
    public float noiseScale;
    public Noise n;

    // Start is called before the first frame update
    void Start()
    {
        n = new Noise();
        xMax = 100;
        yMax = 100;
        worldScale = 4;
        noiseScale = 10;
        
        
        
        
        
        spawnWalls();
        
    }



    private void spawnWalls()
    {
        
        for(float x=0; x < xMax; x++)
        {
            for(float y =0; y < yMax; y++)
            {
                float xPerlin = x / xMax * noiseScale;
                float yPerlin = y / yMax * noiseScale;
                 float perlin = Mathf.PerlinNoise(xPerlin + seed, yPerlin + seed);
               // float perlin = (float)n.Get2dNoiseZeroToOne((int)x, (int)y, seed);
              //  Debug.Log("Noise:" + perlin);

                if (perlin < 0.45){

                    GameObject sqaure = Instantiate(Resources.Load("Prefabs/Wall")) as GameObject;
                    sqaure.name = "Wall " + (x / worldScale) + "," + (y / worldScale);
                    sqaure.transform.position = new Vector3((x / worldScale), (y / worldScale), 0);
                }

            }
        }
       // GameObject sqaure = Instantiate(Resources.Load("Prefabs/Wall")) as GameObject;
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
