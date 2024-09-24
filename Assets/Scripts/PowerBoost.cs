using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBoost : PowerUp
{
    [SerializeField]
    private float m_scaleBoost = 1.3f;    
    [SerializeField]
    private float m_playerWeaponOffset = 1.05f;

    public override void BoostPlayer()
    {
        PlayerController.Instance.CurrentPowerBoostsTaken++;

        // Increase the scale of the weapon
        PlayerController.Instance.Weapon.transform.localScale *= m_scaleBoost;

        // Adding an offset to the position of the weapon relative to the player,
        // so that the weapon does not go into the player.
        // Player Weapon's RigidBody's Interpolate must be set to none otherwise it causes unwanted offset
        Vector3 playerWeaponOffsetPosition = PlayerController.Instance.Weapon.transform.up * m_playerWeaponOffset;
        PlayerController.Instance.Weapon.transform.position += playerWeaponOffsetPosition;
    }

    public override void UnboostPlayer()
    {
        PlayerController.Instance.CurrentPowerBoostsTaken--;

        // Decrease the scale of the weapon
        PlayerController.Instance.Weapon.transform.localScale /= m_scaleBoost;

        // Reducing the offset between the position of the weapon and the position of the player,
        // so that the weapon goes back to its original position.
        // Player Weapon's RigidBody's Interpolate must be set to none otherwise it causes unwanted offset
        Vector3 playerWeaponOffsetPosition = -PlayerController.Instance.Weapon.transform.up * m_playerWeaponOffset;
        PlayerController.Instance.Weapon.transform.position += playerWeaponOffsetPosition;
    }
}
