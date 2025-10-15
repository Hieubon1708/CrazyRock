using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TrajectoryPrediction : MonoBehaviour
{
    LineRenderer lineRenderer;

    public int resolution = 20;

    public float timeStep = 0.1f;

    public float launchForce = 15f;

    Vector3 initialVelocity;

    Vector3 gravity;

    void Awake()
    {
        gravity = Physics.gravity;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = resolution;
    }

    void Update()
    {
        initialVelocity = transform.forward * launchForce;

        DrawTrajectory();
    }

    private void DrawTrajectory()
    {
        Vector3[] points = new Vector3[resolution];

        Vector3 currentPosition = transform.position;
        Vector3 currentVelocity = initialVelocity;

        for (int i = 0; i < resolution; i++)
        {
            points[i] = currentPosition;

            currentVelocity += gravity * timeStep;

            currentPosition += currentVelocity * timeStep;

            // **3. (Tùy chọn) Kiểm tra va chạm để dừng đường kẻ**
            // Nếu bạn muốn đường kẻ dừng lại khi va chạm, bạn có thể dùng Raycast
            /*
            RaycastHit hit;
            if (Physics.Raycast(currentPosition, currentVelocity.normalized, out hit, currentVelocity.magnitude * timeStep))
            {
                points[i] = hit.point;
                // Cắt bớt các điểm còn lại
                lineRenderer.positionCount = i + 1; 
                break; 
            }
            */

            if (i == resolution - 1)
            {
                lineRenderer.positionCount = resolution;
            }
        }

        lineRenderer.SetPositions(points);
    }
}
