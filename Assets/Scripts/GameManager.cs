using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    //[HideInInspector]
    //public bool GameOver = false;

    // UI
    [SerializeField]
    private GameObject m_gameOverScreen;
    public GameObject GameOverScreen { get { return m_gameOverScreen; } }
    [SerializeField]
    private TextMeshProUGUI m_currentScoreText;
    [SerializeField]
    private TextMeshProUGUI m_playerNameText;
    [SerializeField]
    private TextMeshProUGUI m_bestScoreText;

    // Enemy spawn
    [SerializeField]
    private GameObject[] m_enemyPrefabs;
    [SerializeField]
    private BoxCollider[] m_enemySpawnAreas;
    [SerializeField]
    private float m_enemySpawnDelay = 2f;

    private float m_enemySpawnTimer = 0f;

    private int m_points;

    private void Awake()
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
        // !! Uncomment this for testing at fixed framerate
        //Application.targetFrameRate = 20;

        m_playerNameText.text = $"{MainManager.Instance.PlayerName}";

        MainManager.Instance.LoadBestScore();
        m_bestScoreText.text = $"Best Score : {MainManager.Instance.BestScorePlayerName} : {MainManager.Instance.BestScore}";

        // Spawn an Enemy at the start
        m_enemySpawnTimer = m_enemySpawnDelay;
    }

    private void Update()
    {
        // Enemy spawn
        m_enemySpawnTimer += Time.deltaTime;

        if (m_enemySpawnTimer >= m_enemySpawnDelay)
        {
            int randomIndex = UnityEngine.Random.Range(0, m_enemyPrefabs.Length);

            Instantiate(m_enemyPrefabs[randomIndex], GenerateSpawnPosition(),
                m_enemyPrefabs[randomIndex].transform.rotation);

            m_enemySpawnTimer = 0f;
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        // Select a random index to pick one of the four colliders
        int randomIndex = UnityEngine.Random.Range(0, m_enemySpawnAreas.Length);

        BoxCollider boxCollider = m_enemySpawnAreas[randomIndex];

        // Get the center and size of the box collider in local space
        Vector3 boxLocalCenter = boxCollider.center;
        Vector3 boxLocalSize = boxCollider.size;

        // Generate a random point within the local XZ plane of the collider
        float spawnPosLocalX = UnityEngine.Random.Range(-boxLocalSize.x / 2, boxLocalSize.x / 2);
        float spawnPosLocalZ = UnityEngine.Random.Range(-boxLocalSize.z / 2, boxLocalSize.z / 2);

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
    }

    public void GameOver()
    {
        GameOverScreen.SetActive(true);
        if (m_points > MainManager.Instance.BestScore)
        {
            MainManager.Instance.BestScore = m_points;
            MainManager.Instance.SaveBestScore();
        }
    }
}
