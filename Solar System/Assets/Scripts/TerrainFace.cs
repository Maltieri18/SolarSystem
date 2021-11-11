using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    Mesh mesh;
    int res;
    Vector3 upwardsVec;
    Vector3 aAxis;
    Vector3 bAxis;

    public TerrainFace(Mesh mesh, int res, Vector3 upwardsVec)
    {
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

        for(int y = 0; y < res; y++)
        {
            
            for(int x = 0; x < res; x++)
            {
                int i = x + y * res;
                Vector2 proportion = new Vector2(x, y) / (res - 1); //Scales down (x, y) in terms of the resolution
                Vector3 pointOnCube = upwardsVec + (proportion.x - 0.5f) * 2 * aAxis + (proportion.y - 0.5f) * 2 * bAxis; //Rescale in terms of (-1, 1)
                Vector3 pointOnSphere = pointOnCube.normalized;
                vertices[i] = pointOnSphere;

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
        mesh.normals = vertices;
        //mesh.RecalculateNormals();
    }
}
