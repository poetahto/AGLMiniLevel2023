
namespace InventorySystem
{
    /// <summary>
    /// Interface for collectable items. Used by the player to discern whether or no an item in the world is collectable.
    /// </summary>
    public interface ICollectable
    {
        public void Collect(Inventory _inventory);
    }
}
