using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : MonoBehaviour
{
    public float speed = 20f;

    public int maxBounces = 3;

    Vector3 direction;
    int bounceCount;

    TrailRenderer trailRenderer;

    LayerMask wallLayer;

    private void Awake()
    {
        wallLayer = LayerMask.GetMask("Wall");

        trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    public void Shot(Vector3 startPosition, Vector3 dir)
    {
        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(dir);

        direction = transform.forward;

        trailRenderer.Clear();

        bounceCount = 0;

        gameObject.SetActive(true);
    }

    void Update()
    {
        Vector3 nextPosition = transform.position + direction * speed * Time.deltaTime;

        RaycastHit hit;

        float rayLength = Vector3.Distance(transform.position, nextPosition);

        if (Physics.Raycast(transform.position, direction, out hit, rayLength + 0.1f))
        {
            Vector3 reflectionDirection = Vector3.Reflect(direction, hit.normal);

            direction = reflectionDirection.normalized;

            transform.position = hit.point;

            transform.rotation = Quaternion.LookRotation(direction);

            bounceCount++;

            if (bounceCount >= maxBounces)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            transform.position = nextPosition;
        }
    }
}
