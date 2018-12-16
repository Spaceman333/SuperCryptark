using System;
using System.Collections.Generic;

namespace Medusa
{
    public enum InventorySlot
    {
        ABILITY,
        PASSIVE_1,
        PASSIVE_2,
        PASSIVE_3,
        PASSIVE_4,
        WEAPON_1,
        WEAPON_2,
        WEAPON_3,
        WEAPON_4,
        ITEM_1,
        ITEM_2,
        ITEM_3,
        ITEM_4,
        ITEM_5,
        ITEM_6,
        KEYS,
        NONE,
        PASSIVES,
    }

    public class Inventory
    {
        public Entity Entity;
        public Dictionary<InventorySlot, InventoryItem> Items;
        public Inventory SourceInventory;   // Used by player 2 (for ammo, keys, etc)
        public List<InventoryItem> PassiveItems; // used in rogue mode to have unlimited amount of passive items.

        public Inventory(Entity entity)
        {
            Entity = entity;
            Items = new Dictionary<InventorySlot, InventoryItem>();
            PassiveItems = new List<InventoryItem>();
        }
    }
}
