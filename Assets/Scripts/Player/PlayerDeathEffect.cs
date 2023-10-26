using System.Collections;
using AGL.Asteroids;
using UnityEngine;

namespace AGL.Player
{
    public class PlayerDeathEffect : MonoBehaviour
    {
        [SerializeField]
        private PlayerAnimator playerAnimator;

        [SerializeField]
        private float deathTime = 3;

        private GameState _gameState;

        private void Start()
        {
            _gameState = FindAnyObjectByType<GameState>();
        }

        public void Play()
        {
            StartCoroutine(DeathEffectCoroutine());
        }

        private IEnumerator DeathEffectCoroutine()
        {
            // todo: you are dead - sfx, vfx
            print("you died!");
            playerAnimator.PlayDeath();
            yield return new WaitForSeconds(deathTime);
            _gameState.RestartGame();
        }
    }
}
