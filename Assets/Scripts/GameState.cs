using UnityEngine;

namespace DefaultNamespace
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