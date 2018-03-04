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
        TD.size = new Vector3(100,200,100);
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
            if (startx >= Tex.height)
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
        splat[0].tileSize = new Vector2(128,1);
        TD.splatPrototypes = splat;
    }
    IEnumerator NewTexture(int x)
    {
        for (int z = 0; z < Tex.height; z++)
        {
            Tex.SetPixel(x,z,RandColor);
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
                int R = Random.Range(NX + 100, 500);
                heights[x, z] = Mathf.SmoothStep(Mathf.PI / R,Mathf.PerlinNoise(x / 50f, z / 50f) / 4f, 0.1f);
                if (heights[x,z] >= 0.2f)
                {
                    //heights[x, z] = Mathf.Lerp(heights[x -1, z - 1],0f,0.1f);
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
            TD.SetHeights(0, 0, heights);
        }

    }
}
