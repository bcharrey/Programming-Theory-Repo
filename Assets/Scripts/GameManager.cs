using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentScoreText;
    [SerializeField]
    private TextMeshProUGUI playerNameText;
    [SerializeField]
    private TextMeshProUGUI bestScoreText;
    [SerializeField]
    private GameObject gameOverScreen;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        playerNameText.text = $"{MainManager.Instance.playerName} :";

        MainManager.Instance.LoadBestScore();
        bestScoreText.text = $"Best Score : {MainManager.Instance.bestScorePlayerName} : {MainManager.Instance.bestScore}";
    }

    private void Update()
    {
        if (!m_Started)
        {

        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    //void AddPoint(int point)
    //{
    //    m_Points += point;
    //    ScoreText.text = $"Score : {m_Points}";
    //}

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
