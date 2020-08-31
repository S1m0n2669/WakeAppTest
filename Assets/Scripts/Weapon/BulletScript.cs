using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Скрипт снаряда
/// </summary>
public class BulletScript : MonoBehaviour
{
    [SerializeField] protected float leftLiveTime;
    int damage;

    protected bool inited;
    protected float startAngle;
    protected bool isActive;
    protected float speed;

    public void Init(int damage, float startAngle, float speed)
    {
        this.damage = damage;
        this.startAngle = startAngle;
        this.speed = speed;
        isActive = true;
        inited = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == Enemy.ENEMY_TAG)
        {
            collision.transform.GetComponent<Enemy>().TakeHit(damage);
        }

        Despawn();
    }

    protected void Despawn()
    {
        isActive = false;
        Destroy(gameObject);
    }
}
