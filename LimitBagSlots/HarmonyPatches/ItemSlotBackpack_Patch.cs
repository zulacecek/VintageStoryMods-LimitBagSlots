using HarmonyLib;
using System;
using Vintagestory.API.Common;

namespace LimitBagSlots.HarmonyPatches
{
    [HarmonyPatch]
    public class ItemSlotBackpack_Patch
    {
        [HarmonyPatch(typeof(ItemSlotBackpack), "StorageType", MethodType.Getter)]
        [HarmonyPostfix]
        static void StorageTypePostfix(ItemSlotBackpack __instance, ref EnumItemStorageFlags __result)
        {
            var index = __instance.Inventory.GetSlotId(__instance);

            if (index > Math.Min(LimitBagSlotsModSystem.LimitBagSlotsConfig.BagSlotLimit - 1, LimitBagSlotsConfig.NumberOfVanillaBagSlots))
            {
                __result = 0;
                return;
            }
        }
    }
}
