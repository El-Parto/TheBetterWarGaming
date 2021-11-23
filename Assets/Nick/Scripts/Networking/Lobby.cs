using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

namespace Networking
{
    public class Lobby : NetworkBehaviour
    {
        [SerializeField] Button startButton, readyButton, leaveButton;
        [SerializeField] Toggle timerToggle;

        // only the host can interact with certain GUI elements
        void Awake()
        {
            startButton.gameObject.SetActive(CustomNetworkManager.Instance.IsHost);
            timerToggle.gameObject.SetActive(CustomNetworkManager.Instance.IsHost);
        }

        public void OnClickStartMatch()
        {
            NetworkPlayer localPlayer = CustomNetworkManager.LocalPlayer;
            localPlayer.StartMatch();
        }

        public void OnClickLeaveMatch()
        {
            NetworkPlayer localPlayer = CustomNetworkManager.LocalPlayer;
            localPlayer.LeaveMatch();
        }

        public void OnClickReady()
        {
            NetworkPlayer localPlayer = CustomNetworkManager.LocalPlayer;
            localPlayer.Ready();
        }

        public void OnClickTimerToggle(bool value)
        {
            if (!MatchManager.isTimerEnabled) MatchManager.isTimerEnabled = value;
            else if (MatchManager.isTimerEnabled) MatchManager.isTimerEnabled = value;
        }
    } 
}
