using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punk : Enemy
{
    protected override void Update()
    {
        base.Update();

        // Create a movement vector
        Vector3 move = transform.position + Speed * Time.deltaTime * m_xzDirectionUnitVector;

        m_rigidbody.MovePosition(move);
    }
}
