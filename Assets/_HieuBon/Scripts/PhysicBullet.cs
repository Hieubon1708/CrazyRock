using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicBullet : MonoBehaviour
{
    Rigidbody rb;

    TrailRenderer trailRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    public void Shot(Vector3 startPosition, Vector3 dir, float initialVelocity)
    {
        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(dir);

        trailRenderer.Clear();

        rb.angularDrag = 0.05f;
        rb.velocity = transform.forward * initialVelocity;
        rb.angularVelocity = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)).normalized * initialVelocity * 150;

        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.y - PlayerController.instance.transform.position.y) > 100
            || rb.angularVelocity.magnitude < 0.15f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.angularDrag = 3;
    }
}
