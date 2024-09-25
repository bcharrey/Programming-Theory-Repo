using System.Collections;
using UnityEngine;

// INHERITANCE
public abstract class PowerUp : MonoBehaviour
{
    private float m_rotationSpeed = 60f;

    // ENCAPSULATION
    [SerializeField]
    private float m_duration = 10f;
    protected float Duration { get { return m_duration; } }

    [SerializeField]
    private AudioSource m_pickedUpSound;

    void Update()
    {
        // Rotate the PowerUp game object around its Z-axis
        transform.Rotate(0, 0, m_rotationSpeed * Time.deltaTime);
    }

    // POLYMORPHISM
    public abstract void BoostPlayer();
    // POLYMORPHISM
    public abstract void UnboostPlayer();

    public IEnumerator CountdownRoutine()
    {
        BoostPlayer();
        yield return new WaitForSeconds(Duration);
        UnboostPlayer();
    }

    public void PickedUp()
    {
        m_pickedUpSound.Play();

        // Disable visual and collider
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        // Destroy after the audio finishes playing
        Destroy(gameObject, m_pickedUpSound.clip.length);
    }
}
