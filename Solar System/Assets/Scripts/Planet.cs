using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{

    [Range(2, 256)]
    public int resolution = 10;
    public bool autoUpdate = true;

    public ShapeSetting shapeSetting;
    public ColorSetting colorSetting;

    [HideInInspector]
    public bool shapeSettingFoldout;
    [HideInInspector]
    public bool colorSettingFoldout;

    ShapeGenerator shapeGenerator;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    public void Init()
    {
        shapeGenerator = new ShapeGenerator(shapeSetting);

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

                meshObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, dir[i]);
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
        foreach (TerrainFace face in terrainFaces)
        {
            face.CreateMesh();
        }
    }

    void GenerateColors()
    {
        foreach(MeshFilter filter in meshFilters)
        {
            filter.GetComponent<MeshRenderer>().sharedMaterial.color = colorSetting.color;
        }
    }
}
