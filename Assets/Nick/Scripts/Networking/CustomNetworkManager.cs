using Mirror;
using System.Collections.Generic;

namespace Networking
{
    public class CustomNetworkManager : NetworkManager
    {
        // a reference to the CustomNetworkManager version of the singleton
        public static CustomNetworkManager instance => singleton as CustomNetworkManager;

        /// <summary> attempts to find a player using the passed ID </summary>
        /// <param name="_id"> the NetID of the player we are trying to find </param>
        public static NetworkPlayer FindPlayer(uint _id)
        {
            instance.players.TryGetValue(_id, out NetworkPlayer player);
            return player;
        }

        public static void AddPlayer(NetworkPlayer _player) => instance.players.Add(_player.netId, _player);
        public static void RemovePlayer(NetworkPlayer _player) => instance.players.Remove(_player.netId);


        public static NetworkPlayer LocalPlayer
        {
            get
            {
                if (localPlayer == null)
                {
                    foreach (NetworkPlayer networkPlayer in instance.players.Values)
                    {
                        if (networkPlayer.isLocalPlayer)
                        {
                            localPlayer = networkPlayer;
                            break;
                        }
                    }
                }
                return localPlayer;
            }
        }

        static NetworkPlayer localPlayer;

        public bool isHost { get; private set; } = false;

        public CustomNetworkDiscovery discovery;

        Dictionary<uint, NetworkPlayer> players = new Dictionary<uint, NetworkPlayer>();

        public override void OnStartHost()
        {
            isHost = true;
            discovery.AdvertiseServer();
        }

        public override void OnStopHost() => isHost = false;
    } 
}
