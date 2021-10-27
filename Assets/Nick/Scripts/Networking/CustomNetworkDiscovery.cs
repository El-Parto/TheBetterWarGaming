using UnityEngine;
using Mirror;
using UnityEngine.Events;
using Mirror.Discovery;
using System;
using System.Net;
using kcp2k;

namespace Networking
{
    public class DiscoveryRequest : NetworkMessage
    {

    }

    public class DiscoveryResponse : NetworkMessage
    {
        public IPEndPoint EndPoint { get; set; }
        public Uri uri;
        public ushort port;
        public long serverId;
    }

    [Serializable] public class ServerFoundEvent : UnityEvent<DiscoveryResponse>
    {

    }

    public class CustomNetworkDiscovery : NetworkDiscoveryBase<DiscoveryRequest, DiscoveryResponse>
    {
        public long ServerId { get; private set; }
        public Transport transport;
        public ServerFoundEvent onServerFound = new ServerFoundEvent();

        public override void Start()
        {
            ServerId = RandomLong();

            if (transport == null)
            {
                transport = Transport.activeTransport;
            }

            base.Start();
        }

        protected override DiscoveryResponse ProcessRequest(DiscoveryRequest request, IPEndPoint endpoint)
        {
            try
            {
                // chance to throw error if transport doesn't support network discovery
                return new DiscoveryResponse()
                {
                    serverId = ServerId,
                    uri = transport.ServerUri(),
                    port = ((KcpTransport)transport).Port
                };
            }
            catch (NotImplementedException e)
            {
                Debug.LogException(e, gameObject);
                throw;
            }
        }

        protected override DiscoveryRequest GetRequest() => new DiscoveryRequest();

        protected override void ProcessResponse(DiscoveryResponse response, IPEndPoint endpoint)
        {
            // we receive a message from the remote endpoint
            response.EndPoint = endpoint;

            UriBuilder realUri = new UriBuilder(response.uri)
            {
                Host = response.EndPoint.Address.ToString()
            };

            response.uri = realUri.Uri;

            onServerFound.Invoke(response);

        }
    } 
}
