using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    public Transform spine;

    float startZ = -80;
    float endZ = 35;

    float totalPercent;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        totalPercent = startZ - endZ;
    }

    private void Update()
    {


    }
    private void LateUpdate()
    {
        float z = Mathf.Clamp(ConvertAngle(spine.eulerAngles.z), startZ, endZ);

        spine.rotation = Quaternion.Euler(spine.eulerAngles.x, spine.eulerAngles.y, z);
    }

    float ConvertAngle(float angle)
    {
        if (angle > 180) return angle - 360;

        return angle;
    }
}
