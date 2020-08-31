using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Родительский класс для всего оружия
/// </summary>
public abstract class Weapon : MonoBehaviour
{
    public GameObject BulletPrefab = null;

    public float BulletSpeed;
    public float Range;
    public int   Damage;
    public float ReloadTime; //сколько времени должно перезаряжаться оружие между выстрелами

    public Transform FirePoint;
    protected Enemy targetEnemy = null;
    protected Vector3 targetPoint;

    protected float timeOnReloading; //сколько времени пушка на данный момент находится на перезарядке

    Vector3 noY = new Vector3(1, 0, 1); //убирать составляющую высоты нужно только если предполагается, что все игроки на одной высоте

    bool isReloaded { get { return ReloadTime - timeOnReloading <= 0; } }
    bool isRotated
    {
        get
        {
            if (targetEnemy == null)
                return false;

            Vector3 Dir = Vector3.Scale((targetPoint - FirePoint.position), noY);
            return Vector3.Angle(FirePoint.forward, Dir) < ShootingController.ANGLE_FOR_SHOOT;
        }
    }

    //пушка повёрнута к цели и перезаряжена
    bool isReadyToShoot { get { return isReloaded && isRotated; } }

    protected virtual void Update()
    {
        if (timeOnReloading < ReloadTime)
        {
            timeOnReloading += Time.deltaTime;
        }
    }

    public void SetTarget(Enemy target)
    {
        targetEnemy = target;
    }

    public virtual void Fire(Vector3 targetPoint)
    {
        this.targetPoint = targetPoint;
        if (isReadyToShoot)
        {
            timeOnReloading = 0f;
            Shoot();
        }
    }

    public virtual void Shoot() { }

    public virtual Vector3 CalculatePredict(Transform target, float bulletSpeed, Vector3 LuncherPosition, float TargetSpeed)
    {
        return Vector3.zero;
    }

    public Vector3 CalculatePredict(Transform target, float TargetSpeed)
    {
       return CalculatePredict(target, BulletSpeed, FirePoint.position, TargetSpeed);
    }
}

