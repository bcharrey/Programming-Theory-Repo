using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ENCAPSULATION
    public static GameManager Instance { get; private set; }

    // UI
    // ENCAPSULATION
    [SerializeField]
    private GameObject m_gameOverScreen;
    public GameObject GameOverScreen { get { return m_gameOverScreen; } }
    [SerializeField]
    private TextMeshProUGUI m_currentScoreText;
    [SerializeField]
    private TextMeshProUGUI m_playerNameText;
    [SerializeField]
    private TextMeshProUGUI m_bestScoreText;
    [SerializeField]
    private TextMeshProUGUI m_difficultyLevelText;

    // Enemy spawn
    [SerializeField]
    private GameObject[] m_enemyPrefabs;
    [SerializeField]
    private BoxCollider[] m_enemySpawnAreas;
    [SerializeField]
    private float m_enemySpawnDelay = 2f;
    [SerializeField]
    private int m_pointsForNextDifficultyLevel = 5;
    [SerializeField]
    private float m_enemySpawnDelayReductionWithDifficulty = 0.8f;
    // ENCAPSULATION
    [SerializeField]
    private float m_enemySpeedBonusWithDifficulty = 0.5f;
    public float EnemySpeedBonusWithDifficulty { get { return m_enemySpeedBonusWithDifficulty; } }

    private float m_enemySpawnTimer = 0f;

    private int m_points = 0;
    // ENCAPSULATION
    private int m_difficultyLevel = 1;
    public int DifficultyLevel { get { return m_difficultyLevel; } }
    // ENCAPSULATION
    private bool m_gameIsOver = false;
    public bool GameIsOver { get { return m_gameIsOver; } }

    private void Awake()
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
        // !! Uncomment this for testing at fixed framerate
        //Application.targetFrameRate = 20;

        // Setting the player name in UI
        m_playerNameText.text = $"{MainManager.Instance.PlayerName}";

        // ABSTRACTION
        SetBestScoreInUI();

        // Setting the enemy spawn time directly at the enemy spawn delay
        // in order to pawn an enemy at the start and avoid wait
        m_enemySpawnTimer = m_enemySpawnDelay;
    }
    
    private void Update()
    {
        if (!m_gameIsOver)
        {
            // ABSTRACTION
            SpawnEnemyAtDelay();
        }
    }

    // ABSTRACTION
    private void SetBestScoreInUI()
    {
        // Setting the best score in UI
        MainManager.Instance.LoadBestScore();
        m_bestScoreText.text = $"Best Score : {MainManager.Instance.BestScorePlayerName} : {MainManager.Instance.BestScore}";
    }

    // ABSTRACTION
    private void SpawnEnemyAtDelay()
    {
        m_enemySpawnTimer += Time.deltaTime;

        if (m_enemySpawnTimer >= m_enemySpawnDelay)
        {
            // ABSTRACTION
            SpawnEnemy();
            m_enemySpawnTimer = 0f;
        }
    }

    // ABSTRACTION
    private void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, m_enemyPrefabs.Length);
        // ABSTRACTION
        Instantiate(m_enemyPrefabs[randomIndex], GenerateSpawnPosition(),
            m_enemyPrefabs[randomIndex].transform.rotation);
    }

    // ABSTRACTION
    private Vector3 GenerateSpawnPosition()
    {
        // Select a random index to pick one of the four colliders
        int randomIndex = Random.Range(0, m_enemySpawnAreas.Length);

        BoxCollider boxCollider = m_enemySpawnAreas[randomIndex];

        // Get the center and size of the box collider in local space
        Vector3 boxLocalCenter = boxCollider.center;
        Vector3 boxLocalSize = boxCollider.size;

        // Generate a random point within the local XZ plane of the collider
        float spawnPosLocalX = Random.Range(-boxLocalSize.x / 2, boxLocalSize.x / 2);
        float spawnPosLocalZ = Random.Range(-boxLocalSize.z / 2, boxLocalSize.z / 2);

        // Create the local position based on the random values
        Vector3 spawnLocalPosition = new Vector3(spawnPosLocalX, 0, spawnPosLocalZ) + boxLocalCenter;

        // Convert the local position to world space using the collider's transform
        Vector3 spawnWorldPosition = boxCollider.transform.TransformPoint(spawnLocalPosition);
        // Y coordinate is 0 because the enemy has his feet on the ground
        spawnWorldPosition.y = 0;

        return spawnWorldPosition;
    }

    public void AddPoint()
    {
        m_points++;
        m_currentScoreText.text = $"Score : {m_points}";

        // Rising difficulty level with points earned
        if (m_points % m_pointsForNextDifficultyLevel == 0)
        {
            // ABSTRACTION
            IncreaseDifficultyLevel();
        }
    }

    // ABSTRACTION
    private void IncreaseDifficultyLevel()
    {
        m_difficultyLevel++;
        m_difficultyLevelText.text = $"Difficulty Level : {m_difficultyLevel}";
        m_enemySpawnDelay *= m_enemySpawnDelayReductionWithDifficulty;
    }

    public void GameOver()
    {
        m_gameIsOver = true;
        GameOverScreen.SetActive(true);
        // ABSTRACTION
        UpdateBestScore();
    }

    // ABSTRACTION
    private void UpdateBestScore()
    {
        if (m_points > MainManager.Instance.BestScore)
        {
            MainManager.Instance.BestScore = m_points;
            MainManager.Instance.SaveBestScore();
        }
    }
}
