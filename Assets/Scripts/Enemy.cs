using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 5f;
    protected float Speed { get { return m_speed; } }

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
            Destroy(gameObject);
        }
    }
}
