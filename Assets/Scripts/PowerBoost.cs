using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBoost : PowerUp
{
    [SerializeField]
    private float m_scaleBoost = 1.5f;    
    [SerializeField]
    private float m_playerWeaponZOffset = 1.5f;

    public override void BoostPlayer()
    {
        PlayerController.Instance.Weapon.transform.localScale *= m_scaleBoost;
        Vector3 position = PlayerController.Instance.Weapon.transform.localPosition;
        position.z += m_playerWeaponZOffset;
        PlayerController.Instance.Weapon.transform.localPosition = position;
    }

    public override void UnboostPlayer()
    {
        PlayerController.Instance.Weapon.transform.localScale /= m_scaleBoost;
        Vector3 position = PlayerController.Instance.Weapon.transform.localPosition;
        // Bug here ?
        position.z -= m_playerWeaponZOffset;
        PlayerController.Instance.Weapon.transform.localPosition = position;
    }
}
