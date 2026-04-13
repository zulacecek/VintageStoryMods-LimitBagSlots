using HarmonyLib;
using Vintagestory.API.Common;

namespace LimitBagSlots.HarmonyPatches
{
    [HarmonyPatch]
    public class CollectibleObject_Patch
    {
        [HarmonyPatch(typeof(CollectibleObject), "GetStorageFlags")]
        [HarmonyPostfix]
        static void GetStorageFlagsPostfix(CollectibleObject __instance, ItemStack itemstack, ref EnumItemStorageFlags __result)
        {
            var collectible = itemstack?.Collectible;
            if (collectible == null || !__result.HasFlag(EnumItemStorageFlags.Backpack))
            {
                return;
            }

            var config = LimitBagSlotsModSystem.LimitBagSlotsConfig;
            if (BackSlotsLimitRules.ForbidItem(collectible, config.ForbidBagsFromSlots, config.ForbidNonBagsFromBagSlots))
            {
                __result = EnumItemStorageFlags.General;
            }
        }
    }
}
