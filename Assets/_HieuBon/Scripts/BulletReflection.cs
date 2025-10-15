using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletReflection : MonoBehaviour
{
    public float speed = 20f;

    public int maxBounces = 3;

    Vector3 direction;
    int bounceCount;

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

            bounceCount++;

            if (bounceCount >= maxBounces)
            {
                Destroy(gameObject);

                return;
            }
        }
        else
        {
            transform.position = nextPosition;
        }
    }
}
