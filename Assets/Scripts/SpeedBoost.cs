using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : PowerUp
{
    [SerializeField]
    private float m_speedMultiplier = 1.1f;
    [SerializeField]
    private float m_attackCycleReduction = 0.9f;

    public override void BoostPlayer()
    {
        PlayerController.Instance.MoveSpeed *= m_speedMultiplier;
        PlayerController.Instance.AttackCycleDuration *= m_attackCycleReduction;
    }

    public override void UnboostPlayer()
    {
        PlayerController.Instance.MoveSpeed /= m_speedMultiplier;
        PlayerController.Instance.AttackCycleDuration /= m_attackCycleReduction;
    }
}
