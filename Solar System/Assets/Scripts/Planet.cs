using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    [Range(2, 256)]
    public int resolution = 10;
    public bool autoUpdate = true;
    public enum FaceRender {All, Top, Bottom, Left, Right, Front, Back};
    public FaceRender faceRender;

    public ShapeSetting shapeSetting;
    public ColorSetting colorSetting;

    [HideInInspector]
    public bool shapeSettingFoldout;
    [HideInInspector]
    public bool colorSettingFoldout;

    ShapeGenerator shapeGenerator = new ShapeGenerator();
    ColorGenerator colorGenerator = new ColorGenerator();

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;  
    TerrainFace[] terrainFaces;

    private void Start()
    {
        GeneratePlanet();
    }
    public void Init()
    {
        shapeGenerator.UpdateSettings(shapeSetting);
        colorGenerator.UpdateSettings(colorSetting);

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] dir = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObject = new GameObject("mesh");
                meshObject.transform.parent = transform;

                meshObject.AddComponent<MeshRenderer>();
                meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSetting.planetMat;
            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, dir[i]);
            bool renderFace = faceRender == FaceRender.All || (int)faceRender - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }
    public void PrintMinMax()
    {
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf) //If its active
            {
                Debug.Log(terrainFaces[i].shapeGenerator.heightMinMax.Max);
                Debug.Log(terrainFaces[i].shapeGenerator.heightMinMax.Min);
                Debug.Log("=============================================");
            }
        }
    }
    public void GeneratePlanet()
    {
        Init();
        GenerateFaces();
        GenerateColors();
    }

    public void OnShapeSettingUpdate()
    {
        if (autoUpdate)
        {
            Init();
            GenerateFaces();
        }
    }

    public void OnColorSettingUpdate()
    {
        if (autoUpdate)
        {
            Init();
            GenerateColors();
        }
    }

    void GenerateFaces()
    {
        for(int i = 0; i < 6; i++)
        {
            if(meshFilters[i].gameObject.activeSelf) //If its active
            {
                terrainFaces[i].CreateMesh();
            }
        }

        colorGenerator.UpdateElevation(shapeGenerator.heightMinMax);
    }

    void GenerateColors()
    {
        colorGenerator.UpdateColors();
        for(int i = 0; i < 6; i++)
        {
            if(meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].UpdateUV(colorGenerator);
            }
        }
    }
}
