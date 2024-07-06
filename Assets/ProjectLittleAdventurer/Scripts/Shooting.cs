using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform ShootPoint;

    public GameObject DamageOrb;

    private Character cc;
    private void Awake()
    {
        cc = GetComponent<Character>();
    }
    public void ShootTheDamageOrb()
    {
        Instantiate(DamageOrb,ShootPoint.position,Quaternion.LookRotation(ShootPoint.forward));
    }
    private void Update()
    {
        cc.RotateTarget();
    }

}
