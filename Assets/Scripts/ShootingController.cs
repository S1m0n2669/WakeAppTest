using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Отвечает за поиск цели для стрельбы
/// </summary>
[RequireComponent (typeof(PlayerController))]
[RequireComponent (typeof(AvatarController))]
public class ShootingController : SingletonObject <ShootingController>
{
    public const float ANGLE_FOR_SHOOT = 5f; //угол, который задаёт погрешность, когда пушка считается наведённой на цель, а когда нет

    public Weapon CurrentWeapon = null;

    PlayerController playerController = null;
    Enemy targetEnemy = null;

    AvatarController playerAvatar = null;

    void Start()
    {
        playerAvatar = GetComponent<AvatarController>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (playerController.isMoving() || CurrentWeapon == null)
            return;

        if (targetEnemy != null)
        {
            if (!CheckDestination(targetEnemy.transform.position))
            {
                //потеряли цель (вышла за радиус стрельбы либо ушла за стену)
                targetEnemy = null; //сбрасываем цель, она нам более не интересна, нужно искать другую
            }
        }

        if (targetEnemy == null)
        {
            targetEnemy = FindClosesTarget();
            CurrentWeapon.SetTarget(targetEnemy);

            if (targetEnemy == null)
            {
                Debug.Log("No Enemy to get to aim target");
                return;
            }
        }

        Vector3 PositionForRotate = CurrentWeapon.CalculatePredict(targetEnemy.transform, targetEnemy.GetApproxSpeed());

        //поворачиваемся не прямо к цели, а к точке предикта для конкретного вида оружия, чтобы стрелять на упреждение
        playerAvatar.RotateToTarget(PositionForRotate/*CurrentAimTarget.position*/);

        if (CurrentWeapon != null)
        {
            CurrentWeapon.Fire(PositionForRotate);
        }
    }

    /// <summary>
    /// Проверяет на досягаемость цель.
    /// Есть ли между игроком и целью препятствие, достаёт ли оружие
    /// </summary>
    private bool CheckDestination(Vector3 targetPosition)
    {
        var playerPosition = playerController.transform.position;

        RaycastHit hit;
        if (Physics.Raycast(playerPosition, (targetPosition - playerPosition), out hit, CurrentWeapon.Range))
        {
            return hit.transform.tag == Enemy.ENEMY_TAG; 
        }
        else
        {
            return false;
        }
    }

    Enemy FindClosesTarget()
    {
        float minDistance = float.MaxValue;
        Enemy closestEnemy = null;

        Vector3 PlayerPosition = playerController.transform.position;

        foreach (var enemy in Enemy.EnemyList)
        {
            if (enemy == null || enemy.isDead || !CheckDestination(enemy.transform.position))
                continue;

            var tmpDistance = Vector3.Distance(PlayerPosition, enemy.transform.position);
            if (tmpDistance < minDistance)
            {
                minDistance = tmpDistance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
