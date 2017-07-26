using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttonscript : MonoBehaviour
{
    public void LoadLevelButton(string levelName)
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    public void LoadLevelButtonAndSavePlayerPrefs(string levelName)
    {
        string playerName = GameObject.Find("PlayerNameInputText").GetComponent<Text>().text;

        PlayerPrefs.DeleteKey(playerName);
        PlayerPrefs.SetString("PlayerName", playerName);

        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}
