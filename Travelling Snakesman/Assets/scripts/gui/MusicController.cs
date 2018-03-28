using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    private static MusicController instance = null;
    public static MusicController Instance
    {
        get { return instance; }
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "MainGameScreen")
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
