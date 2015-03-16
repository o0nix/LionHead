using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LionHead.Core.Entities
{
    public class LootTableItem
    {
        public readonly string ItemId;
        public readonly byte DropChance;
        public LootTableItem(string itemId, byte dropChance)
        {
            if (dropChance > 100) throw new ArgumentOutOfRangeException("Drop chance over 100");
            ItemId = itemId;
            DropChance = dropChance;
        }
    }
}
