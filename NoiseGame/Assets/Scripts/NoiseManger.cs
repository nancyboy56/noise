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
	NoisePercent,
}

public class NoiseManger : MonoBehaviour
{
    private static NoiseManger _instance;

    public static NoiseManger Instance { get { return _instance; } }


	public int seed = 10;
	public int xMax = 100;
	public int yMax = 100;
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

		sr.sprite = Sprite.Create(GenerateTexture(), new Rect(0, 0, xMax, yMax), new Vector2(0.5f, 0.5f));

		//spawnSquares();
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
		float perlin = Mathf.PerlinNoise(xPerlin, yPerlin);
		Color c = new Color();

		if (currentColour == ColourType.Hex)
		{
			
			uint colour = (uint)(perlin * Math.Pow(1,9));

			string myHex = colour.ToString("X");
			//Debug.Log("hex number: " + x + ", " + myHex);

			ColorUtility.TryParseHtmlString("#" + myHex, out c);

		}
		else if (currentColour == ColourType.HSV)
		{
			uint colour = (uint)(perlin * Math.Pow(1, 9));

			string myHex = colour.ToString("X");
			//Debug.Log("hex number: " + x + ", " + myHex);

			ColorUtility.TryParseHtmlString("#" + myHex, out c);

			//cc.rgb2hsv()
		}
		else if (currentColour == ColourType.Monochrome)
		{
			perlin = (float)RoundUp(perlin, 1);
			c = new Color(perlin, perlin, perlin);
		}
		else if (currentColour == ColourType.Monochrome)
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
	}



	public Color CreateNoise( int x, int y) {

		Color colour = new Color();
	
		if (currentNoise == NoiseType.Perlin)
		{
			PerlinColor(x, y);
		/*	if (currentColour == ColourType.Hex)
			{
				
			}
			else if (currentColour == ColourType.HSV)
			{

			}
			else if (currentColour == ColourType.Monochrome)
			{
				
			}
			else if (currentColour == ColourType.Monochrome)
			{
				Red();
			}*/
		} else if(currentNoise == NoiseType.Noise2D)
		{

		}
		if(currentColour == ColourType.Hex)
		{
			//HexColour(noise, i, j);
		} else if (currentColour == ColourType.HSV)
		{

		} 
		else if(currentColour == ColourType.Monochrome)
		{
			//Monocrhome(noise,i,j);
		}
		else if (currentColour == ColourType.Monochrome)
		{
			
		}
		else
		{
			///HexColour(noise, i, j);
		}

		return colour;
	}

/*	//need to add a seed some how unsure yet
	public uint Perlin(int x, int y)
	{
		//convert to an uint bc thats what ive been using this whole time 
		// it shouldnt change anything
		return (uint)(Mathf.PerlinNoise(x, y) * Math.Pow(1.0, 9.0));
	}*/

	/*private void UseNoiseType(int , int j)
	{
		uint noise =0;
		if(currentNoise== NoiseType.Noise2D)
		{
			noise = n.Get2dNoiseUint(x, j, seed);

		} else if (currentNoise == NoiseType.NoisePercent)
		{
			double noiseDecmial = n.Get2dNoiseNegOneToOne(i, j, seed);
			noise = (uint)(noiseDecmial * Math.Pow(1.0, 9.0));
		} 
		else if(currentNoise== NoiseType.Perlin)
		{
			sr.sprite = Sprite.Create(GenerateTexture(), new Rect(0, 0, xMax, yMax), new Vector2(0.5f, 0.5f));
		}
		else
		{
			noise = n.Get2dNoiseUint(i, j, seed);
		}
		
	}*/

	private Color HexColour( int x, int y)
	{
		uint noise = n.Get2dNoiseUint(x, y, seed);
		Color newColour;
		uint colour = noise / hexscale;

		string myHex = colour.ToString("X");
		//Debug.Log("hex number: " + x + ", " + myHex);
		
		ColorUtility.TryParseHtmlString("#" + myHex, out newColour);

		//Debug.Log("Colour:" + newColour.r + "" + newColour.g + "" + newColour.b);
		//renders[i + "," + j].color = newColour;
		return newColour;
	}

	private void Monocrhome(uint noise, int i, int j)
	{
		Color newColour;
		uint colour = noise / hexscale;

		string myHex = colour.ToString("X");
		Debug.Log("hex number: " + i + ", " + myHex);

		ColorUtility.TryParseHtmlString("#" + myHex, out newColour);

		Debug.Log("Colour:" + newColour.r + "" + newColour.g + "" + newColour.b);
		//renders[i + "," + j].color = newColour;
	}


	// Update is called once per frame
	void Update()
	{
		if (lastSeed != seed)
		{
			//ShowNoise();
			lastSeed = seed;
		}

		if (lastXMax != xMax || lastYMax != yMax)
		{
			//ShowNoise();
			lastXMax = xMax;
			lastYMax = yMax;
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




}


