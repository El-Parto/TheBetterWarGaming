using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Networking
{
    public class MatchManager : NetworkBehaviour
    {
        CustomNetworkManager networkManager;
        public static MatchManager instance = null;
        [SyncVar(hook = nameof(OnReceivedMatchStarted))] public bool matchStarted = false;
        [SerializeField] string[] maps; // the names of the map scenes
        public static bool isTimerEnabled = true;
        [SyncVar] public bool timerEnabled;

        void Awake()
        {
            if (instance == null) instance = this;
            else if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            timerEnabled = isTimerEnabled;
            networkManager = CustomNetworkManager.Instance;
        }

        void OnReceivedMatchStarted(bool _old, bool _new)
        {  
            if (_new) ChooseRandomScene();
        }

        void ChooseRandomScene()
        {
            //int index = Random.Range(0, maps.Length);
            //networkManager.ServerChangeScene(maps[index]);

            networkManager.ServerChangeScene("map_Game");
        }

        [Server]
        public void StartMatch()
        {
            matchStarted = true;
        }
    } 
}
