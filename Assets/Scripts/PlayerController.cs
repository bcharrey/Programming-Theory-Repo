using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    
    public float MoveSpeed = 25f;
    public float AttackCycleDuration = 5f;

    [SerializeField]
    private float m_attackRotationSpeed = 500f;
    [SerializeField]
    private GameObject m_weapon;
    public GameObject Weapon { get { return m_weapon; } }

    private Rigidbody m_rigidbody;

    private readonly float m_rotationSpeed = 50f;

    private Vector3 move = Vector3.zero;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        // Get the Rigidbody component
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        HandleMovement(move);
        HandleAttack(move);
    }

    private void Update()
    {
        move = CalculateMovementInput();
    }

    private Vector3 CalculateMovementInput()
    {
        // Get input from keyboard
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Create a movement vector relative to the input
        Vector3 move = new Vector3(moveX, 0, moveZ);

        // Normalize the vector to ensure consistent speed in all directions
        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        return move;
    }

    private void HandleMovement(Vector3 move)
    {
        // Apply movement using Rigidbody
        m_rigidbody.velocity = move * MoveSpeed;
    }

    private void HandleAttack(Vector3 move)
    {
        // Player attacks by spinning with his weapon for attackCycleDuration / 2
        // Then does not attack until attackCycleDuration / 2
        if (Time.time % AttackCycleDuration < AttackCycleDuration / 2)
        {
            Quaternion newRotation = Quaternion.Euler(0, m_attackRotationSpeed * Time.deltaTime, 0) * m_rigidbody.rotation;
            m_rigidbody.MoveRotation(newRotation);
            m_weapon.SetActive(true);
        }
        else
        {
            m_weapon.SetActive(false);

            // Rotate the player to face the direction of movement
            if (move != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * m_rotationSpeed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PowerUp powerUp))
        {
            StartCoroutine(powerUp.CountdownRoutine());
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            GameManager.Instance.GameOver();
            gameObject.SetActive(false);
        }
    }
}
