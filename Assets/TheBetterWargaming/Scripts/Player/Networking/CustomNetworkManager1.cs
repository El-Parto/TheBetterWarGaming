//using JetBrains.Annotations;

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Mirror;

//public class CustomNetworkManager1 : NetworkManager
//{
//    	/// <summary>
//		/// A refernce to the CustomNetworkManager version of the singleton
//		/// to prevent referencing it all the time.
//		/// </summary>
//		public static CustomNetworkManager1 Instance => singleton as CustomNetworkManager1;

//		/// <summary>
//		///* Attempts to find a player using the passed Net ID, this can return null
//		/// </summary>
//		/// <param name="_id">The NetID of the player that we are trying to find.</param>
		
//		[CanBeNull]
//		public static NetworkPlayer FindPlayer(uint _id) // id comes from the network player.
//		{
//			Instance.players.TryGetValue(_id, out NetworkPlayer player);
//			return player;
//		}
		
		
		
//		// for the dictionary
//		/// <summary>
//		/// Adds a player to the dictionary
//		/// </summary>
//		public static void AddPlayer([NotNull] NetworkPlayer _player) => Instance.players.Add(_player.netId, _player);

//		/// <summary>
//		/// Removes a player from the dictionary name.
//		/// </summary>
//		public static void RemovePlayer([NotNull] NetworkPlayer _player) => Instance.players.Remove(_player.netId);

//		/// <summary>
//		/// A reference to the local player of the game,
//		/// </summary>
//		public static NetworkPlayer LocalPlayer
//		{
//			get
//			{
//				// if the internal localPlayer instance is Null
//				if(localPlayer == null)
//				{
//					// * loop through each player in the game and check if it is a local player
//					foreach(var networkPlayer in Instance.players.Values)
//					{
//						if(networkPlayer.isLocalPlayer)
//						{
//							// * Set local player to this player as it is local player
//							localPlayer = networkPlayer;
//							break;
//						}
//					}
//				} 
//				//return the cached local player
//				return localPlayer;
//			}
			
//		}
//		// the internal reference to the local player,
//		private static NetworkPlayer localPlayer;
		
		
//		/// <summary>
//		/// Whether or not This NetworkManager is the host
//		/// </summary>
//		public bool IsHost
//		{
//			get;
//			private set;
//		} = false;

//		//public CustomNetworkDiscovery discovery; // variable discovery
		

//		/// <summary>
//		/// The dictionary of all connected players using their Net ID as the key.
//		/// </summary>
//		private readonly Dictionary<uint, NetworkPlayer> players = new Dictionary<uint, NetworkPlayer>();


//		/// <summary>
//		/// This is invoked when a host is started.
//		/// <para>StartHost has multiple signatures, but they all cause this hook to be called.</para>
//		/// </summary>
//		public override void OnStartHost()
//		{
//			IsHost = true;
//			//this makes it visible on the network
//			//discovery.AdvertiseServer();
//		}

//		/// <summary>
//		/// This is invoked when a server is started - including when a host is started.
//		/// <para>StartServer has multiple signatures, but they all cause this hook to be called.</para>
//		/// </summary>

		
//		///<summary>
//		/// called when host is stopped
//		/// </summary>
		
//		public override void OnStopHost()
//		{
//			IsHost = false;
//		}

		
		
//	}

