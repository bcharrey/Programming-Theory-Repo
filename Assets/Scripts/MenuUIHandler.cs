using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField m_nameInputField;
    [SerializeField]
    private TextMeshProUGUI m_nameText;

    public void StartGame()
    {
        if (string.IsNullOrWhiteSpace(m_nameInputField.text))
        {
            // If name field empty, turn name text red
            m_nameText.color = Color.red;
        }
        else
        {
            // Else, saving the name and loading the scene where the game happens
            MainManager.Instance.PlayerName = m_nameInputField.text;
            SceneManager.LoadScene(1);
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}
