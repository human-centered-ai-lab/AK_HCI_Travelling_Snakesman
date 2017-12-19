using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIResizer : MonoBehaviour {

    public GameObject LeaderboardCanvas;
    public GameObject ControlCanvas;
	// Use this for initialization
	void Start () {
        LeaderboardCanvas.GetComponent<Canvas>().GetComponent<RectTransform>().offsetMax = new Vector2(0, -Screen.height/4);
        LeaderboardCanvas.GetComponent<Canvas>().GetComponent<RectTransform>().offsetMin= new Vector2((Screen.width / 3), 0);

        ControlCanvas.GetComponent<Canvas>().GetComponent<RectTransform>().offsetMax = new Vector2(-(Screen.width / 3) * 2, -Screen.height / 4);
        ControlCanvas.GetComponent<Canvas>().GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);

        // ControlCanvas.GetComponent<Canvas>().transform.width = Screen.Width / 2.0f;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
