using System.Collections.Generic;
using System.Linq;

namespace InventorySystem.Utilities
{
    public static class Extensions
    {
        public static bool TryGet(this IEnumerable<InventoryItem> list, ItemData _data, out InventoryItem _item)
        {
            _item = list.FirstOrDefault(i => i.Data == _data);
            return (_item != null);
        }
    }
}