using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float radius;

    public float leftLimit;
    public float rightLimit;

    public Transform point;
    public Transform spine;

    Vector3 dir;
    Vector3 offset;

    bool isRight;
    bool isDrag;

    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;

        dir = spine.eulerAngles.normalized;

        point.position = transform.position + dir * 10;

        isRight = transform.localScale.x == 1;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrag = true;

            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            mousePos.z = 0;

            offset = point.position - mousePos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
        }

        if (isDrag)
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            mousePos.z = 0;

            isRight = mousePos.x + offset.x > transform.position.x;

            float angle = isRight ? rightLimit : leftLimit;

            transform.localScale = new Vector3(isRight ? 1 : -1, 1, 1);
            transform.rotation = Quaternion.Euler(0, isRight ? 135 : 225, 0);

            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            dir = (mousePos + offset - transform.position).normalized;

            float x = mousePos.x;
            float r = Mathf.Tan(angle * Mathf.Deg2Rad);
            float y = x * r + transform.position.y;

            Debug.DrawRay(transform.position, new Vector3(x, y, 0).normalized * 10, Color.red);

            point.position = new Vector3(mousePos.x, Mathf.Clamp(mousePos.y, y, 99f), 0) + offset;
        }
    }

    private void LateUpdate()
    {
        spine.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(0, isRight ? 65 : -65, 0);
    }
}
