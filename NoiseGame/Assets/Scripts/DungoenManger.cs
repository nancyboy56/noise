using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class DungoenManger : MonoBehaviour
{
    private static DungoenManger _instance;

    public static DungoenManger Instance { get { return _instance; } }

    //private Dictionary<string, GameObject> walls = new Dictionary<string, GameObject>();

        //thread safe
    private ConcurrentDictionary<string, GameObject> walls = new ConcurrentDictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
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
