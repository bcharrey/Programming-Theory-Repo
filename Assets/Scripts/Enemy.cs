using UnityEngine;

// INHERITANCE
public abstract class Enemy : MonoBehaviour
{
    // ENCAPSULATION
    [SerializeField]
    private float m_speed = 10f;
    protected float Speed { get { return m_speed; } }

    [SerializeField]
    private GameObject m_DroppedPowerUp;
    [SerializeField]
    private float m_powerUpDropChance = 0.50f;
    [SerializeField]
    private AudioSource m_hitSound;

    // ENCAPSULATION
    protected Rigidbody Rigidbody { get; private set; }
    protected Vector3 m_xzDirectionUnitVector;
    protected readonly float m_rotationSpeed = 10000f;
    
    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        m_xzDirectionUnitVector = (PlayerController.Instance.transform.position - transform.position).normalized;

        // Enemies spawn faster as difficulty level grows
        m_speed += GameManager.Instance.EnemySpeedBonusWithDifficulty * GameManager.Instance.DifficultyLevel;
    }

    // POLYMORPHISM
    protected virtual void FixedUpdate()
    {
        // Enemy is always looking at the player
        Rigidbody.MoveRotation(Quaternion.LookRotation(PlayerController.Instance.transform.position - transform.position));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            // ABSTRACTION
            HitByWeapon();
        }
    }

    // ABSTRACTION
    private void HitByWeapon()
    {
        GameManager.Instance.AddPoint();
        // ABSTRACTION
        ChanceToDropAPowerUp();
        // ABSTRACTION
        Die();
    }

    // ABSTRACTION
    private void ChanceToDropAPowerUp()
    {
        float randomValue = Random.Range(0f, 1f);
        if (randomValue <= m_powerUpDropChance)
            Instantiate(m_DroppedPowerUp, new Vector3(transform.position.x, m_DroppedPowerUp.transform.position.y, transform.position.z),
                m_DroppedPowerUp.transform.rotation);
    }

    // ABSTRACTION
    private void Die()
    {
        m_hitSound.Play();

        // Disable visual and collider
        GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        // Destroy after the audio finishes playing
        Destroy(gameObject, m_hitSound.clip.length);
    }
}
