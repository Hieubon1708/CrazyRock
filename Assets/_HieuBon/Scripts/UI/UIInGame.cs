using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInGame : MonoBehaviour
{
    public static UIInGame instance;

    private void Awake()
    {
        instance = this;
    }

    public void StraightBullet()
    {
        PlayerController.instance.StraightBullet();
    }
    
    public void PhysicBullet()
    {
        PlayerController.instance.PhysicBullet();
    }
}
