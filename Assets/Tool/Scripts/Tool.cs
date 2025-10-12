using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tool : MonoBehaviour
{
    public GameObject preCube;
    public GameObject preCorner;
    public GameObject preBorder;

    GameObject objDrag;

    LayerMask cubeLayer;
    LayerMask gridLayer;

    Vector3 offset;
    Vector3 gridPos;

    bool isShift;

    public TextMeshProUGUI xScale;
    public TextMeshProUGUI yScale;
    public TextMeshProUGUI minusX;
    public TextMeshProUGUI minusY;

    float xScaleCopy;
    float yScaleCopy;

    bool isMinusPosY;
    bool isMinusPosX;

    private void Awake()
    {
        cubeLayer = LayerMask.GetMask("Cube");
        gridLayer = LayerMask.GetMask("Grid");
    }

    private void Update()
    {
        if (objDrag != null)
        {
            ShowScale();

            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            pos.z = 0;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, gridLayer))
            {
                float x = Mathf.Round(hit.point.x + offset.x);
                float y = Mathf.Round(hit.point.y + offset.y);

                gridPos = new Vector3(x - (isMinusPosX ? 0.5f : 0), y - (isMinusPosY ? 0.5f : 0), 0);

                objDrag.transform.position = gridPos;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                objDrag.transform.rotation = Quaternion.Euler(0, 0, objDrag.transform.eulerAngles.z + 90);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                bool isUp = Mathf.Abs(objDrag.transform.up.y) < Mathf.Abs(objDrag.transform.up.x);

                float x = objDrag.transform.localScale.x;
                float y = objDrag.transform.localScale.y;

                float value = isShift ? 6 : 1;

                objDrag.transform.localScale = new Vector3(isUp ? x : x + value, isUp ? y + value : y, 1);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                bool isUp = Mathf.Abs(objDrag.transform.up.y) < Mathf.Abs(objDrag.transform.up.x);

                float x = objDrag.transform.localScale.x;
                float y = objDrag.transform.localScale.y;

                float value = isShift ? 6 : 1;

                objDrag.transform.localScale = new Vector3(isUp ? x : x - value, isUp ? y - value : y, 1);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                bool isUp = Mathf.Abs(objDrag.transform.up.y) > Mathf.Abs(objDrag.transform.up.x);

                float x = objDrag.transform.localScale.x;
                float y = objDrag.transform.localScale.y;

                float value = isShift ? 6 : 1;

                objDrag.transform.localScale = new Vector3(isUp ? x : x + value, isUp ? y + value : y, 1);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                bool isUp = Mathf.Abs(objDrag.transform.up.y) > Mathf.Abs(objDrag.transform.up.x);

                float x = objDrag.transform.localScale.x;
                float y = objDrag.transform.localScale.y;

                float value = isShift ? 6 : 1;

                objDrag.transform.localScale = new Vector3(isUp ? x : x - value, isUp ? y - value : y, 1);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift)) isShift = true;
            if (Input.GetKeyUp(KeyCode.LeftShift)) isShift = false;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                xScaleCopy = objDrag.transform.localScale.x;

                HighLight(true, false);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                yScaleCopy = objDrag.transform.localScale.y;

                HighLight(false, true);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                objDrag.transform.localScale = new Vector3(xScaleCopy, objDrag.transform.localScale.y, 1);

                HighLight(false, false);
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                objDrag.transform.localScale = new Vector3(objDrag.transform.localScale.x, yScaleCopy, 1);

                HighLight(false, false);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                Destroy(objDrag);

                objDrag = null;
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                isMinusPosX = !isMinusPosX;

                minusX.color = isMinusPosX ? Color.red : Color.black;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                isMinusPosY = !isMinusPosY;

                minusY.color = isMinusPosY ? Color.red : Color.black;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            objDrag = null;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, cubeLayer))
            {
                Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                mouse.z = 0;

                offset = hit.collider.transform.position - mouse;

                objDrag = hit.collider.gameObject;
            }
        }
    }

    void ShowScale()
    {
        xScale.text = "X Scale :" + objDrag.transform.localScale.x;
        yScale.text = "Y Scale :" + objDrag.transform.localScale.y;
    }

    void HighLight(bool isHighLightX, bool isHighLightY)
    {
        xScale.color = isHighLightX ? Color.red : Color.black;
        yScale.color = isHighLightY ? Color.red : Color.black;
    }

    public void CreateCube()
    {
        HighLight(false, false);

        isMinusPosX = false;
        isMinusPosY = false;

        offset = Vector3.zero;

        objDrag = Instantiate(preCube, transform);

        objDrag.transform.localScale = new Vector3(35, 15, 1);
    }


    public void CreateCorner()
    {
        HighLight(false, false);

        isMinusPosX = false;
        isMinusPosY = false;

        offset = Vector3.zero;

        objDrag = Instantiate(preCorner, transform);
    }

    public void CreateBorder()
    {
        HighLight(false, false);

        isMinusPosX = false;
        isMinusPosY = false;

        offset = Vector3.zero;

        objDrag = Instantiate(preBorder, transform);

        objDrag.transform.localScale = new Vector3(35, 2, 1);
    }
}
