using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punk : Enemy
{
    void Update()
    {
        // Create a movement vector
        Vector3 move = transform.position + XzDirectionUnitVector * Speed * Time.fixedDeltaTime;

        Rb.MovePosition(move);

        // Is always facing the player
        Vector3 directionToPlayer = PlayerController.Instance.transform.position - transform.position;
        directionToPlayer.y = 0;  // Keep rotation in the XZ plane

        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * RotationSpeed);
    }
}
