using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompleteHandler : MonoBehaviour
{
    [SerializeField]
    private Button nextLevelButton;
    private bool isNextLevelButton = true;
    private string nextLevel;
    void Start()
    {
        nextLevelButton.onClick.AddListener(PerformClick);
        SetUI();
    }
    void SetUI()
    {
        for (int i = 0; i < AntAlgorithmManager.NumOfLevels; i++)
        {
            if (PlayerPrefs.GetString("TspName") == ("Level" + (AntAlgorithmManager.NumOfLevels)))
            {
                isNextLevelButton = false;
                nextLevelButton.GetComponentInChildren<Text>().text = "View highscores";
                break;
            }
            else if (PlayerPrefs.GetString("TspName") == ("Level" + (i + 1)))
            {
                isNextLevelButton = true; ;
                nextLevelButton.GetComponentInChildren<Text>().text = "Play Level " + (i + 2);
                nextLevel = "Level" + (i + 2);
                break;
            }
        }
    }

    void PerformClick()
    {
        if (isNextLevelButton)
        {
            PlayerPrefs.SetString("TspName", nextLevel);
            SceneManager.LoadScene("MainGameScreen", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("HighscoreScreen", LoadSceneMode.Single);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}



