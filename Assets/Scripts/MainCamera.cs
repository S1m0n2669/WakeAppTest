using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : SingletonObject<MainCamera>
{
    public Transform CameraTarget = null;
    public Vector3 Offset;

    [Space]
    [Header("Shake Params")]

    public float ShakePower = 1f;

    void Update()
    {
        transform.position = CameraTarget.position + Offset;
    }

    public void ShakeCamera()
    {

    }
}
