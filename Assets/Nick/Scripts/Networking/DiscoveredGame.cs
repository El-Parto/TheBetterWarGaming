using UnityEngine;
using UnityEngine.UI;
using kcp2k;

namespace Networking
{
    [RequireComponent(typeof(Button))]
    public class DiscoveredGame : MonoBehaviour
    {
        [SerializeField] Text gameInformation;
        CustomNetworkManager networkManager;
        KcpTransport transport;
        DiscoveryResponse response;

        public void Setup(DiscoveryResponse _response, CustomNetworkManager _networkManager, KcpTransport _transport)
        {
            networkManager = _networkManager;
            transport = _transport;
            UpdateResponse(_response);
            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                // set the ip address to the endpoint address
                networkManager.networkAddress = response.EndPoint.Address.ToString();
                // change the port to the correct type and assign the port to it
                transport.Port = response.port;
                // start the client with the address information
                networkManager.StartClient();
            });
        }
       
        public void UpdateResponse(DiscoveryResponse _response)
        {
            response = _response;
            // setup the text to show the ip in bold
            gameInformation.text = $"<b>{response.EndPoint.Address}</b>";
        }

        void Update()
        {
            
        }
    } 
}
