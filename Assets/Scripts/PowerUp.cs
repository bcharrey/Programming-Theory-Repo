using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private float rotationSpeed = 60f;
    
    void Update()
    {
        // Rotate the PowerUp game object around its Z-axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
