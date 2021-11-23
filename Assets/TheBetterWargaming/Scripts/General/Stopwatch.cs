using UnityEngine;
using TMPro;
using Mirror;

namespace Networking
{
    public class Stopwatch : NetworkBehaviour
    {
        public GameObject watch;
        [SerializeField] TextMeshProUGUI timeText;
        [SerializeField] float timeValue = 120;

        void Start()
        {
            if (MatchManager.instance.timerEnabled) return;
            Invoke("RpcDisableStopwatch", 0.5f);
        }

        [ClientRpc]
        public void RpcDisableStopwatch()
        {
            watch.SetActive(false);
        }

        void Update()
        {         
            if (!MatchManager.instance.timerEnabled) return;

            Timer();
            DisplayTime(timeValue);

            if (timeValue > 0) return;
            LoadResultsScene();
        }

        void LoadResultsScene() => CustomNetworkManager.Instance.ServerChangeScene("Results");

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
}
