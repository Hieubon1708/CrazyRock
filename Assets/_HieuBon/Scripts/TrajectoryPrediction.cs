using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.EventSystems;

public class TrajectoryPrediction : MonoBehaviour
{
    public float timeStep = 0.1f;

    bool isDrag;

    public Transform[] points;

    float defaultVelocity = 5;

    Vector3 startMouse;

    LayerMask layerMask;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Wall");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            isDrag = true;

            ActivePoints(true);

            startMouse = PlayerController.instance.MousePosition();
        }

        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            isDrag = false;

            ActivePoints(false);
        }

        if (isDrag)
        {
            Vector3 startPosition = transform.position;

            startPosition.z = 0;

            float initialVelocity = defaultVelocity + Vector2.Distance(PlayerController.instance.MousePosition(), startMouse);

            Vector3 initialVelocityVector = PlayerController.instance.gun.forward * initialVelocity;

            points[0].position = startPosition;

            Vector3 gravity = Physics.gravity;

            int hideIndex = points.Length;

            for (int i = 1; i < points.Length; i++)
            {
                float time = i * timeStep;

                // p(t) = p0 + v0*t + 0.5 * g * t^2
                Vector3 point = startPosition
                              + initialVelocityVector * time
                              + 0.5f * gravity * time * time;

                point.z = 0;

                points[i].position = point;

                RaycastHit hit;

                if (Physics.Linecast(points[i - 1].position, points[i].position, out hit, layerMask)) hideIndex = i;

                points[i].gameObject.SetActive(i < hideIndex);

            }
        }
    }

    void ActivePoints(bool isActive)
    {
        foreach (var point in points)
        {
            point.gameObject.SetActive(isActive);
        }
    }
}
