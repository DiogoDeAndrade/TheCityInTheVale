using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class TerrainGen : MonoBehaviour
{
    public int      resolution = 513;
    public float    freqX = 1.0f;
    public float    freqY = 1.0f;
    public Vector3  size = new Vector3(500.0f, 100.0f, 500.0f);
    public float    minHeight = 0.0f;
    public int      heightRes = 0;

    Terrain terrain;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    [Button("Generate Terrain")]
    void GenTerrain()
    { 
        terrain = GetComponent<Terrain>();

        var terrainData = terrain.terrainData;

        terrainData.heightmapResolution = resolution;
        terrainData.size = size;

        float[,] heights = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
        float    h;

        for (int y = 0; y < terrainData.heightmapResolution; y++)
        {
            for (int x = 0; x < terrainData.heightmapResolution; x++)
            {
                h = Mathf.PerlinNoise((float)x * freqX / (float)terrainData.heightmapResolution, (float)y * freqY / (float)terrainData.heightmapResolution);

                if (h < minHeight) h = minHeight;

                h = (h - minHeight) / (1.0f - minHeight);

                if (heightRes > 0)
                {
                    h = Mathf.Floor(h * heightRes) / heightRes;
                }

                heights[x, y] = h;
            }
        }


        terrainData.SetHeights(0, 0, heights);        

        terrain.terrainData = terrainData;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
