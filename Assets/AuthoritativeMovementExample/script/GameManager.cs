using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
    public abstract partial class PlayerBehavior : NetworkBehavior
    {
        //Server purpose only
        public uint OwnerNetworkId = 0;
    }
}

namespace AuthMovementExample
{
    /*
     * Singleton GameManager class which handles connection and disconnection of clients
     */
    public class GameManager : GameManagerBehavior
    {
        // Singleton instance
        public static GameManager Instance = null;

        // List of players
        private Dictionary<uint, List<PlayerBehavior>> _playerObjects = new Dictionary<uint, List<PlayerBehavior>>();

        //public Dictionary<uint, PlayerBehavior> GetPlayers() { return _playerObjects; }

        private HashSet<uint> _playerIds = new HashSet<uint>();

        private bool _netStarted = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);
            DontDestroyOnLoad(Instance);
        }

        // Force NetworkStart to happen - a work around for NetworkStart not happening
        // for objects instantiated in scene in the latest version of Forge
        private void FixedUpdate()
        {
            if (!_netStarted && networkObject != null)
            {
                _netStarted = true;
                NetworkStart();
            }
        }

        protected override void NetworkStart()
        {
            base.NetworkStart();

            _netStarted = true;

            Debug.Log("GameManager: NetworkStart");

            if (NetworkManager.Instance.IsServer)
            {
                NetworkManager.Instance.Networker.playerAccepted += (player, sender) =>
                {
                    MainThreadManager.Run(() =>
                    {
                        uint networkId = player.NetworkId;

                        if (_playerIds.Contains(networkId)) {
                            Debug.LogWarning("Already contains player with networkId: " + networkId);
                        }
                        else {
                            Debug.Log("playerConnected; networkId: " + networkId);

                            CreatePlayer(networkId);

                            CallBecomeOwner(player);
                        }
                    });
                };

                NetworkManager.Instance.Networker.playerDisconnected += (player, sender) =>
                {
                    uint networkId = player.NetworkId;

                    if (_playerIds.Contains(networkId))
                    {
                        Debug.Log("playerDisconnected; networkId: " + networkId);

                        DestroyPlayer(networkId, sender);
                    }
                };

                CreatePlayer(networkObject.MyPlayerId);

                NetworkManager.Instance.InstantiateInputListener();
            }
            else
            {
                NetworkManager.Instance.InstantiateInputListener();
            }
        }

        private void CreatePlayer(uint networkId)
        {
            _playerIds.Add(networkId);

            List<PlayerBehavior> objects = new List<PlayerBehavior>();

            PlayerBehavior p1 = NetworkManager.Instance.InstantiatePlayer();

            p1.OwnerNetworkId = networkId;

            objects.Add(p1);

            _playerObjects.Add(networkId, objects);
        }

        private void CallBecomeOwner(NetworkingPlayer player)
        {
            List<PlayerBehavior> objects = _playerObjects[player.NetworkId];

            foreach (var p in objects)
            {
                p.networkObject.SendRpc(player, PlayerBehavior.RPC_BECOME_OWNER);
            }
        }

        private void DestroyPlayer(uint networkId, NetWorker sender)
        {
            //Removing server-owned player object
            _playerIds.Remove(networkId);

            List<PlayerBehavior> objects = _playerObjects[networkId];

            foreach (PlayerBehavior p in objects)
            {
                p.networkObject.Destroy();
            }

            //Removing client-owned objects
            for (int i = sender.NetworkObjectList.Count - 1; i >= 0; i--)
            {
                var networkObject = sender.NetworkObjectList[i];
                if (networkObject.Owner.NetworkId == networkId)
                {
                    sender.NetworkObjectList.RemoveAt(i);
                    networkObject.Destroy();
                }
            }

            _playerObjects.Remove(networkId);
        }
    }
}