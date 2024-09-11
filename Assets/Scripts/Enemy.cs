using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 10f;
    protected float Speed { get { return m_speed; } }

    [SerializeField]
    private GameObject m_DroppedPowerUp;
    [SerializeField]
    private float m_powerUpDropChance = 0.50f;

    protected Rigidbody m_rigidBody { get; private set; }
    protected Vector3 m_xzDirectionUnitVector { get; private set; }
    protected readonly float m_rotationSpeed = 10000f;
    
    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_xzDirectionUnitVector = (PlayerController.Instance.transform.position - transform.position).normalized;
    }
    
    protected virtual void Update()
    {
        // Enemy is always looking at the player
        m_rigidBody.MoveRotation(Quaternion.LookRotation(PlayerController.Instance.transform.position - transform.position));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            GameManager.Instance.AddPoint();

            float randomValue = Random.Range(0f, 1f);

            if (randomValue <= m_powerUpDropChance)
                Instantiate(m_DroppedPowerUp, new Vector3 (transform.position.x, m_DroppedPowerUp.transform.position.y, transform.position.z), 
                    m_DroppedPowerUp.transform.rotation);

            Destroy(gameObject);
        }
    }
}
