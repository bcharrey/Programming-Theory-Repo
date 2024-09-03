using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    private float rotationSpeed = 60f;

    [SerializeField]
    private float m_duration = 10f;
    protected float Duration { get { return m_duration; } }

    void Update()
    {
        // Rotate the PowerUp game object around its Z-axis
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    public abstract void BoostPlayer();
    public abstract void UnboostPlayer();

    public IEnumerator CountdownRoutine()
    {
        BoostPlayer();
        yield return new WaitForSeconds(Duration);
        UnboostPlayer();
    }
}
