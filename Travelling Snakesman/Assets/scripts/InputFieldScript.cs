using UnityEngine;
using UnityEngine.UI;

public class InputFieldScript : MonoBehaviour {

	public GameObject buttonToToggle;
	private Button button;
	private InputField inputField;

	// Use this for initialization
	void Start () {
		button = buttonToToggle.GetComponent<Button> ();
		inputField = gameObject.GetComponent<InputField> ();


		if (PlayerPrefs.GetString ("PlayerName", "").Length >= 1)
		{
			inputField.text = PlayerPrefs.GetString ("PlayerName");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (inputField.text.Length >= 1)
		{ 
			button.interactable = true;
		}
		else 
		{
			button.interactable = false;
		}
	}
}
