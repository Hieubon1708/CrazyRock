using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRenderer : MonoBehaviour
{
    float minX, maxX, minY, maxY;

    public float minScaleX, maxScaleX, minScaleY, maxScaleY;

    public GameObject[] preGeometries;

    public int col, row;

    public GameObject p;

    public List<Area> areas;

    Vector2[][] ars;

    private void Awake()
    {
        maxX = Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x;

        minX = -maxX;

        maxY = Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height).y;

        minY = -maxY;

        float width = maxX * 2;
        float height = maxY * 2;

        int pointRow = row + 1;
        int pointCol = col + 1;

        float cellWidth = width / row;
        float cellHeight = height / col;

        ars = new Vector2[pointRow][];

        for (int x = 0; x < pointRow; x++)
        {
            Vector2[] ar = new Vector2[pointCol];

            for (int y = 0; y < pointCol; y++)
            {
                float xPos = x * cellWidth - width / 2f;
                float yPos = y * cellHeight - height / 2f;

                ar[y] = new Vector2(xPos, yPos);
                Instantiate(p, new Vector3(xPos, yPos, 0), Quaternion.identity);
            }

            ars[x] = ar;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Ren();
        }
    }

    private void Ren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        areas = new List<Area>();

        /*for (int x = 0; x < pointRow; x++)
        {
            for (int y = 0; y < pointCol; y++)
            {
                Debug.Log("x = " + x + " " + " y = " + y + " " + ars[x][y]);
            }
        }*/

        for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < col - 1; y++)
            {
                areas.Add(new Area(ars[x][y], ars[x][y + 1], ars[x + 1][y], ars[x + 1][y + 1]));
            }
        }

        int randomAmount = Random.Range(1, areas.Count + 1);

        while (randomAmount > 0)
        {
            int randomIndex = Random.Range(0, areas.Count);

            GameObject e = Instantiate(preGeometries[0], transform);

            float xSCale = Random.Range(minScaleX, maxScaleX);
            float ySCale = Random.Range(minScaleY, maxScaleY);

            float x = Random.Range(areas[randomIndex].MinX, areas[randomIndex].MaxX);
            float y = Random.Range(areas[randomIndex].MinY, areas[randomIndex].MaxY);

            e.transform.localScale = new Vector3(xSCale, ySCale, 1);

            e.transform.position = new Vector3(x, y, 0);

            randomAmount--;

            areas.RemoveAt(randomIndex);
        }
    }
}

[System.Serializable]
public class Area
{
    public Vector2 a, b, c, d;

    public Area(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
    }

    public float MinX { get { return a.x; } }
    public float MaxX { get { return c.x; } }
    public float MinY { get { return a.y; } }
    public float MaxY { get { return b.y; } }
}
