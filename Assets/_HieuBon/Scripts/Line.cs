using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Line : MonoBehaviour
{
    public int xSegments = 10;
    public int zSegments = 10;

    public float gridWidth = 10f;
    public float gridDepth = 10f;

    LayerMask layerMask;

    Mesh mesh;

    float centerX;

    bool isDrag;

    MeshRenderer meshRenderer;

    void Awake()
    {
        layerMask = LayerMask.GetMask("Wall");

        mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;

        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (xSegments <= 0 || zSegments <= 0) return;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            isDrag = true;

            meshRenderer.enabled = true;
        }

        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            isDrag = false;

            meshRenderer.enabled = false;
        }

        if (isDrag)
        {
            int vertexCountX = xSegments + 1;
            int vertexCountZ = zSegments + 1;
            int numVertices = vertexCountX * vertexCountZ;
            int numTriangles = xSegments * zSegments * 6;

            Vector3[] vertices = new Vector3[numVertices];
            Vector2[] uv = new Vector2[numVertices];
            int[] triangles = new int[numTriangles];

            float cellWidth = gridWidth / xSegments;
            float cellDepth = gridDepth / zSegments;

            for (int z = 0; z < vertexCountZ; z++)
            {
                for (int x = 0; x < vertexCountX; x++)
                {
                    float xPos = x * cellWidth - gridWidth / 2f;
                    float zPos = z * cellDepth - gridDepth / 2f;

                    if (z == 1)
                    {
                        RaycastHit hit;

                        Vector3 origin = transform.position + transform.right * xPos;
                        Vector3 direction = transform.forward;

                        if (Physics.Raycast(origin, direction, out hit, 10, layerMask))
                        {
                            Debug.DrawRay(origin, direction * hit.distance, Color.red);

                            zPos = hit.distance;

                            if (x == 1) centerX = zPos;
                        }
                        else zPos = centerX;
                    }

                    int index = z * vertexCountX + x;
                    vertices[index] = new Vector3(xPos, 0, zPos);

                    uv[index] = new Vector2((float)x / xSegments, (float)z / zSegments);
                }
            }

            int vert = 0;
            int tris = 0;

            for (int z = 0; z < zSegments; z++)
            {
                for (int x = 0; x < xSegments; x++)
                {
                    int v0 = vert;
                    int v1 = vert + 1;
                    int v2 = vert + vertexCountX;
                    int v3 = vert + vertexCountX + 1;

                    triangles[tris + 0] = v0;
                    triangles[tris + 1] = v2;
                    triangles[tris + 2] = v1;

                    triangles[tris + 3] = v1;
                    triangles[tris + 4] = v2;
                    triangles[tris + 5] = v3;

                    vert++;
                    tris += 6;
                }
                vert++;
            }

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }
}
