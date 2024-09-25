using UnityEngine;

// INHERITANCE
public class Punk : Enemy
{
    // POLYMORPHISM
    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // Move the punk towards where he's facing
        // ABSTRACTION
        MoveForward();
    }

    // ABSTRACTION
    private void MoveForward()
    {
        Vector3 move = transform.position + Speed * Time.deltaTime * m_xzDirectionUnitVector;
        Rigidbody.MovePosition(move);
    }

    private void OnTriggerExit(Collider other)
    {
        // When the Punk exits the play area borders, it enables collision with the play area borders
        if (other.CompareTag("PlayAreaBorder"))
            SwitchCollisionLayer("Punk");
    }

    public void SwitchCollisionLayer(string layerName)
    {
        // Get the layer index from the layer name
        int layerIndex = LayerMask.NameToLayer(layerName);

        // Check if the layer exists
        if (layerIndex != -1)
            // Change the layer of the enemy GameObject
            gameObject.layer = layerIndex;
        else
            Debug.LogWarning($"Layer '{layerName}' does not exist!");
    }

    private void OnCollisionStay(Collision collision)
    {
        // On colliding with a play area border, changes movement direction on XZ plane
        if (collision.gameObject.CompareTag("PlayAreaBorder"))
        {
            // ABSTRACTION
            ChangeXZDirection();
        }
    }

    // ABSTRACTION
    private void ChangeXZDirection()
    {
        Vector2 randomPoint = Random.insideUnitCircle.normalized;
        // Convert to a Vector3 on the XZ plane
        m_xzDirectionUnitVector = new Vector3(randomPoint.x, 0f, randomPoint.y);
    }
}
