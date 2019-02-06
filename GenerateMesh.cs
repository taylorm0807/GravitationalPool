using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GenerateMesh : MonoBehaviour
{
    private int xSize, ySize;
    private Vector3[] vertices;
    private Mesh mesh;
    GameObject[] walls; 

    void Start()
    {
        
    }


    private void Awake()
    {
        walls = new GameObject[4];
        walls[0] = GameObject.Find("Top Wall");
        walls[1] = GameObject.Find("Right Wall");
        walls[2] = GameObject.Find("Bottom Wall");
        walls[3] = GameObject.Find("Left Wall");
        xSize = (int)(walls[1].transform.position.x - walls[3].transform.position.x);
        ySize = (int)(walls[0].transform.position.z - walls[2].transform.position.z);
        Generate();
    }

    private void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, 0, y);
            }
        }
        mesh.vertices = vertices;

        int[] triangles = new int[xSize * ySize * 6];
        for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
        {
            for (int x = 0; x < xSize; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                triangles[ti + 5] = vi + xSize + 2;
            }
        }
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

}
