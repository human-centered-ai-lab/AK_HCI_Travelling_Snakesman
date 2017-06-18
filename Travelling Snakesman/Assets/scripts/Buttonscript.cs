using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
		Application.LoadLevel(levelName);
	}

	public void LoadLevelButtonAndSavePlayerName (string levelName)
	{
		string playerName = GameObject.Find ("PlayerNameInputText").GetComponent<Text> ().text;

		Debug.Log ("playerName = " + playerName);
		AntAlgorithmManager.Instance.playerName = playerName;

		Application.LoadLevel(levelName);
	}

}
