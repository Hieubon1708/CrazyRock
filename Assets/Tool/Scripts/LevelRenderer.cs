using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRenderer : MonoBehaviour
{
    float minX, maxX, minY, maxY;

    public float minScaleX, maxScaleX, minScaleY, maxScaleY;

    public int minAmount, maxAmount;

    public GameObject[] preGeometries;

    private void Awake()
    {
        maxX = Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x;

        minX = -maxX;

        maxY = Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height).y;

        minY = -maxY;

        Debug.Log("minX " + minX);
        Debug.Log("maxX " + maxX);
        Debug.Log("minY " + minY);
        Debug.Log("maxY " + maxY);

        int randomAmount = Random.Range(minAmount, maxAmount + 1);

        while (randomAmount > 0)
        {
            int randomIndex = Random.Range(0, preGeometries.Length);

            GameObject e = Instantiate(preGeometries[randomIndex], transform);

            float xSCale = Random.Range(minScaleX, maxScaleX);
            float ySCale = Random.Range(minScaleY, maxScaleY);

            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);

            if (e.name.Contains("Triangle") || e.name.Contains("Cylinder")) ySCale = xSCale;

            e.transform.localScale = new Vector3(xSCale, ySCale, 1);
            e.transform.position = new Vector3(x, y, 0);
            e.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 2) == 0 ? 0 : 90);

            randomAmount--;
        }
    }
}
