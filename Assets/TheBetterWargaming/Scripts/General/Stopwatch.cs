using UnityEngine;
using TMPro;

public class Stopwatch : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] float timeValue = 120;

    void Update()
    {
        Timer();
        DisplayTime(timeValue);
    }

    void Timer()
    {
        if (timeValue > 0) timeValue -= Time.deltaTime;
        else timeValue = 0;
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0) timeToDisplay = 0;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{00}:{1:00}", minutes, seconds);
    }

   
}
