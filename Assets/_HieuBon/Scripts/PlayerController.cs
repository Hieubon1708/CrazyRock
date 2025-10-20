using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float radius;

    public float leftLimit;
    public float rightLimit;

    public Transform point;
    public Transform spine;
    public Transform gun;
    public Transform pool;

    Vector3 dir;
    [HideInInspector]
    public Vector3 mousePos;
    Vector3 offset;

    bool isRight;
    bool isDrag;
    bool isInit;

    Camera mainCamera;

    public GameObject line;
    public GameObject trajectoryPrediction;

    Gun gunSc;

    private void Awake()
    {
        instance = this;

        gunSc = GetComponentInChildren<Gun>();

        mainCamera = Camera.main;

        isRight = transform.localScale.x == 1;

        ActiveMode(true, false);
    }

    public Vector3 MousePosition()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            isDrag = true;

            mousePos = MousePosition();

            offset = point.position - mousePos;

            offset.z = 0;
        }

        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            isDrag = false;

            gunSc.Shot();
        }

        if (isDrag)
        {
            isRight = point.position.x > spine.position.x;

            transform.localScale = new Vector3(isRight ? 1 : -1, 1, 1);
            transform.rotation = Quaternion.Euler(0, isRight ? 135 : 225, 0);

            mousePos = MousePosition();

            float angle = isRight ? rightLimit : leftLimit;

            float x = mousePos.x - spine.position.x + offset.x;
            float r = Mathf.Tan(angle * Mathf.Deg2Rad);
            float y = x * r + spine.position.y;

            Debug.DrawRay(spine.position, new Vector3(x, x * r, 0).normalized * 10, Color.red);

            dir = (new Vector3(mousePos.x + offset.x, Mathf.Clamp(mousePos.y + offset.y, y, 99f), 0) - spine.position).normalized;
            dir.z = 0;

            Debug.DrawRay(spine.position, dir * 10, Color.yellow);

            point.position = spine.position + dir * radius;
        }
    }

    private void LateUpdate()
    {
        if (!isInit)
        {
            isInit = true;

            dir = new Vector3(isRight ? 1 : -1, 0.75f, 0);

            point.position = spine.position + dir * radius;
        }

        spine.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(0, isRight ? 65 : -65, isRight ? 24.25f : -24.25f);
        gun.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(7f, 0, 0);
    }

    public void StraightBullet()
    {
        ActiveMode(true, false);

        gunSc.SetType(Gun.BulletType.Straight);
    }

    public void PhysicBullet()
    {
        ActiveMode(false, true);

        gunSc.SetType(Gun.BulletType.Physic);
    }

    void ActiveMode(bool isLineActive, bool isPhysicActive)
    {
        line.SetActive(isLineActive);
        trajectoryPrediction.SetActive(isPhysicActive);
    }
}
