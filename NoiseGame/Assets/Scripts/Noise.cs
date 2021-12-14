using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Noise : MonoBehaviour
{
	public int seed =10;
	public int xMax =10;
	public int yMax = 10;
	private uint maxColours;
	private uint maxValue;
	private uint scale;
	private double ONE_OVER_MAX_INT = 1.0 / 0x7FFFFFFF;
	private double ONE_OVER_MAX_UINT = 1.0 / 0xFFFFFFFF;
	private List<GameObject> squares = new List<GameObject>();
	private List<SpriteRenderer> renders = new List<SpriteRenderer>();
	/*public Dropdown colourType;
	public Dropdown noiseType;
	public InputField xInput;
	public InputField yInput;
	public InputField seedInput;*/
	

	// Start is called before the first frame update
	void Start()
    {
		maxColours = (uint)Math.Pow( 256.0 , 3.0 );
		maxValue = 4294967295;
		scale = maxValue / maxColours;

		spawnSquares();
	}

	private void spawnSquares()
	{
		for (int i = -10; i < yMax; i++)
		{
			for (int j = -10; j < xMax; j++)
			{

				GameObject sqaure = Instantiate(Resources.Load("Prefabs/Square")) as GameObject;
				sqaure.transform.position = new Vector3(i, j, 0);
				uint noise = Get2dNoiseUint(i, j, seed);
				uint colour = noise / scale;
				squares.Add(sqaure);
				string myHex = colour.ToString("X");
				Debug.Log("hex number: " + i + ", " + myHex);
				SpriteRenderer sr = sqaure.GetComponent<SpriteRenderer>();
				Color newColour;
				ColorUtility.TryParseHtmlString("#" + myHex, out newColour);

				Debug.Log("Colour:" + newColour.r + "" + newColour.g + "" + newColour.b);
				sr.color = newColour;
			}
		}
	}



	private void ColourRandom()
	{

	}

	private void Monocrhome()
	{

	}

	private void RedHue()
	{

	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public uint SquirrelNoise5(int positionX, int seed)
	{
		uint SQ5_BIT_NOISE1 = 0xd2a80a3f;    // 11010010101010000000101000111111
		uint SQ5_BIT_NOISE2 = 0xa884f197;    // 10101000100001001111000110010111
		uint SQ5_BIT_NOISE3 = 0x6C736F4B; // 01101100011100110110111101001011
		uint SQ5_BIT_NOISE4 = 0xB79F3ABB;    // 10110111100111110011101010111011
		uint SQ5_BIT_NOISE5 = 0x1b56c4f5;    // 00011011010101101100010011110101

		uint mangledBits = (uint) positionX;
		mangledBits *= SQ5_BIT_NOISE1;
		mangledBits += (uint) seed;
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

	public uint Get1dNoiseUint(int positionX,  int seed)
	{
		return SquirrelNoise5(positionX, seed);
	}

	 public uint Get2dNoiseUint(int indexX, int indexY,  int seed)
	{
		int PRIME_NUMBER = 198491317; // Large prime number with non-boring bits
		return SquirrelNoise5(indexX + (PRIME_NUMBER * indexY), seed);
	}


	public uint Get3dNoiseUint(int indexX, int indexY, int indexZ,  int seed)
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


	public double Get1dNoiseZeroToOne(int index,  int seed)
	{
		return ONE_OVER_MAX_UINT * SquirrelNoise5(index, seed);
	}

	public double Get2dNoiseZeroToOne(int indexX, int indexY,  int seed)
	{
		return ONE_OVER_MAX_UINT * Get2dNoiseUint(indexX, indexY, seed);
	}


	public double Get3dNoiseZeroToOne(int indexX, int indexY, int indexZ,  int seed)
	{
		return ONE_OVER_MAX_UINT * Get3dNoiseUint(indexX, indexY, indexZ, seed);
	}

	public double Get4dNoiseZeroToOne(int indexX, int indexY, int indexZ, int indexT,  int seed)
	{
		return ONE_OVER_MAX_UINT * (double)Get4dNoiseUint(indexX, indexY, indexZ, indexT, seed);
	}

	public double Get1dNoiseNegOneToOne(int index,  int seed)
	{
		return ONE_OVER_MAX_INT * SquirrelNoise5(index, seed);
	}

	public double Get2dNoiseNegOneToOne(int indexX, int indexY,  int seed)
	{
		return ONE_OVER_MAX_INT * Get2dNoiseUint(indexX, indexY, seed);
	}

	public double Get3dNoiseNegOneToOne(int indexX, int indexY, int indexZ,  int seed)
	{
		return ONE_OVER_MAX_INT * Get3dNoiseUint(indexX, indexY, indexZ, seed);
	}

	public double Get4dNoiseNegOneToOne(int indexX, int indexY, int indexZ, int indexT,  int seed)
	{
		return ONE_OVER_MAX_INT * Get4dNoiseUint(indexX, indexY, indexZ, indexT, seed);
	}
}
