using UnityEngine;

namespace AGL.Asteroids
{
    public class GameState : MonoBehaviour
    {
        [SerializeField]
        private GameObject player;

        public bool TryGetPlayer(out GameObject playerInstance)
        {
            playerInstance = player;
            return player != null;
        }
    }
}