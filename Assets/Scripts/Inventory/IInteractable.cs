using UnityEngine;

namespace InventorySystem
{
    public interface IInteractable
    {
        public GameObject Object { get; }
        public void Select();
        public void DeSelect();
        public void Interact(Transform _user);
    }
}
