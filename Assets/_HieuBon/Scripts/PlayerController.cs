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
    bool isInit;

    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;

        isRight = transform.localScale.x == 1;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrag = true;

            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            offset = point.position - mousePos;

            offset.z = 0;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrag = false;
        }

        if (isDrag)
        {
            isRight = point.position.x > spine.position.x;

            transform.localScale = new Vector3(isRight ? 1 : -1, 1, 1);
            transform.rotation = Quaternion.Euler(0, isRight ? 135 : 225, 0);

            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            float angle = isRight ? rightLimit : leftLimit;

            float x = mousePos.x - spine.position.x + offset.x;
            float r = Mathf.Tan(angle * Mathf.Deg2Rad);
            float y = x * r + spine.position.y;

            Debug.DrawRay(spine.position, new Vector3(x, x * r, 0).normalized * 10, Color.red);

            dir = new Vector3(mousePos.x + offset.x, Mathf.Clamp(mousePos.y + offset.y, y, 99f), 0) - spine.position;
            dir.z = 0;

            Debug.DrawRay(spine.position, dir.normalized * 10, Color.yellow);

            point.position = spine.position + dir.normalized * radius;
        }

    }

    private void LateUpdate()
    {
        if (!isInit)
        {
            isInit = true;

            dir = isRight ? Vector3.right : -Vector3.right;

            point.position = spine.position + dir.normalized * radius;
        }

        spine.rotation = Quaternion.LookRotation(dir.normalized) * Quaternion.Euler(0, isRight ? 65 : -65, 0);
    }
}
