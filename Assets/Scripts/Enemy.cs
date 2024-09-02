using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 5f;
    protected float Speed { get { return m_speed; } }

    protected Rigidbody Rb { get; private set; }
    protected Vector3 XzDirectionUnitVector { get; private set; }
    protected float RotationSpeed { get; private set; } = 10000f;

    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        XzDirectionUnitVector = (PlayerController.Instance.transform.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
