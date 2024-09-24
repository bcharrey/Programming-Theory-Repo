using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.UI;

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
            m_nameText.color = Color.red;
        }
        else
        {
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
