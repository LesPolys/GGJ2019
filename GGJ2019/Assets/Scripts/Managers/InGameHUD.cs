using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameHUD : MonoBehaviour
{
    [SerializeField]
    private Text TimerText = null;

    public void SetTimerText(double remainingTime, Vector4 colour)
    {
        TimerText.color = colour;

        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(remainingTime);
        TimerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
    }
}
