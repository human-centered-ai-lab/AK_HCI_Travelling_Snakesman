using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttonscript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LoadLevelButton(string levelName)
	{
		SceneManager.LoadScene (levelName, LoadSceneMode.Single);
	}

	public void LoadLevelButtonAndSavePlayerName (string levelName)
	{
		string playerName = GameObject.Find ("PlayerNameInputText").GetComponent<Text> ().text;

		PlayerPrefs.SetString("PlayerName", playerName);

		SceneManager.LoadScene (levelName, LoadSceneMode.Single);
	}
}
