﻿using BeardedManStudios.Forge.Networking.Generated;
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
        private Dictionary<uint, PlayerBehavior> _playerObjects = new Dictionary<uint, PlayerBehavior>();
        public Dictionary<uint, PlayerBehavior> GetPlayers() { return _playerObjects; }

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

            Debug.Log("NetworkStart");

            if (NetworkManager.Instance.IsServer)
            {
                NetworkManager.Instance.Networker.playerConnected += (player, sender) =>
                {
                    // Instantiate the player on the main Unity thread, get the Id of its owner and add it to a list of players
                    MainThreadManager.Run(() =>
                    {
                        uint networkId = player.NetworkId;

                        Debug.Log("playerConnected; networkId: " + networkId);

                        if (_playerObjects.ContainsKey(networkId)) {
                            Debug.LogWarning("Already contains player with networkId: " + networkId);
                            return;
                        }

                        PlayerBehavior p = NetworkManager.Instance.InstantiatePlayer();
                        p.networkObject.ownerNetId = networkId;
                        _playerObjects.Add(player.NetworkId, p);
                    });
                };

                NetworkManager.Instance.Networker.playerDisconnected += (player, sender) =>
                {
                    uint networkId = player.NetworkId;

                    Debug.Log("playerDisconnected; networkId: " + networkId);

                    if (!_playerObjects.ContainsKey(networkId))
                    {
                        Debug.LogWarning("Doesn't contains player with networkId: " + networkId);
                        return;
                    }

                    // Remove the player from the list of players and destroy it
                    PlayerBehavior p = _playerObjects[networkId];
                    _playerObjects.Remove(networkId);
                    p.networkObject.Destroy();
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