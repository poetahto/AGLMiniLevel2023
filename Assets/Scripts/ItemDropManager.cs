using UnityEngine;

namespace AGL.Asteroids
{
    public class ItemDropManager : MonoBehaviour
    {
        // Reasoning for this setup: I want things to be mostly in charge of what they want to drop,
        // for instance making the engine only drop from a UFO, ect. But they also shouldn't
        // drop duplicates, hence this function exists to take care of that. This manager
        // largely will just keep track of the current item objectives, and see if something
        // is a duplicate or not.
        public bool ShouldDrop(GameObject itemPrefab)
        {
            // todo: real functionality once items are good
            return true;
        }
    }
}