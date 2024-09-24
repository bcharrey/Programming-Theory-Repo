using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : PowerUp
{
    [SerializeField]
    private float m_speedMultiplier = 1.1f;
    [SerializeField]
    private float m_attackRotationSpeedMultiplier = 1.2f;

    public override void BoostPlayer()
    {
        PlayerController.Instance.MoveSpeed *= m_speedMultiplier;
        PlayerController.Instance.AttackRotationSpeed *= m_attackRotationSpeedMultiplier;
    }

    public override void UnboostPlayer()
    {
        PlayerController.Instance.MoveSpeed /= m_speedMultiplier;
        PlayerController.Instance.AttackRotationSpeed /= m_attackRotationSpeedMultiplier;
    }
}
