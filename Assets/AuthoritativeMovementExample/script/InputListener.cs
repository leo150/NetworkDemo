using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AuthMovementExample
{
    public class InputListener : InputListenerBehavior
    {
        #region Inspector
        public uint InputListenerPlayerId = uint.MaxValue;
        #endregion

        private Player _player;
        private Camera _targetCamera;

        private void Start() {}

        protected override void NetworkStart()
        {
            base.NetworkStart();

            Debug.Log("InputListener.NetworkStart: " + networkObject.MyPlayerId);

            InputListenerPlayerId = networkObject.Owner.NetworkId;
        }

        private void Update() {}

        private void FixedUpdate()
        {
            if (networkObject == null || networkObject.IsOwner == false) return;

            if (_targetCamera == null)
                _targetCamera = FindObjectOfType<Camera>();

            if (_targetCamera == null) return;

            networkObject.mousePosition = _targetCamera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}