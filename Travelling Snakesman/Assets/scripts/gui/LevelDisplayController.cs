using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplayController : MonoBehaviour {

    [SerializeField]
    private Text levelText;

    // Use this for initialization
    void Start()
    {
        levelText.text = PlayerPrefs.GetString("TspName");
    }
}

