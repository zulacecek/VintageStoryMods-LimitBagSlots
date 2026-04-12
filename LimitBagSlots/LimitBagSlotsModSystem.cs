using HarmonyLib;
using System;
using Vintagestory.API.Common;

namespace LimitBagSlots
{
    public class LimitBagSlotsModSystem : ModSystem
    {
        public static LimitBagSlotsConfig LimitBagSlotsConfig { get; private set; } = new LimitBagSlotsConfig();
        private static Harmony? _harmony;

        public override void Start(ICoreAPI api)
        {
            Mod.Logger.Notification("Hello from template mod: " + api.Side);

            TryToLoadConfig(api);

            _harmony = new Harmony(Mod.Info.ModID);
            _harmony.PatchAll();

        }

        public override void Dispose()
        {
            _harmony?.UnpatchAll();
            base.Dispose();
        }

        private void TryToLoadConfig(ICoreAPI api)
        {
            var fileName = "LimitBagSlotsConfig.json";

            try
            {
                LimitBagSlotsConfig = api.LoadModConfig<LimitBagSlotsConfig>(fileName) ?? new LimitBagSlotsConfig();

                api.StoreModConfig(LimitBagSlotsConfig, fileName);
            }
            catch (Exception e)
            {
                Mod.Logger.Error("Could not load config! Loading default settings instead.");
                Mod.Logger.Error(e);
            }
        }
    }
}
