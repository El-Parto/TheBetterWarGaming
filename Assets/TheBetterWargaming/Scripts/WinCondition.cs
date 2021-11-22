using UnityEngine;
using TMPro;

namespace Networking
{
    public class WinCondition : MonoBehaviour
    {
        public static string winner;
        public int players;

        void Update()
        {
            Invoke("CheckForLastPlayerLeft", 3);
        }

        void CheckForLastPlayerLeft()
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            players = playerObjects.Length;

            if (players == 1)
            {
                foreach (GameObject player in playerObjects)
                {
                    if (player.gameObject.activeInHierarchy)
                    {
                        winner = player.GetComponentInChildren<TextMeshPro>().text;
                        Invoke("LoadResultsScene", 3);
                    }
                }
            }
        }

        void LoadResultsScene() => CustomNetworkManager.Instance.ServerChangeScene("Results");
    } 
}
