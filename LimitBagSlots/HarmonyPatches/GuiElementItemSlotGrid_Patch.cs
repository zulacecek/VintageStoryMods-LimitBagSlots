using HarmonyLib;
using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace LimitBagSlots.HarmonyPatches
{
    [HarmonyPatch]
    public class GuiElementItemSlotGrid_Patch
    {
        private static readonly AccessTools.FieldRef<GuiElementItemSlotGrid, IInventory> inventoryFieldRef =
                AccessTools.FieldRefAccess<GuiElementItemSlotGrid, IInventory>("inventory");

        private static readonly AccessTools.FieldRef<GuiElementItemSlotGrid, OrderedDictionary<int, ItemSlot>> availableSlotsFieldRef =
            AccessTools.FieldRefAccess<GuiElementItemSlotGrid, OrderedDictionary<int, ItemSlot>>("availableSlots");

        private static readonly AccessTools.FieldRef<GuiElementItemSlotGrid, OrderedDictionary<int, ItemSlot>> renderedSlotsFieldRef =
            AccessTools.FieldRefAccess<GuiElementItemSlotGrid, OrderedDictionary<int, ItemSlot>>("renderedSlots");

        [HarmonyPatch(typeof(GuiElementItemSlotGrid), "DetermineAvailableSlots")]
        [HarmonyPrefix]
        static bool DetermineAvailableSlotsPrefix(GuiElementItemSlotGrid __instance, int[] visibleSlots)
        {
            const int numberOfVanillaSlots = LimitBagSlotsConfig.NumberOfVanillaBagSlots;

            // Only do this for bag slots. There is always 4 of them comming in the visibleSlots parameters
            // Accessing the inventory through reflection is slow and can lag if done too many times.
            if (visibleSlots == null || visibleSlots.Length != numberOfVanillaSlots)
            {
                return true;
            }

            var inventory = inventoryFieldRef(__instance);
            if (inventory == null || inventory.Count == 0)
            {
                return true;
            }

            var availableSlots = availableSlotsFieldRef(__instance) ?? new OrderedDictionary<int, ItemSlot>();
            var renderedSlots = renderedSlotsFieldRef(__instance) ?? new OrderedDictionary<int, ItemSlot>();

            availableSlots.Clear();
            renderedSlots.Clear();

            for (int i = 0; i < Math.Min(LimitBagSlotsModSystem.LimitBagSlotsConfig.BagSlotLimit, numberOfVanillaSlots); i++)
            {
                availableSlots.Add(i, inventory[i]);
                renderedSlots.Add(i, inventory[i]);
            }

            return false;
        }
    }
}
