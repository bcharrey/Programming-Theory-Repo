using UnityEngine;
using System.IO;

public class MainManager : MonoBehaviour
{
    // ENCAPSULATION
    public static MainManager Instance { get; private set; }

    [HideInInspector]
    public string PlayerName;
    [HideInInspector]
    public string BestScorePlayerName;
    [HideInInspector]
    public int BestScore;

    private void Awake()
    {
        // Instantiating the Instance
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // This instance is kept between scenes
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    class SaveData
    {
        public string bestScorePlayerName;
        public int bestScore;
    }

    public void SaveBestScore()
    {
        SaveData data = new SaveData();
        data.bestScorePlayerName = PlayerName;
        data.bestScore = BestScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            BestScorePlayerName = data.bestScorePlayerName;
            BestScore = data.bestScore;
        }
    }
}
