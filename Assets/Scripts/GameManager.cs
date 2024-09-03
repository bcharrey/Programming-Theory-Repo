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

    [SerializeField]
    private TextMeshProUGUI currentScoreText;
    [SerializeField]
    private TextMeshProUGUI playerNameText;
    [SerializeField]
    private TextMeshProUGUI bestScoreText;
    [SerializeField]
    private GameObject gameOverScreen;

    // Limits of the game area
    [SerializeField]
    private Transform m_areaLimitUpperLeft;
    [SerializeField]
    private Transform m_areaLimitLowerLeft;

    public Transform AreaLimitUpperLeft { get { return m_areaLimitUpperLeft; } }
    public Transform AreaLimitLowerLeft { get { return m_areaLimitLowerLeft; } }

    // Enemy spawn
    [SerializeField]
    private GameObject[] enemyPrefabs;
    [SerializeField]
    private BoxCollider[] enemySpawnAreas;

    // Enemy spawn
    [SerializeField]
    private float enemySpawnDelay = 2f;
    private float enemySpawnTimer = 0f;

    private int m_Points;
    //private bool m_GameOver = false;

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
        playerNameText.text = $"{MainManager.Instance.playerName} :";

        MainManager.Instance.LoadBestScore();
        bestScoreText.text = $"Best Score : {MainManager.Instance.bestScorePlayerName} : {MainManager.Instance.bestScore}";

        // Spawn an Enemy at the start
        enemySpawnTimer = enemySpawnDelay;
    }

    private void Update()
    {
        // Enemy spawn
        enemySpawnTimer += Time.deltaTime;

        if (enemySpawnTimer >= enemySpawnDelay)
        {
            int randomIndex = UnityEngine.Random.Range(0, enemyPrefabs.Length);

            Instantiate(enemyPrefabs[randomIndex], GenerateSpawnPosition(),
                enemyPrefabs[randomIndex].transform.rotation);

            enemySpawnTimer = 0f;
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        // Select a random index to pick one of the four colliders
        int randomIndex = UnityEngine.Random.Range(0, enemySpawnAreas.Length);

        BoxCollider boxCollider = enemySpawnAreas[randomIndex];

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
        m_Points++;
        currentScoreText.text = $"Score : {m_Points}";
    }

    //public void GameOver()
    //{
    //    m_GameOver = true;
    //    GameOverScreen.SetActive(true);

    //    if (m_Points > MainManager.Instance.bestScore)
    //    {
    //        MainManager.Instance.bestScore = m_Points;
    //        MainManager.Instance.SaveBestScore();
    //    }
    //}
}
