using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public GameObject ProjectilePrefab;
    public float ProjectileAngle = 30;
    public float ProjectileForce = 8;
    public float ProjectileTime = 5;
    protected override void Fire()
    {
        ProjectileFire();
    }

    void ProjectileFire()
    {
        Camera cam = Camera.main;
        Vector3 direction = cam.transform.forward;
        direction = Quaternion.AngleAxis(-ProjectileAngle,transform.right) * direction;
        direction.Normalize();
        direction *= ProjectileForce;

        GameObject obj = Instantiate(ProjectilePrefab);
        obj.transform.position = FiringPosition.position;
        obj.GetComponent<Boom>().time = ProjectileTime; // Boom 의 Time 쪽에 ProjectileTime 를 적용
        obj.GetComponent<Rigidbody>().AddForce(direction,ForceMode.Impulse);
    }
}
