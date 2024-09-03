using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBoost : PowerUp
{
    [SerializeField]
    private float scaleBoost = 1.5f;

    public override void BoostPlayer()
    {
        PlayerController.Instance.Weapon.transform.localScale *= scaleBoost;
    }

    public override void UnboostPlayer()
    {
        PlayerController.Instance.Weapon.transform.localScale /= scaleBoost;
    }
}
