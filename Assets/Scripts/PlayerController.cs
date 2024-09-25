using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    // ENCAPSULATION
    [SerializeField]
    private float m_moveSpeed = 25f;
    public float MoveSpeed
    {
        get { return m_moveSpeed; }
        set
        {
            if (value < 0)
            {
                Debug.LogWarning("Attempted to set MoveSpeed to a negative value. Setting to 0 instead.");
                m_moveSpeed = 0;
            }
            else
            {
                m_moveSpeed = value;
            }
        }
    }

    // ENCAPSULATION
    [SerializeField]
    private float m_attackRotationSpeed = 800f;
    public float AttackRotationSpeed
    {
        get { return m_attackRotationSpeed; }
        set
        {
            if (value < 0)
            {
                Debug.LogWarning("Attempted to set AttackRotationSpeed to a negative value. Setting to 0 instead.");
                m_attackRotationSpeed = 0;
            }
            else
            {
                m_attackRotationSpeed = value;
            }
        }
    }
    // ENCAPSULATION
    private int m_currentPowerBoostsTaken = 0;
    public int CurrentPowerBoostsTaken
    {
        get { return m_currentPowerBoostsTaken; }
        set
        {
            if (value < 0)
            {
                Debug.LogWarning("Attempted to set CurrentPowerBoostsTaken to a negative value. Setting to 0 instead.");
                m_currentPowerBoostsTaken = 0;
            }
            else
            {
                m_currentPowerBoostsTaken = value;
            }
        }
    }

    // ENCAPSULATION
    private int m_currentSpeedBoostsTaken = 0;
    public int CurrentSpeedBoostsTaken
    {
        get { return m_currentSpeedBoostsTaken; }
        set
        {
            if (value < 0)
            {
                Debug.LogWarning("Attempted to set CurrentSpeedBoostsTaken to a negative value. Setting to 0 instead.");
                m_currentSpeedBoostsTaken = 0;
            }
            else
            {
                m_currentSpeedBoostsTaken = value;
            }
        }
    }

    [SerializeField]
    private float m_attackCycleDuration = 5f;
    [SerializeField]
    private float m_lookRotationSpeed = 50f;
    // ENCAPSULATION
    [SerializeField]
    private GameObject m_weapon;
    public GameObject Weapon { get { return m_weapon; } }
    [SerializeField]
    private AudioSource m_hitSound;
    [SerializeField]
    private int m_maxPowerBoostsTaken = 4;
    [SerializeField]
    private int m_maxSpeedBoostsTaken = 7;

    private Rigidbody m_rigidbody;
    private Vector3 move = Vector3.zero;

    void Awake()
    {
        // Instanciating the Instance
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        // Getting the Rigidbody component
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.GameIsOver)
        {
            // ABSTRACTION
            HandleMovement(move);
            // ABSTRACTION
            HandleAttack(move);
        }
    }

    private void Update()
    {
        // ABSTRACTION
        move = CalculateMovementInput();
    }

    // ABSTRACTION
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

    // ABSTRACTION
    private void HandleMovement(Vector3 move)
    {
        // Apply movement using Rigidbody
        m_rigidbody.velocity = move * m_moveSpeed;
    }

    // ABSTRACTION
    private void HandleAttack(Vector3 move)
    {
        // Player attacks by spinning with his weapon for attackCycleDuration / 2
        // Then does not attack until attackCycleDuration / 2
        if (Time.time % m_attackCycleDuration < m_attackCycleDuration / 2)
        {
            // ABSTRACTION
            CycloneAttack();
        }
        else
        {
            // ABSTRACTION
            StopAttack();
            // Rotate the player to face the direction of movement
            // ABSTRACTION
            FaceMovementDirection();
        }
    }

    // ABSTRACTION
    private void CycloneAttack()
    {
        if (!m_weapon.activeSelf)
            m_weapon.SetActive(true);
        Quaternion newRotation = Quaternion.Euler(0, m_attackRotationSpeed * Time.deltaTime, 0) * m_rigidbody.rotation;
        m_rigidbody.MoveRotation(newRotation);
    }

    // ABSTRACTION
    private void StopAttack()
    {
        m_weapon.SetActive(false);
    }

    // ABSTRACTION
    private void FaceMovementDirection()
    {
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            Quaternion smoothedRotation = Quaternion.Slerp(m_rigidbody.rotation, targetRotation, Time.deltaTime * m_lookRotationSpeed);
            m_rigidbody.MoveRotation(smoothedRotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PowerUp powerUp))
        {
            // ABSTRACTION
            PickUpPowerUp(powerUp);
        }
    }

    // ABSTRACTION
    private void PickUpPowerUp(PowerUp powerUp)
    {
        // ABSTRACTION
        if (DoesPowerUpGrantEffect(powerUp))
            StartCoroutine(powerUp.CountdownRoutine());

        // PowerUp picked up either way, but does not grant effect if max is reached
        powerUp.PickedUp();
    }

    // ABSTRACTION
    private bool DoesPowerUpGrantEffect(PowerUp powerUp)
    {
        if (powerUp is PowerBoost && m_currentPowerBoostsTaken <= m_maxPowerBoostsTaken)
            return true;

        if (powerUp is SpeedBoost && m_currentSpeedBoostsTaken <= m_maxSpeedBoostsTaken)
            return true;

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Checking if hit by an enemy
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            // ABSTRACTION
            HitByEnnemy(collision);
        }
    }

    // ABSTRACTION
    private void HitByEnnemy(Collision collision)
    {
        m_hitSound.Play();
        m_weapon.SetActive(false);
        GameManager.Instance.GameOver();

        // Rotating the player 90° around X to appear dead
        transform.Rotate(90, 0, 0);
        // Deactivating physics and collisions on the player
        m_rigidbody.isKinematic = true;
        m_rigidbody.detectCollisions = false;

        // Destroying the enemy gameobject
        Destroy(collision.gameObject);
    }
}
