
namespace InventorySystem
{
    public interface ICollectable : IInteractable
    {
        public int StackSize { get; set; }
        public InventoryItem Collect(Inventory _inventory);
    }
}
