using UnityEngine;

// INHERITANCE
public class Dog : Enemy
{
    // POLYMORPHISM
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // Calculate the direction from the dog to the player
        Vector3 direction = PlayerController.Instance.transform.position - transform.position;
        direction.Normalize();

        // Move the dog towards the player
        transform.position += Speed * Time.deltaTime * direction;
    }
}
