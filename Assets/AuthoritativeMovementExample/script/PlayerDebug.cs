using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerDebug : MonoBehaviour {

    private Camera _targetCamera;
    private Vector2 _mousePosition;
    private Rigidbody2D _rigidBody;
    private Collider2D _collider2D;
    private ContactFilter2D _noFilter;
    private Collider2D[] _collisions = new Collider2D[20];

    private float speed = 5.0f;
    private float maxDiff = 0.15f;

	// Use this for initialization
	void Start () 
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
        _noFilter = new ContactFilter2D().NoFilter();
	}
	
	void Update () 
    {
        if (_targetCamera == null)
            _targetCamera = FindObjectOfType<Camera>();

        if (_targetCamera == null) return;

        _mousePosition = _targetCamera.ScreenToWorldPoint(Input.mousePosition);

        PlayerUpdate();
	}

    private void PlayerUpdate()
    {
        Move();
        //PhysicsCollisions();
    }

    private void Move()
    {
        bool useLerp = true;
        float time = Time.fixedDeltaTime;

        if (useLerp)
        {
            var newPosition = Vector2.Lerp(_rigidBody.position,
                                           _mousePosition, speed * time);
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
            _rigidBody.velocity = Vector2.zero;
        }
        else
        {
            Vector2 position = _mousePosition - _rigidBody.position;
            float signX = Math.Sign(position.x);
            float signY = Math.Sign(position.y);
            Vector2 velocity = new Vector2(signX, signY) * speed * time;
            _rigidBody.velocity = velocity;
            _rigidBody.position += _rigidBody.velocity;
        }
    }

    private void PhysicsCollisions()
    {
        int numColliders = Physics2D.OverlapCollider(_collider2D, _noFilter, _collisions);

        for (int i = 0; i < numColliders; ++i)
        {
            ColliderDistance2D overlap = _collider2D.Distance(_collisions[i]);
            if (overlap.isOverlapped) _rigidBody.position += overlap.normal * overlap.distance;
        }
    }
}
