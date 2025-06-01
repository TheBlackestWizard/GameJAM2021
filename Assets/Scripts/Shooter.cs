using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform target;

    [SerializeField] private float shootRate;
    [SerializeField] private float projectileMaxMoveSpeed;
    [SerializeField] private float projectileMaxHeight;

    [SerializeField] private AnimationCurve trajectoryAnimationCurve;
    [SerializeField] private AnimationCurve axisCorrection;
    [SerializeField] private AnimationCurve speedCurve;

    [SerializeField] private CharacterController2D controller;


    private float shootTimer;

    private void Update()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0 )
        {
            shootTimer = 0;
        }

        if (shootTimer <= 0 && CharacterController2D.fired)
        {
            CharacterController2D.fired = false;
            shootTimer = shootRate;
            Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
            projectile.InitalizeProjectile(target, projectileMaxMoveSpeed, projectileMaxHeight);
            projectile.InitializeAnimationCurves(trajectoryAnimationCurve, axisCorrection, speedCurve);
        }
    }
}
