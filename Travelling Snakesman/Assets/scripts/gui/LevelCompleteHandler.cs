using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompleteHandler : MonoBehaviour
{
    [SerializeField]
    private Button nextLevelButton;
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private AudioSource audioSource;
    private bool isNextLevelButton = true;
    private string nextLevel;
    private int numOfBackgrounds = 3;
    void Start()
    {
        nextLevelButton.onClick.AddListener(PerformClick);
        SetUI();
        SetBackground();
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

    void SetBackground()
    {
        for (int i = 0; i < AntAlgorithmManager.NumOfLevels; i++)
        {
            int backgroundNo = ((i + 1) % numOfBackgrounds) + 1; 
            if (PlayerPrefs.GetString("TspName") == ("Level" + (i+1)))
            {
                Sprite backgroundSprite = Resources.Load("Sprites/background" + backgroundNo, typeof(Sprite)) as Sprite;
                background.GetComponent<SpriteRenderer>().sprite = backgroundSprite;
                audioSource.clip = Resources.Load("Sounds/backgroundMusic" + backgroundNo) as AudioClip;
                audioSource.Play();
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
}



