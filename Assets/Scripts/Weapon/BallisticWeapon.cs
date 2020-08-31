using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticWeapon : Weapon
{
    public const float G = 9.8f * 3f; //усиленная гравитация для более отчётливой дуги траектории

    public override void Shoot()
    {
        Vector3 TargetPoint = CalculatePredict(targetEnemy.transform, BulletSpeed, FirePoint.position, targetEnemy.GetApproxSpeed());

        Debug.DrawLine(FirePoint.position, TargetPoint, Color.green, 1f);

        float startAngle = CalculateAngleY(TargetPoint);
        //Debug.Log("startAngle = " + startAngle);
        GameObject bullet = Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation * Quaternion.AngleAxis(-startAngle, Vector3.right));
        bullet.GetComponent<BulletScript>().Init(Damage, startAngle, BulletSpeed);
    }

    /// <summary>
    /// Поправка под каким углом к горизонту надо выпускать снаряд, чтобы 
    /// двигаясь по баллистической кривой, он достиг цели
    /// </summary>
    /// <returns></returns>
    float CalculateAngleY(Vector3 targetPoint)
    {
        var distance = Vector3.Distance(FirePoint.position, targetPoint);
        var hdif = FirePoint.position.y - targetPoint.y; 
        var angB = Math.Asin(hdif / distance) * Mathf.Rad2Deg; // дополнительный угол для компенсации разницы высот
        var sinAB = distance * G / (2 * BulletSpeed * BulletSpeed);
        var asa = Math.Asin(sinAB) * Mathf.Rad2Deg;
        float angle = (float)(asa - angB);

        return angle;
    }

    /// <summary>
    /// Расчёт точки предсказания представляет собой решение систему из двух уравнений
    /// </summary>
    public override Vector3 CalculatePredict(Transform target, float bulletSpeed, Vector3 LuncherPosition, float TargetSpeed)
    {
        Vector3 Dist = target.position - LuncherPosition; //вектор до цели

        float x1 = (-TargetSpeed * target.forward).x; //-Vt.x, начальная точки вектора Dist
        float R = bulletSpeed;
        float K = Mathf.Tan(Mathf.Deg2Rad * Vector3.Angle(target.forward, Dist));//коэфициент K прямой = вектору Dist
        float B = -TargetSpeed * K;

        float K2 = K * K;
        float R2 = R * R;

        float sqrt = Mathf.Sqrt(R2 + K2 * R2 - B * B);

        float X1 = -(K * B + sqrt) / (K2 + 1);
        float X2 = -(K * B - sqrt) / (K2 + 1);

        float Y1 = K * X1 + B;
        float Y2 = K * X2 + B;

        Vector3 Vt = target.forward * TargetSpeed;

        Vector3 Vb1 = new Vector3(X1, 0f, Y1); //вектор скорости снаряда
        Vector3 Vb2 = new Vector3(X2, 0f, Y2); //вектор скорости снаряда (два корня)

        Vector3 Dist_na_t1 = Vb1 + Vt;
        Vector3 Dist_na_t2 = Vb2 + Vt;

        float Dist_na_t_mag = Dist_na_t1.magnitude > Dist_na_t2.magnitude ? Dist_na_t1.magnitude : Dist_na_t2.magnitude;
        float T = Dist.magnitude / Dist_na_t_mag; //T - время до столкновения должно быть наименьшим

        Vector3 resultPoint = target.position + target.forward * TargetSpeed * T;

        return resultPoint;
    }
}
