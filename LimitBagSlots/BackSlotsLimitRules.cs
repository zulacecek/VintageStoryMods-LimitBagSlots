using System;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace LimitBagSlots
{
    public static class BackSlotsLimitRules
    {
        public const string BackpacksInventoryName = "backpack";

        public static bool ForbidItem(CollectibleObject? item, bool forbidBagsFromSlots, bool forbidNonBagsFromBagSlots)
        {
            if (item == null || !item.StorageFlags.HasFlag(EnumItemStorageFlags.Backpack))
            {
                return false;
            }

            var isBag = item.HasBehavior(typeof(IHeldBag), true);

            return (forbidBagsFromSlots && isBag)
                || (forbidNonBagsFromBagSlots && !isBag);
        }

        public static void TryRemovingForbidenItemsInBagSlots(IServerPlayer? player)
        {
            var config = LimitBagSlotsModSystem.LimitBagSlotsConfig;
            if (!config.ForceForbidenItemsDrop || player == null)
            {
                return;
            }

            var backpacks = player.InventoryManager.GetOwnInventory(BackpacksInventoryName);
            if (backpacks == null)
            {
                return;
            }

            var inventoryCleared = false;
            for (int i = 0; i < Math.Min(config.BagSlotLimit, 4); i++)
            {
                if (backpacks.Count < i)
                {
                    break;
                }

                var slot = backpacks[i];
                var item = slot?.Itemstack?.Collectible;
                if (ForbidItem(item, config.ForbidBagsFromSlots, config.ForbidNonBagsFromBagSlots))
                {
                    if (!inventoryCleared && config.ForbidBagsFromSlots)
                    {
                        ForceDropPlayerInvetoryContent(player);
                        inventoryCleared = true;
                    }

                    ForceDrop(player, slot);
                }
            }
        }

        private static void ForceDrop(IServerPlayer player, ItemSlot? slot)
        {
            if (slot == null || slot.Empty)
            {
                return;
            }


            ItemStack stack = slot.TakeOutWhole();
            slot.MarkDirty();

            player.Entity.World.SpawnItemEntity(
                stack,
                player.Entity.Pos.XYZ
            );
        }

        private static void ForceDropPlayerInvetoryContent(IServerPlayer player)
        {
            var backpackSlots = player.InventoryManager.GetOwnInventory(BackpacksInventoryName);
            foreach (var slot in backpackSlots)
            {
                // Skip backpacks as the logic for those is applied afterwards
                if (slot.StorageType.HasFlag(EnumItemStorageFlags.Backpack))
                {
                    continue;
                }

                ForceDrop(player, slot);
            }
        }
    }
}
