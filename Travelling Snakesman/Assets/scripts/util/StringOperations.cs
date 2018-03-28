using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringOperations
{

    public static int GetTimeFromString(string time)
    {
            string[] timeStrings = time.Split(':');
        Debug.Log("" + timeStrings[0] + "-" +  timeStrings[1] + "-" + timeStrings[2]);
            return (Int32.Parse(timeStrings[0]) * 60000 + Int32.Parse(timeStrings[1]) * 1000 + Int32.Parse(timeStrings[2]) * 10);
    }

    public static string GetStringFromTime(float timeInMillis)
    {
        var minutes = Mathf.Floor(timeInMillis / 60000);
        var seconds = Mathf.Floor((timeInMillis / 1000) % 60);
        var milliseconds = Mathf.Floor((timeInMillis) % 1000);

        float msDisplay = milliseconds / 10;
        return string.Format("{0}:{1}:{2}", minutes.ToString("0"), seconds.ToString("00"), msDisplay.ToString("00"));
    }
}
