using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBoost : PowerUp
{
    [SerializeField]
    private float m_scaleBoost = 1.5f;

    public override void BoostPlayer()
    {
        PlayerController.Instance.Weapon.transform.localScale *= m_scaleBoost;
    }

    public override void UnboostPlayer()
    {
        PlayerController.Instance.Weapon.transform.localScale /= m_scaleBoost;
    }
}
