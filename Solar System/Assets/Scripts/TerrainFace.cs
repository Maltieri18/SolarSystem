using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{ 
    public ShapeGenerator shapeGenerator;
    Mesh mesh;
    int res;
    Vector3 upwardsVec;
    Vector3 aAxis;
    Vector3 bAxis;

    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int res, Vector3 upwardsVec)
    {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.res = res;
        this.upwardsVec = upwardsVec;

        aAxis = new Vector3(upwardsVec.y, upwardsVec.z, upwardsVec.x);
        bAxis = Vector3.Cross(upwardsVec, aAxis);
    }

    public void CreateMesh()
    {

        Vector3[] vertices = new Vector3[res * res];
        int[] trianglesIndex = new int[(res - 1) * (res - 1) * 2 * 3];
        int triIndex = 0;
        Vector2[] uv = mesh.uv;

        for(int y = 0; y < res; y++)
        {
            
            for(int x = 0; x < res; x++)
            {
                int i = x + y * res;
                Vector2 proportion = new Vector2(x, y) / (res - 1); //Scales down (x, y) in terms of the resolution
                Vector3 pointOnCube = upwardsVec + (proportion.x - 0.5f) * 2 * aAxis + (proportion.y - 0.5f) * 2 * bAxis; //Rescale in terms of (-1, 1)
                Vector3 pointOnSphere = ToSphere(pointOnCube);
                vertices[i] = shapeGenerator.GetPointOnPlanet(pointOnSphere);

                if(x != res - 1 && y != res - 1)
                {
                    trianglesIndex[triIndex] = i;
                    trianglesIndex[triIndex + 1] = i + res + 1;
                    trianglesIndex[triIndex + 2] = i + res;

                    trianglesIndex[triIndex + 3] = i;
                    trianglesIndex[triIndex + 4] = i + 1;
                    trianglesIndex[triIndex + 5] = i + res + 1;

                    triIndex += 6;
                }
            }
            

        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = trianglesIndex;
        //mesh.normals = vertices;
        mesh.RecalculateNormals();
        if(mesh.uv.Length == uv.Length) mesh.uv = uv;
    }

    public static Vector3 ToSphere(Vector3 point)
    {
        float xsqr = point.x * point.x;
        float ysqr = point.y * point.y;
        float zsqr = point.z * point.z;
        float x = point.x * Mathf.Sqrt(1 - (ysqr + zsqr) / 2 + (ysqr * zsqr) / 3);
        float y = point.y * Mathf.Sqrt(1 - (zsqr + xsqr) / 2 + (zsqr * xsqr) / 3);
        float z = point.z * Mathf.Sqrt(1 - (xsqr + ysqr) / 2 + (xsqr * ysqr) / 3);
        return new Vector3(x, y, z);
    }

    public void UpdateUV(ColorGenerator colorGenerator)
    {
        Vector2[] uv = new Vector2[res * res];

        for (int y = 0; y < res; y++)
        {

            for (int x = 0; x < res; x++)
            {
                int i = x + y * res;
                Vector2 proportion = new Vector2(x, y) / (res - 1); //Scales down (x, y) in terms of the resolution
                Vector3 pointOnCube = upwardsVec + (proportion.x - 0.5f) * 2 * aAxis + (proportion.y - 0.5f) * 2 * bAxis; //Rescale in terms of (-1, 1)
                Vector3 pointOnSphere = ToSphere(pointOnCube);

                uv[i] = new Vector2(colorGenerator.BiomePercentPoint(pointOnSphere), 0);
            }
        }
        mesh.uv = uv;
    }
}
