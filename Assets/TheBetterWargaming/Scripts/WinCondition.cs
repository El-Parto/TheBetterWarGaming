using UnityEngine;
using TMPro;

namespace Networking
{
    public class WinCondition : MonoBehaviour
    {
        public static string winner;
        public int players;

        void Update() => CheckForLastPlayerLeft();

        void CheckForLastPlayerLeft()
        {
            GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
            players = playerObjects.Length;

            if (players == 0)
            {
                foreach (GameObject player in playerObjects)
                {
                    if (player.gameObject.activeInHierarchy)
                    {
                        winner = player.GetComponentInChildren<TextMeshPro>().text;
                        CustomNetworkManager.Instance.ServerChangeScene("map_Results");
                    }
                }
            }
        }
    } 
}
