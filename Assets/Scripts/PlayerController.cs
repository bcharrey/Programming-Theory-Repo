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
        if (!GameManager.Instance.GameIsOver)
        {
            HandleMovement(move);
            HandleAttack(move);
        }
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
        m_rigidbody.velocity = move * m_moveSpeed;
    }

    private void HandleAttack(Vector3 move)
    {
        // Player attacks by spinning with his weapon for attackCycleDuration / 2
        // Then does not attack until attackCycleDuration / 2
        if (Time.time % m_attackCycleDuration < m_attackCycleDuration / 2)
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
                Quaternion smoothedRotation = Quaternion.Slerp(m_rigidbody.rotation, targetRotation, Time.deltaTime * m_lookRotationSpeed);
                m_rigidbody.MoveRotation(smoothedRotation);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PowerUp powerUp))
        {
            if ((powerUp is PowerBoost && m_currentPowerBoostsTaken <= m_maxPowerBoostsTaken)
                || (powerUp is SpeedBoost && m_currentSpeedBoostsTaken <= m_maxSpeedBoostsTaken))
                StartCoroutine(powerUp.CountdownRoutine());

            // PowerUp taken either way, but does not grant effect if max is reached
            powerUp.PickedUp();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Game Over on collision with Enemy
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            m_hitSound.Play();
            GameManager.Instance.GameOver();
            m_weapon.SetActive(false);

            // Rotating the player 90° to appear dead
            transform.Rotate(90, 0, 0);
            // Deactivating physics and collisions on the player
            m_rigidbody.isKinematic = true;
            m_rigidbody.detectCollisions = false;

            // Destroying the enemy gameobject
            Destroy(collision.gameObject);
        }
    }
}
