using UnityEngine;
using UnityEngine.SceneManagement;

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

        public void LoadVictoryScene()
        {
            // todo: implement
            print("you won!");
        }

        public void RestartGame()
        {
            // todo: transition screen, maybe prompt to press "restart"?
            print("restarting game");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
