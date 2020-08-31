using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public const string ENEMY_TAG = "Enemy";
    public static List<Enemy> EnemyList = new List<Enemy>();

    public int Health;
    public bool isDead { get; private set; }

    Rigidbody rBody;
    //Vector3 lastPosition;
    //float moveSpeed;

    private void Awake()
    {
        EnemyList.Add(this);
    }

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    /*private void Update()
    {
        moveSpeed = (lastPosition - transform.position).magnitude / Time.deltaTime;
        lastPosition = transform.position;
    }*/

    public void TakeHit(int HitDamage)
    {
        Debug.Log("TakeHit");
        Health -= HitDamage;

        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        EnemyList.Remove(this);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Не у всех противников может быть скорость и не у всех противников может быть
    /// RigidBody (например разрушаемая стена или башня), поэтому скорость может быть 0
    /// </summary>
    public float GetApproxSpeed()
    {
        if (rBody != null)
        {
            //По какой-то причине, магнитуда умноженная на три даёт более точное решение, 
            //чем прямой расчёт скорости в апдейте 
            return rBody.velocity.magnitude * 3f;
            //return moveSpeed;
        }
        else
            return 0;
    }
}
