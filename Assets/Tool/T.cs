using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class T : MonoBehaviour
{
    public Transform[] corners;
    public Transform[] borders;

    private void Update()
    {
        foreach (var e in corners)
        {
            e.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, e.localScale.z);
        }

        foreach (var e in borders)
        {
            float x = e.localScale.x;
            float y = e.localScale.y;

            if (e.name.Contains("top") || e.name.Contains("bottom")) y = 1 / transform.localScale.y;
            if (e.name.Contains("left") || e.name.Contains("right")) x = 1 / transform.localScale.x;

            e.localScale = new Vector3(x, y, e.localScale.z);
        }
    }
}
