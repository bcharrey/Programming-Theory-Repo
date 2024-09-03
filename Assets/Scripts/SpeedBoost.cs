using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : PowerUp
{
    [SerializeField]
    private float m_speedMultiplier = 1.1f;

    public override void BoostPlayer()
    {
        PlayerController.Instance.MoveSpeed *= m_speedMultiplier;
    }

    public override void UnboostPlayer()
    {
        PlayerController.Instance.MoveSpeed /= m_speedMultiplier;
    }
}
