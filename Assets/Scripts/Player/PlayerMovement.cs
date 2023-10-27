using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace AGL.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private float turnSpeed = 15;

        [SerializeField]
        private InputHandler inputHandler;

        [SerializeField]
        private Rigidbody targetRigidbody;

        [SerializeField]
        private float moveSpeed = 5;

        [SerializeField]
        private float acceleration = 10;

        [SerializeField]
        private float deceleration = 10;

        [SerializeField]
        private PlayerAnimator playerAnimator;

        [SerializeField]
        private Transform meshTransform;

        [SerializeField]
        private Transform cameraTransform;

        private Vector2 _targetDirection;

        private void Start()
        {
            inputHandler.OnMoveEvent += HandleMoveEvent;
        }

        private void FixedUpdate()
        {
            playerAnimator.SetMovementSpeed(targetRigidbody.velocity.magnitude);
            Vector3 targetVelocity = new Vector3(_targetDirection.x, 0, _targetDirection.y) * moveSpeed;
            targetVelocity = cameraTransform.rotation * targetVelocity;
            Vector3 velocity = targetRigidbody.velocity;
            float originalY = velocity.y;
            velocity.y = 0;

            if (_targetDirection != Vector2.zero)
            {
                // accelerating
                velocity = Vector3.MoveTowards(velocity, targetVelocity, acceleration * Time.deltaTime);
                meshTransform.localRotation = Quaternion.Slerp(meshTransform.localRotation, Quaternion.LookRotation(velocity.normalized, Vector3.up), turnSpeed * Time.deltaTime);
            }
            else
            {
                // decelerating
                velocity = Vector3.Lerp(velocity, Vector3.zero, deceleration * Time.deltaTime);
            }

            velocity.y = originalY;
            targetRigidbody.velocity = velocity;
        }

        private void HandleMoveEvent(Vector2 direction)
        {
            _targetDirection = direction;
        }
    }
}
