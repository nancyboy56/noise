using System;
using UnityEngine;

public enum ColourType{
	Hex,
	Red,
	Monochrome,
	HSV,
}

public enum NoiseType
{
	Noise2D,
	Perlin,
}

public class NoiseManger : MonoBehaviour
{
    private static NoiseManger _instance;

    public static NoiseManger Instance { get { return _instance; } }


	public int seed = 10;
	public int xMax = 256;
	public int yMax = 256;
	private int lastXMax = 10;
	private int lastYMax = 10;
	private uint maxHexColours;
	private uint maxUint;
	private uint hexscale;
	public int scale = 5;
	
	/*private Dictionary<string, GameObject> squares = new Dictionary<string, GameObject>();
	private Dictionary<string, SpriteRenderer> renders = new Dictionary<string, SpriteRenderer>();*/
	public ColourType currentColour;
	public NoiseType currentNoise;
	private ColourType lastColour;
	private NoiseType lastNoise;
	private int lastSeed;
	private Noise n;
	private ColourCovert cc;
	private SpriteRenderer sr;
	public GameObject noiseSquare;
	private int lastScale;


	// Start is called before the first frame update
	void Start()
	{
		n = new Noise();
		cc = new ColourCovert();
		lastSeed = seed;
		lastXMax = xMax;
		lastYMax = yMax;
		lastColour = currentColour;
		lastNoise = currentNoise;
		maxHexColours = (uint) Math.Pow(256.0, 3.0);
		maxUint = 4294967295;
		hexscale = maxUint / maxHexColours;
		currentColour = ColourType.Hex;
		currentNoise = NoiseType.Noise2D;
		sr = noiseSquare.GetComponent<SpriteRenderer>();
		lastScale = scale;

		//sr.sprite = Sprite.Create((), new Rect(0, 0, xMax, yMax), new Vector2(0.5f, 0.5f));

		//spawnSquares();

		CreateTextures();
	}


	
	private Texture2D GenerateTexture()
	{
		Texture2D texture = new Texture2D(xMax, yMax);

		for (int x = 0; x < xMax; x++)
		{
			for (int y = 0; y < yMax; y++)
			{
				Color colour = CreateNoise(x, y);
				texture.SetPixel(x, y, colour);
			}
		}
		texture.Apply();
		return texture;
	}



	private Color PerlinColor(int x, int y)
	{
		float xPerlin = (float)x / xMax * scale;
		float yPerlin = (float)y / yMax * scale;
		float perlin = Mathf.PerlinNoise(xPerlin+seed, yPerlin+seed);
		Color c = new Color();

		if (currentColour == ColourType.Hex)
		{

			uint colour = (uint)(perlin * Math.Pow(1, 9));

			colour = colour / hexscale;

			string myHex = colour.ToString("X");
			Debug.Log("hex number: " + x + ", " + myHex);

			ColorUtility.TryParseHtmlString("#" + myHex, out c);

		}
		else if (currentColour == ColourType.HSV)
		{
			uint colour = (uint)(perlin * Math.Pow(1, 9));

			colour = colour / hexscale;

			string myHex = colour.ToString("X");
			//Debug.Log("hex number: " + x + ", " + myHex);

			ColorUtility.TryParseHtmlString("#" + myHex, out c);

			double[] hsv = cc.rgb2hsv(c.r, c.b, c.g);
			double[] rgb = cc.hsv2rgb(hsv[0], hsv[1], hsv[2]);
			c = new Color((float)rgb[0], (float)rgb[1], (float)rgb[2]);
		}
		else if (currentColour == ColourType.Monochrome)
		{
			perlin = (float)RoundUp(perlin, 1);
			c = new Color(perlin, perlin, perlin);
		}
		else if (currentColour == ColourType.Red)
		{
			c = new Color(perlin, 0, 0);
		}
		return c;
	}

	public double RoundUp(double input, int places)
	{
		double multiplier = System.Math.Pow(10, System.Convert.ToDouble(places));
		return System.Math.Ceiling(input * multiplier) / multiplier;
	}

	public void CreateTextures()
	{
		sr.sprite = Sprite.Create(GenerateTexture(), new Rect(0, 0, xMax, yMax), new Vector2(0.5f, 0.5f));
		sr.sprite.texture.filterMode = FilterMode.Point;
	}



	public Color CreateNoise( int x, int y) {

		Color colour = new Color();
	
		if (currentNoise == NoiseType.Perlin)
		{
			colour = PerlinColor(x, y);
		
		} else if(currentNoise == NoiseType.Noise2D)
		{
			colour = Noise2DColour(x, y);
		}
		return colour;
	}

	public Color Noise2DColour(int x, int y)
	{
		Color c = new Color();


		uint noise = n.Get2dNoiseUint(x, y, seed*scale);
		

		if (currentColour == ColourType.Hex)
		{
			uint colour = noise / hexscale;
			

			string myHex = colour.ToString("X");
			//Debug.Log("hex number: " + x + ", " + myHex);

			ColorUtility.TryParseHtmlString("#" + myHex, out c);

		}
		else if (currentColour == ColourType.HSV)
		{
			uint colour = noise / hexscale;

			string myHex = colour.ToString("X");
			//Debug.Log("hex number: " + x + ", " + myHex);

			ColorUtility.TryParseHtmlString("#" + myHex, out c);

			double[] hsv = cc.rgb2hsv(c.r, c.b, c.g);
			double[] rgb = cc.hsv2rgb(hsv[0], hsv[1], hsv[2]);
			c = new Color((float)rgb[0], (float)rgb[1], (float)rgb[2]);
		}
		else if (currentColour == ColourType.Monochrome)
		{
			double newNoise = RoundUp(n.Get2dNoiseZeroToOne(x, y, seed*scale), 1);

			c = new Color((float)newNoise, (float)newNoise, (float)newNoise);
		}
		else if (currentColour == ColourType.Red)
		{
			double newNoise = RoundUp(n.Get2dNoiseZeroToOne(x, y, seed*scale), 1);

			c = new Color((float)newNoise, 0, 0);
		}

		return c;
	}

	// Update is called once per frame
	void Update()
	{
		if (lastSeed != seed)
		{
			CreateTextures();
			lastSeed = seed;
		}
		else if (lastXMax != xMax || lastYMax != yMax)
		{
			CreateTextures();
			lastXMax = xMax;
			lastYMax = yMax;
		}
		else if (currentColour != lastColour)
		{
			CreateTextures();
			lastColour = currentColour;
		}
		else if (currentNoise != lastNoise)
		{
			CreateTextures();
			lastNoise = currentNoise;
		}
		else if (scale != lastScale)
		{
			CreateTextures();
			lastScale = scale;
		}

		
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

	// every pixel is a square
	/*private void spawnSquares()
	{
		if (squares.Count != 0)
		{
			foreach(GameObject s in squares.Values)
			{
				Destroy(s);
			}
			squares = new Dictionary<string, GameObject>();
			renders = new Dictionary<string, SpriteRenderer>();
		}
		Debug.Log("sqaures:" + squares.Count);
		for (int i = -yMax; i < yMax; i++)
		{
			for (int j = -xMax; j < xMax; j++)
			{

				GameObject sqaure = Instantiate(Resources.Load("Prefabs/Square")) as GameObject;
				sqaure.transform.position = new Vector3(i, j, 0);
				uint noise = UseNoiseType(i, j);
				
				squares.Add(i+","+j, sqaure);
				
				
				SpriteRenderer sr = sqaure.GetComponent<SpriteRenderer>();
				renders.Add(i + "," + j, sr);

				UseColourType(noise, i, j);
			}
		}
	}
*/



}


