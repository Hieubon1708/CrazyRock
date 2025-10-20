using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    BulletType bulletType;

    public GameObject preStraightBullet;
    public GameObject prePhysicBullet;

    StraightBullet[] straightBullets;
    PhysicBullet[] physicsBullets;

    int straightIndex;
    int physicIndex;

    TrajectoryPrediction trajectoryPrediction;
    Line line;

    private void Start()
    {
        trajectoryPrediction = GetComponentInChildren<TrajectoryPrediction>(true);
        line = GetComponentInChildren<Line>(true);

        int length = 5;

        straightBullets = new StraightBullet[length];
        physicsBullets = new PhysicBullet[length];

        for (int i = 0; i < length; i++)
        {
            GameObject b1 = Instantiate(preStraightBullet, PlayerController.instance.pool);
            GameObject b2 = Instantiate(prePhysicBullet, PlayerController.instance.pool);

            straightBullets[i] = b1.GetComponent<StraightBullet>();
            physicsBullets[i] = b2.GetComponent<PhysicBullet>();

            b1.SetActive(false);
            b2.SetActive(false);
        }
    }

    public enum BulletType
    {
        Straight, Physic
    }

    public void SetType(BulletType type)
    {
        this.bulletType = type;
    }

    public void Shot()
    {
        if (bulletType == BulletType.Straight)
        {
            straightBullets[straightIndex].Shot(line.transform.position, line.transform.forward);

            straightIndex++;

            if (straightIndex == straightBullets.Length) straightIndex = 0;
        }
        else if (bulletType == BulletType.Physic)
        {
            physicsBullets[physicIndex].Shot(trajectoryPrediction.transform.position, trajectoryPrediction.transform.forward, trajectoryPrediction.initialVelocity);

            physicIndex++;

            if (physicIndex == physicsBullets.Length) physicIndex = 0;
        }
    }
}
