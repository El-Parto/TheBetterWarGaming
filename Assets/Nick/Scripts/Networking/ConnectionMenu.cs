using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using kcp2k;
using System.Net;

namespace Networking
{
    public class ConnectionMenu : MonoBehaviour
    {
        CustomNetworkManager networkManager;
        KcpTransport transport;
        [SerializeField] Button hostButton;
        [SerializeField] InputField inputField;
        [SerializeField] Button connectButton;
        [Space]
        [SerializeField] DiscoveredGame discoveredGameTemplate;
        Dictionary<IPAddress, DiscoveredGame> discoveredGames = new Dictionary<IPAddress, DiscoveredGame>();


        void Awake()
        {
            //networkManager = CustomNetworkManager.instance;
            networkManager = GetComponent<CustomNetworkManager>();
            transport = Transport.activeTransport as KcpTransport;

            // adds functionality to the buttons
            hostButton.onClick.AddListener(OnClickHost);
            inputField.onEndEdit.AddListener(OnEndEditAddress);
            connectButton.onClick.AddListener(OnClickConnect);

            CustomNetworkDiscovery discovery = networkManager.discovery;
            discovery.onServerFound.AddListener(OnFoundServer);
            discovery.StartDiscovery();
        }

        // when the 'host game' button is pressed
        void OnClickHost() => networkManager.StartHost();

        // when the ip address of a game has finished being inputted
        void OnEndEditAddress(string value) => networkManager.networkAddress = value;

        // when the 'connect' button is pressed
        void OnClickConnect()
        {
            string address = inputField.text;
            if(string.IsNullOrEmpty(address))
            {
                address = "localhost";
            }
            networkManager.networkAddress = address;
            networkManager.StartClient();
        }

        void OnFoundServer(DiscoveryResponse _response)
        {
            // have we received a server that is broadcasting on the network that we haven't already found
            if (!discoveredGames.ContainsKey(_response.EndPoint.Address))
            {            
                DiscoveredGame game = Instantiate(discoveredGameTemplate, discoveredGameTemplate.transform.parent);
                game.gameObject.SetActive(true);
                game.Setup(_response, networkManager, transport);
                discoveredGames.Add(_response.EndPoint.Address, game);
            }
        }
    } 
}
