using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicController : MonoBehaviour {

	void Start () {
        if (PlayerPrefs.GetInt("mute") == 1)
        {
            this.gameObject.GetComponent<AudioSource>().mute = true;
        }
        else
        {
            this.gameObject.GetComponent<AudioSource>().mute = false;
        }
    }
}
