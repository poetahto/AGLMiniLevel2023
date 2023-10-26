using UnityEngine;

namespace AGL.Player
{
    public class PlayerSpaceSuit : MonoBehaviour, IDamageable
    {
        public float Integrity { get; set; }
        public float Oxygen { get; set; }

        public void Damage()
        {
            print("damaged!");
        }
    }
}
