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

            LimitBagSlotsModSystem.LimitBagSlotsConfig.ForbidBagsFromSlots = false;
            LimitBagSlotsModSystem.LimitBagSlotsConfig.ForbidNonBagsFromBagSlots = true;

            var isBag = collectible.HasBehavior(typeof(IHeldBag), true);
            if ((LimitBagSlotsModSystem.LimitBagSlotsConfig.ForbidBagsFromSlots && isBag)
                || (LimitBagSlotsModSystem.LimitBagSlotsConfig.ForbidNonBagsFromBagSlots && !isBag))
            {
                __result = EnumItemStorageFlags.General;
            }
        }
    }
}
