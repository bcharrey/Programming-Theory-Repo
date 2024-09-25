using UnityEngine;

// INHERITANCE
public class SpeedBoost : PowerUp
{
    [SerializeField]
    private float m_speedMultiplier = 1.1f;
    [SerializeField]
    private float m_attackRotationSpeedMultiplier = 1.2f;

    // POLYMORPHISM
    public override void BoostPlayer()
    {
        PlayerController.Instance.CurrentSpeedBoostsTaken++;

        PlayerController.Instance.MoveSpeed *= m_speedMultiplier;
        PlayerController.Instance.AttackRotationSpeed *= m_attackRotationSpeedMultiplier;
    }

    // POLYMORPHISM
    public override void UnboostPlayer()
    {
        PlayerController.Instance.CurrentSpeedBoostsTaken--;

        PlayerController.Instance.MoveSpeed /= m_speedMultiplier;
        PlayerController.Instance.AttackRotationSpeed /= m_attackRotationSpeedMultiplier;
    }
}
