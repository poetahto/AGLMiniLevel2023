﻿using UnityEngine;

namespace AGL.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField]
        private Animator targetAnimator;

        public void SetMovementDirection(Vector2 direction)
        {
            print($"moving {direction}");
        }

        public void SetIsGrounded(bool isGrounded)
        {
            print($"is grounded: {isGrounded}");
        }

        public void PlayDamage()
        {
            targetAnimator.Play("Damage");
        }

        public void PlayDeath()
        {
            targetAnimator.Play("Death");
        }

        public void PlayUse()
        {
            targetAnimator.Play("Use");
        }
    }
}
