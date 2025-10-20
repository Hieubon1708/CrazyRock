using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Rigidbody>().angularVelocity = Vector3.right * 100;
    }
}
