using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticBullet : BulletScript
{
    void Update()
    {
        if (!inited || !isActive)
            return;

        leftLiveTime -= Time.deltaTime;

        if (leftLiveTime <= 0f)
            Despawn();

        // вычисляем изменение угла
        var eulerAngles = transform.localRotation.eulerAngles;
        double curAngle = eulerAngles.x;
        curAngle += Math.Atan(BallisticWeapon.G * Time.deltaTime / speed) / Math.PI * 180;

        transform.SetPositionAndRotation(
            transform.position + transform.forward * (speed * Time.deltaTime),
            Quaternion.Euler((float)curAngle, eulerAngles.y, eulerAngles.z));
    }
}
