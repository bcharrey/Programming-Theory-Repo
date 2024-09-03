using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    
    public float moveSpeed = 25f;

    [SerializeField]
    private float attackRotationSpeed = 2000f;
    [SerializeField]
    private float attackCycleDuration = 5f;
    [SerializeField]
    private GameObject m_weapon;
    public GameObject Weapon { get { return m_weapon; } }

    private Rigidbody rb;

    private readonly float rotationSpeed = 50f;

    private void Awake()
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
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 move = CalculateMovementInput();
        HandleMovement(move);
        HandleAttack(move);
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
        rb.velocity = move * moveSpeed;

        // Clamp the player's position within the screen bounds
        float leftLimitX = Math.Abs(GameManager.Instance.AreaLimitLowerLeft.position.x);
        float clampedPositionX = Mathf.Clamp(rb.position.x, -leftLimitX, leftLimitX);
        float clampedPositionZ = Mathf.Clamp(rb.position.z, GameManager.Instance.AreaLimitLowerLeft.position.z,
            GameManager.Instance.AreaLimitUpperLeft.position.z);
        rb.position = new Vector3(clampedPositionX, rb.position.y, clampedPositionZ);
    }

    private void HandleAttack(Vector3 move)
    {
        // Player attacks by spinning with his weapon for attackCycleDuration / 2
        // Then does not attack until attackCycleDuration / 2
        if (Time.time % attackCycleDuration < attackCycleDuration / 2)
        {
            transform.Rotate(0, attackRotationSpeed * Time.deltaTime, 0);
            m_weapon.SetActive(true);
        }
        else
        {
            m_weapon.SetActive(false);

            // Rotate the player to face the direction of movement
            if (move != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PowerUp>(out PowerUp powerUp))
        {
            StartCoroutine(powerUp.CountdownRoutine());
            Destroy(other.gameObject);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemmy"))
    //    {
    //        Debug.Log("Game Over");
    //        Destroy(gameObject);
    //    }
    //}
}
