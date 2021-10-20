using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Tooltip("Seconds")]
    [SerializeField] float timeLeft;
    [Tooltip("Seconds")]
    [SerializeField] float startTime;
    [SerializeField] TextMeshProUGUI timeText;

    void Start() => timeLeft = startTime;

    void Update() => Countdown();

    void Countdown()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            DisplayTime(timeLeft);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:0}:{1}", minutes, seconds);
    }

}
