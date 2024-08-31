using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 25f;
    [SerializeField]
    private float attackRotationSpeed = 2000f;
    [SerializeField]
    private float attackCycleDuration = 5f;
    [SerializeField]
    private GameObject weapon;
    //[SerializeField]
    //private int powerUpAttackCycles = 2;
    [SerializeField]
    private Transform areaLimitUpperLeft;
    [SerializeField]
    private Transform areaLimitLowerLeft;

    private Rigidbody rb;
    //private int killCount = 0;
    private float rotationSpeed = 50f;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    void Update()
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

        // Apply movement using Rigidbody
        rb.velocity = move * moveSpeed;

        // Clamp the player's position within the screen bounds
        float leftLimitX = Math.Abs(areaLimitLowerLeft.position.x);
        float clampedPositionX = Mathf.Clamp(rb.position.x, -leftLimitX, leftLimitX);
        float clampedPositionZ = Mathf.Clamp(rb.position.z, areaLimitLowerLeft.position.z, areaLimitUpperLeft.position.z);
        rb.position = new Vector3(clampedPositionX, rb.position.y, clampedPositionZ);

        // Weapon attack
        if (Time.time % attackCycleDuration < attackCycleDuration / 2)
        {
            transform.Rotate(0, attackRotationSpeed * Time.deltaTime, 0);
            weapon.SetActive(true);
        }
        else
        {
            weapon.SetActive(false);

            // Rotate the player to face the direction of movement
            if (move != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }

        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("PowerUp"))
    //    {
    //        Destroy(other.gameObject);
    //        StartCoroutine(PowerupCountdownRoutine());
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemmy"))
    //    {
    //        Debug.Log("Game Over");
    //        Destroy(gameObject);
    //    }
    //}

    //IEnumerator PowerupCountdownRoutine()
    //{
    //    weapon.transform.localScale = weapon.transform.localScale * 2;
    //    yield return new WaitForSeconds(attackCycleDuration * powerUpAttackCycles);
    //    weapon.transform.localScale = weapon.transform.localScale / 2;
    //}
}
