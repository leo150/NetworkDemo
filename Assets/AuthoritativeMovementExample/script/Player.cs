using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using System;
using System.Linq;
using UnityEngine;

namespace AuthMovementExample
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : PlayerBehavior
    {
        #region Inspector
        [Tooltip("The movement speed of the player.")]
        public float speed = 10.0f;
        #endregion

        private float maxDiff = 0.15f;

        private Rigidbody2D _rigidBody;
        private Collider2D _collider2D;
        private ContactFilter2D _noFilter;
        private Collider2D[] _collisions = new Collider2D[20];

        private bool _isLocalOwner = false;

        public bool GetIsLocalOwner() {
            return _isLocalOwner;
        }

        private InputListener _inputListener;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            _noFilter = new ContactFilter2D().NoFilter();
        }

        protected override void NetworkStart()
        {
            base.NetworkStart();

            Debug.Log("Player: NetworkStart");

            CheckIsLocalOwner();
        }

        private void Update() {}

        void FixedUpdate()
        {
            if (networkObject == null) return;

            if (FindInputListener() == false) return;

            #region Netcode Logic
            if (networkObject.IsServer)
            {
                PlayerUpdate(_inputListener); 
            }
            else
            {
                _rigidBody.position = networkObject.position;
            }
            #endregion
        }

        // RPC
        public override void BecomeOwner(RpcArgs args)
        {
            _isLocalOwner = true;
        }

        private void PlayerUpdate(InputListener inputListener)
        {
            Move(inputListener);
            //PhysicsCollisions();

            networkObject.position = _rigidBody.position;
        }

        private void Move(InputListener inputListener)
        {
            bool useLerp = false;
            float time = Time.fixedDeltaTime;

            if (useLerp)
            {
                var newPosition = Vector2.Lerp(_rigidBody.position,
                                           inputListener.networkObject.mousePosition, speed * time);
                var oldPosition = _rigidBody.position;
                var diff = newPosition - oldPosition;

                if (diff.x > maxDiff)
                    newPosition.x = oldPosition.x + maxDiff;
                else if (diff.x < -maxDiff)
                    newPosition.x = oldPosition.x - maxDiff;

                if (diff.y > maxDiff)
                    newPosition.y = oldPosition.y + maxDiff;
                else if (diff.y < -maxDiff)
                    newPosition.y = oldPosition.y - maxDiff;

                _rigidBody.position = newPosition;
            }
            else
            {
                Vector2 position = inputListener.networkObject.mousePosition - _rigidBody.position;
                float signX = Math.Sign(position.x);
                float signY = Math.Sign(position.y);
                Vector2 velocity = new Vector2(signX, signY) * speed * time;
                _rigidBody.velocity = velocity;
                _rigidBody.position += _rigidBody.velocity;
            }
        }

        private void PhysicsCollisions()
        {
            // Collision detection - get a list of colliders the player's collider overlaps with
            int numColliders = Physics2D.OverlapCollider(_collider2D, _noFilter, _collisions);

            // Collision Resolution - for each of these colliders check if that collider and the player overlap
            for (int i = 0; i < numColliders; ++i)
            {
                ColliderDistance2D overlap = _collider2D.Distance(_collisions[i]);

                // If the colliders overlap move the player
                if (overlap.isOverlapped) _rigidBody.position += overlap.normal * overlap.distance;
            }
        }

        //Helpers

        private void CheckIsLocalOwner()
        {
            if (networkObject.IsServer == false) return;

            if (OwnerNetworkId == networkObject.MyPlayerId)
                _isLocalOwner = true;
        }

        // Return true if find input listener or return false
        private bool FindInputListener()
        {
            if (_inputListener == null)
            {
                if (networkObject.IsServer)
                {
                    InputListener[] listeners = FindObjectsOfType<InputListener>();
                    foreach (InputListener listener in listeners)
                    {
                        if (listener.networkObject.Owner.NetworkId == OwnerNetworkId)
                        {
                            _inputListener = listener;
                            return true;
                        }
                    }
                }
                else
                {
                    InputListener[] listeners = FindObjectsOfType<InputListener>();
                    foreach (InputListener listener in listeners)
                    {
                        if (listener.networkObject.IsOwner)
                        {
                            _inputListener = listener;
                            return true;
                        }
                    }
                }

                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
