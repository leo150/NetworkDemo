using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using System.Collections.Generic;
using UnityEngine;

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
                NetworkManager.Instance.Networker.playerConnected += (player, sender) =>
                {
                    // Instantiate the player on the main Unity thread, get the Id of its owner and add it to a list of players
                    MainThreadManager.Run(() =>
                    {
                        uint networkId = player.NetworkId;

                        if (_playerIds.Contains(networkId)) {
                            Debug.LogWarning("Already contains player with networkId: " + networkId);
                        }
                        else {
                            Debug.Log("playerConnected; networkId: " + networkId);

                            _playerIds.Add(networkId);

                            List<PlayerBehavior> objects = new List<PlayerBehavior>();

                            PlayerBehavior p1 = NetworkManager.Instance.InstantiatePlayer();
                            p1.networkObject.ownerNetId = networkId;
                            objects.Add(p1);

                            PlayerBehavior p2 = NetworkManager.Instance.InstantiatePlayer();
                            p2.networkObject.ownerNetId = networkId;
                            objects.Add(p2);

                            PlayerBehavior p3 = NetworkManager.Instance.InstantiatePlayer();
                            p3.networkObject.ownerNetId = networkId;
                            objects.Add(p3);

                            _playerObjects.Add(networkId, objects);
                        }
                    });
                };

                NetworkManager.Instance.Networker.playerDisconnected += (player, sender) =>
                {
                    uint networkId = player.NetworkId;

                    if (_playerIds.Contains(networkId))
                    {
                        Debug.Log("playerDisconnected; networkId: " + networkId);
                       
                        _playerIds.Remove(networkId);

                        List<PlayerBehavior> objects = _playerObjects[networkId];

                        foreach (PlayerBehavior p in objects)
                        {
                            p.networkObject.Destroy();
                        }

                        _playerObjects.Remove(networkId);
                    }
                    else {
                        Debug.LogWarning("Doesn't contains player with networkId: " + networkId);
                        return;
                    }
                };
            }
            else
            {
                // This is a local client - it needs to list for input
                NetworkManager.Instance.InstantiateInputListener();
            }
        }
    }
}