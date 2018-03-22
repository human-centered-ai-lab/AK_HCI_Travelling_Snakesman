using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttonscript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void LoadLevelButton(string levelName)
    {
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       
    }

    public void LoadLevelButtonAndSavePlayerPrefs(string levelName)
    {
        string playerName = GameObject.Find("PlayerNameInputText").GetComponent<Text>().text;
        Dropdown tspDropdown = GameObject.Find("TspDropdown").GetComponent<Dropdown>();
        int menuIndex = tspDropdown.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> menuOptions = tspDropdown.GetComponent<Dropdown>().options;
        string tspName = menuOptions[menuIndex].text;
        PlayerPrefs.SetString("TspName", tspName);
        PlayerPrefs.DeleteKey(playerName);
        PlayerPrefs.SetString("PlayerName", playerName);
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowInfo()
    {
        Text infoTextField = GameObject.Find("InfoText").GetComponent<Text>();
        if(infoTextField.enabled)
            infoTextField.enabled = false;
        else
            infoTextField.enabled = true;

    }

    public void SaveTspName()
    {
        Dropdown tspDropdown = GameObject.Find("TspDropdown").GetComponent<Dropdown>();
        int menuIndex = tspDropdown.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> menuOptions = tspDropdown.GetComponent<Dropdown>().options;
        string tspName = menuOptions[menuIndex].text;
        PlayerPrefs.SetString("TspName", tspName);
    }

}
