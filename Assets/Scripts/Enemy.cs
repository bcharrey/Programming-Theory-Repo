using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 5f;
    protected float Speed { get { return m_speed; } }

    [SerializeField]
    private GameObject m_powerUp;
    [SerializeField]
    private Quaternion m_powerUpRotation;
    [SerializeField]
    private float powerUpDropChance = 0.9f;

    protected Rigidbody Rb { get; private set; }
    protected Vector3 XZDirectionUnitVector { get; private set; }
    protected readonly float RotationSpeed = 10000f;
    
    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        XZDirectionUnitVector = (PlayerController.Instance.transform.position - transform.position).normalized;
    }
    
    protected virtual void Update()
    {
        // Enemy is always looking at the player
        transform.LookAt(PlayerController.Instance.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            GameManager.Instance.AddPoint();

            float randomValue = Random.Range(0f, 1f);

            if (randomValue <= powerUpDropChance)
                Instantiate(m_powerUp, new Vector3 (transform.position.x, m_powerUp.transform.position.y, transform.position.z), 
                    m_powerUp.transform.rotation);

            Destroy(gameObject);
        }
    }
}
