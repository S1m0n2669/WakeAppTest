using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Осуществляет движение аватаром (персонажа или бота, разницы нет)
/// посредством передачи силы в Rigidbody
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class AvatarController : MonoBehaviour
{
    [SerializeField] float MoveSpeed = 10f;
    [SerializeField] float RotationSpeed = 10f;

    Rigidbody rigidbody;
    Vector3 plane2DVector = new Vector3(1, 0, 1);

    Quaternion needRotation;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, needRotation, Time.deltaTime * RotationSpeed);
    }

    public void Move(Vector3 move)
    {
        if (move.magnitude > 1f)
            move.Normalize();

        rigidbody.velocity = move * MoveSpeed;
    }

    public void RotateToTarget(Vector3 target)
    {
        Vector3 RotationVector = Vector3.Scale(target - transform.position, plane2DVector);
        needRotation = Quaternion.LookRotation(RotationVector);
        //transform.rotation = Quaternion.LookRotation(RotationVector);
    }

    public void RotateTo(Vector3 dir)
    {
        //transform.rotation = Quaternion.LookRotation(dir);
        needRotation = Quaternion.LookRotation(dir);
    }
}
