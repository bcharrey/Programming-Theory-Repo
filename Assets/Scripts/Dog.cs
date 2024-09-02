using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : Enemy
{
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        // Calculate the direction from the dog to the player
        Vector3 direction = PlayerController.Instance.transform.position - transform.position;
        direction.Normalize();

        // Move the dog towards the player
        transform.position += Speed * Time.deltaTime * direction;
    }
}
