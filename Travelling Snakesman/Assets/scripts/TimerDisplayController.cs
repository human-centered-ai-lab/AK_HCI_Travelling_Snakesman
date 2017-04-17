using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerDisplayController : MonoBehaviour { 
    [SerializeField] private Text timerLabel;
    private float time;
	// Update is called once per frame
	void Update () {
	    time += Time.deltaTime;

	    var minutes = time / 60;
	    var seconds = time % 60;
	    var fraction = (time * 1000) % 1000;

	    timerLabel.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
	}
}
