using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuMusicController : MonoBehaviour
{
    private static MainMenuMusicController instance = null;
    public static MainMenuMusicController Instance
    {
        get { return instance; }
    }
   
    private void Update()
    {
        if(PlayerPrefs.GetInt("mute") == 1)
        {
            this.gameObject.GetComponent<AudioSource>().mute = true;
        }
        else
        {
            this.gameObject.GetComponent<AudioSource>().mute = false;

        }
        if (SceneManager.GetActiveScene().name == "MainGameScreen")
        {
            Destroy(this.gameObject);
        }
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
