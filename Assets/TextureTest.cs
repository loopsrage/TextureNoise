using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTest : MonoBehaviour {

    // Use this for initialization
    TerrainData TD;
    float[,] heights;
    Texture2D Tex;
    int heightMapRes = 33;
    int startx = 0;
    bool ChangeDir = true;
    Color RandColor;
    void Start () {

        TD = new TerrainData();
        TD.heightmapResolution = heightMapRes;
        AddSplatMap();
        TD.size = new Vector3(50,20,50);
        heights = TD.GetHeights(0,0,TD.heightmapResolution,TD.heightmapResolution);
        GameObject T = Terrain.CreateTerrainGameObject(TD);
        T.transform.position = new Vector3(0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(NewTexture(startx));
        if (ChangeDir)
        {
            startx++;
            if (startx >= Tex.width)
            {
                ChangeDir = false;
                RandColor = Random.ColorHSV();
                StartCoroutine(HeightSet(startx));
            }
        }
        else
        {
            startx--;
            if (startx <= 0)
            {
                ChangeDir = true;
                RandColor = Random.ColorHSV();
                StartCoroutine(HeightSet(startx));
            }
        }
    }
    void AddSplatMap()
    {
        SplatPrototype[] splat = new SplatPrototype[]
        {
            new SplatPrototype()
        };
        Tex = new Texture2D((int)TD.size.x,(int)TD.size.z);
        splat[0].texture = Tex;
        splat[0].tileOffset = new Vector2(0f,0f);
        splat[0].tileSize = new Vector2(1,58);
        TD.splatPrototypes = splat;
    }
    IEnumerator NewTexture(int x)
    {
        for (int z = 0; z < Tex.height; z++)
        {
            Tex.SetPixel(x,z,RandColor);
            Tex.SetPixel(x - 1, z - 1, RandColor.gamma);
            yield return new WaitForSeconds(Time.deltaTime);
            Tex.Apply();
        }
    }
    IEnumerator HeightSet(int NX)
    {
        for (int x = 1; x < TD.heightmapResolution; x++)
        {
            for (int z = 1; z < TD.heightmapResolution; z++)
            {
                heights[x - 1, z - 1] = 0f;
                heights[x, z] = Mathf.PerlinNoise(x / 100f, z / 100f) / 20f;
                yield return new WaitForSeconds(Time.deltaTime);
                TD.SetHeights(0,0,heights);
            }
        }
    }
}
