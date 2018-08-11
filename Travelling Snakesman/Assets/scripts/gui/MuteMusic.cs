using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteMusic : MonoBehaviour
{
    public Toggle musicToggle;

    void Start()
    {
        if(PlayerPrefs.GetInt("mute") == 1)
        {
            musicToggle.isOn = false;
        }
        else
        {
            musicToggle.isOn = true;
        }

        musicToggle.onValueChanged.AddListener(delegate
        {
            HandleMusic(musicToggle);
        });
    }

    void HandleMusic(Toggle change)
    {
        if (!musicToggle.isOn)
            PlayerPrefs.SetInt("mute", 1);
        else
            PlayerPrefs.SetInt("mute", 0);
    }
}
