using System;
using UnityEngine;

namespace AGL.Asteroids
{
    public class VictoryUILogic : MonoBehaviour
    {
        public void RestartLogic()
        {
            Time.timeScale = 1;
            FindAnyObjectByType<GameState>().RestartGame();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                RestartLogic();
        }
    }
}
