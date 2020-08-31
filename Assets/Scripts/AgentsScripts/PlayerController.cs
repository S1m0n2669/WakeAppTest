using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// Скрипт управление персонажем игрока
/// </summary>
[RequireComponent(typeof(AvatarController))]
public class PlayerController : SingletonObject <PlayerController>
{
    AvatarController PlayerAvatar;
    Vector3 MoveVector;

    void Start()
    {
        PlayerAvatar = GetComponent<AvatarController>();
    }

    private void FixedUpdate()
    {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

        MoveVector = v * Vector3.forward + h * Vector3.right;

        PlayerAvatar.Move(MoveVector);

        if (isMoving())
        {
            PlayerAvatar.RotateTo(MoveVector);
        }
    }

    public bool isMoving()
    {
        return MoveVector.magnitude != 0f;
    }
}
