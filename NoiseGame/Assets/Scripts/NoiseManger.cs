using System;
using System.Collections;
using System.Collections.Generic;
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
	public int xMax = 10;
	public int yMax = 10;
	private int lastXMax = 10;
	private int lastYMax = 10;
	private uint maxHexColours;
	private uint maxUint;
	private uint hexscale;
	private double ONE_OVER_MAX_INT = 1.0 / 0x7FFFFFFF;
	private double ONE_OVER_MAX_UINT = 1.0 / 0xFFFFFFFF;
	private Dictionary<string, GameObject> squares = new Dictionary<string, GameObject>();
	private Dictionary<string, SpriteRenderer> renders = new Dictionary<string, SpriteRenderer>();
	public ColourType currentColour;
	public NoiseType currentNoise;
	private ColourType lastColour;
	private NoiseType lastNoise;
	private int lastSeed;


	// Start is called before the first frame update
	void Start()
	{
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

		//spawnSquares();
	}

	private void spawnSquares()
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

	private void removeSquares()
	{
		
	}

	private void updateColour()
	{
		if(squares.Count > 0)
		{
			for (int i = -yMax; i < yMax; i++)
			{
				for (int j = -xMax; j < xMax; j++)
				{
					GameObject sqaure= squares[i + "," + j];
					
					uint noise = UseNoiseType(i, j);
					UseColourType(noise, i, j);
					
				}
			}
		}
	}

	public void UseColourType(uint noise, int i, int j)
	{
		if(currentColour == ColourType.Hex)
		{
			HexColour(noise, i, j);
		} else if (currentColour == ColourType.HSV)
		{

		} 
		else if(currentColour == ColourType.Monochrome)
		{
			Monocrhome(noise,i,j);
		}
		else if (currentColour == ColourType.Monochrome)
		{
			Red();
		}
		else
		{
			HexColour(noise, i, j);
		}


	}

	//need to add a seed some how unsure yet
	public uint Perlin(int x, int y)
	{
		//convert to an uint bc thats what ive been using this whole time 
		// it shouldnt change anything
		return (uint) (Mathf.PerlinNoise(x, y)* Math.Pow(1.0, 9.0));
	}

	private uint UseNoiseType(int i, int j)
	{
		uint noise =0;
		if(currentNoise== NoiseType.Noise2D)
		{
			noise = Get2dNoiseUint(i, j, seed);
		} else if (currentNoise == NoiseType.NoisePercent)
		{
			double noiseDecmial = Get2dNoiseNegOneToOne(i, j, seed);
			noise = (uint)(noiseDecmial * Math.Pow(1.0, 9.0));
		} 
		else if(currentNoise== NoiseType.Perlin)
		{
			noise = Perlin(i, j);
		}
		else
		{
			noise = Get2dNoiseUint(i, j, seed);
		}
		return noise;
	}

	private void HexColour(uint noise, int i, int j)
	{
		Color newColour;
		uint colour = noise / hexscale;

		string myHex = colour.ToString("X");
		Debug.Log("hex number: " + i + ", " + myHex);
		
		ColorUtility.TryParseHtmlString("#" + myHex, out newColour);

		Debug.Log("Colour:" + newColour.r + "" + newColour.g + "" + newColour.b);
		renders[i + "," + j].color = newColour;
	}

	private void Monocrhome(uint noise, int i, int j)
	{
		Color newColour;
		uint colour = noise / hexscale;

		string myHex = colour.ToString("X");
		Debug.Log("hex number: " + i + ", " + myHex);

		ColorUtility.TryParseHtmlString("#" + myHex, out newColour);

		Debug.Log("Colour:" + newColour.r + "" + newColour.g + "" + newColour.b);
		renders[i + "," + j].color = newColour;
	}


	private void Red()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (lastSeed != seed)
		{
			spawnSquares();
			lastSeed = seed;
		}

		if (lastXMax != xMax)
		{
			spawnSquares();
			lastXMax = xMax;
		}

		if (lastYMax != yMax)
		{
			spawnSquares();
			lastYMax = yMax;
		}
	}

	public uint SquirrelNoise5(int positionX, int seed)
	{
		uint SQ5_BIT_NOISE1 = 0xd2a80a3f;    // 11010010101010000000101000111111
		uint SQ5_BIT_NOISE2 = 0xa884f197;    // 10101000100001001111000110010111
		uint SQ5_BIT_NOISE3 = 0x6C736F4B; // 01101100011100110110111101001011
		uint SQ5_BIT_NOISE4 = 0xB79F3ABB;    // 10110111100111110011101010111011
		uint SQ5_BIT_NOISE5 = 0x1b56c4f5;    // 00011011010101101100010011110101

		uint mangledBits = (uint)positionX;
		mangledBits *= SQ5_BIT_NOISE1;
		mangledBits += (uint)seed;
		mangledBits ^= (mangledBits >> 9);
		mangledBits += SQ5_BIT_NOISE2;
		mangledBits ^= (mangledBits >> 11);
		mangledBits *= SQ5_BIT_NOISE3;
		mangledBits ^= (mangledBits >> 13);
		mangledBits += SQ5_BIT_NOISE4;
		mangledBits ^= (mangledBits >> 15);
		mangledBits *= SQ5_BIT_NOISE5;
		mangledBits ^= (mangledBits >> 17);
		return mangledBits;
	}

	public uint Get1dNoiseUint(int positionX, int seed)
	{
		return SquirrelNoise5(positionX, seed);
	}

	public uint Get2dNoiseUint(int indexX, int indexY, int seed)
	{
		int PRIME_NUMBER = 198491317; // Large prime number with non-boring bits
		return SquirrelNoise5(indexX + (PRIME_NUMBER * indexY), seed);
	}


	public uint Get3dNoiseUint(int indexX, int indexY, int indexZ, int seed)
	{
		int PRIME1 = 198491317; // Large prime number with non-boring bits
		int PRIME2 = 6542989; // Large prime number with distinct and non-boring bits
		return SquirrelNoise5(indexX + (PRIME1 * indexY) + (PRIME2 * indexZ), seed);
	}


	public uint Get4dNoiseUint(int indexX, int indexY, int indexZ, int indexT, int seed)
	{
		int PRIME1 = 198491317; // Large prime number with non-boring bits
		int PRIME2 = 6542989; // Large prime number with distinct and non-boring bits
		int PRIME3 = 357239; // Large prime number with distinct and non-boring bits
		return SquirrelNoise5(indexX + (PRIME1 * indexY) + (PRIME2 * indexZ) + (PRIME3 * indexT), seed);
	}


	public double Get1dNoiseZeroToOne(int index, int seed)
	{
		return ONE_OVER_MAX_UINT * SquirrelNoise5(index, seed);
	}

	public double Get2dNoiseZeroToOne(int indexX, int indexY, int seed)
	{
		return ONE_OVER_MAX_UINT * Get2dNoiseUint(indexX, indexY, seed);
	}


	public double Get3dNoiseZeroToOne(int indexX, int indexY, int indexZ, int seed)
	{
		return ONE_OVER_MAX_UINT * Get3dNoiseUint(indexX, indexY, indexZ, seed);
	}

	public double Get4dNoiseZeroToOne(int indexX, int indexY, int indexZ, int indexT, int seed)
	{
		return ONE_OVER_MAX_UINT * (double)Get4dNoiseUint(indexX, indexY, indexZ, indexT, seed);
	}

	public double Get1dNoiseNegOneToOne(int index, int seed)
	{
		return ONE_OVER_MAX_INT * SquirrelNoise5(index, seed);
	}

	public double Get2dNoiseNegOneToOne(int indexX, int indexY, int seed)
	{
		return ONE_OVER_MAX_INT * Get2dNoiseUint(indexX, indexY, seed);
	}

	public double Get3dNoiseNegOneToOne(int indexX, int indexY, int indexZ, int seed)
	{
		return ONE_OVER_MAX_INT * Get3dNoiseUint(indexX, indexY, indexZ, seed);
	}

	public double Get4dNoiseNegOneToOne(int indexX, int indexY, int indexZ, int indexT, int seed)
	{
		return ONE_OVER_MAX_INT * Get4dNoiseUint(indexX, indexY, indexZ, indexT, seed);
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
