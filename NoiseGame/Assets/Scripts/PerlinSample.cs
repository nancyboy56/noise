using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinSample : MonoBehaviour
{
    // Width and height of the texture in pixels.
    public int width;
    public int height;



    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale = 5.0F;

  //  private Texture2D noiseTex;
   // private Color[] pix;
    private SpriteRenderer rend;

    void Start()
    {
        //Random.InitState(4815162342);
        Debug.Log("Perlin!!!");
        rend = GetComponent<SpriteRenderer>();
        Debug.Log(rend);
        //Debug.Log()

        // Set up the texture and a Color array to hold pixels during processing.
        //noiseTex = new Texture2D(width, height);
        //pix = new Color[noiseTex.width * noiseTex.height];
        rend.sprite = Sprite.Create(GenerateTexture(), new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
       // rend.material.mainTexture= GenerateTexture();
        //rend.color = new Color(44/255f, 44/255f, 44/255f);

    }



    private Texture2D GenerateTexture()
    {
       
        Texture2D texture = new Texture2D(width, height);

        for(int x =0; x< width; x++)
        {
            for(int y =0; y<height; y++)
            {
                //Debug.Log("Texture!!");
                Color colour = CalculateColour(x, y);
                texture.SetPixel(x, y, colour);
            }
        }
        texture.Apply();
        return texture;
    }

    private Color CalculateColour(int x, int y)
    {
        float xPerlin = (float)x / width *scale;
        float yPerlin = (float)y / height *scale;
        float perlin= (float)RoundUp(Mathf.PerlinNoise(xPerlin, yPerlin), 1);
        return new Color(perlin, perlin, perlin);
    }

    public double RoundUp(double input, int places)
    {
        double multiplier = System.Math.Pow(10, System.Convert.ToDouble(places));
        return System.Math.Ceiling(input * multiplier) / multiplier;
    }

    void Update()
    {
        
    }
}
