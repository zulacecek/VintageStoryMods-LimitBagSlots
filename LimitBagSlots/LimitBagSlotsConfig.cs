namespace LimitBagSlots
{
    public class LimitBagSlotsConfig
    {
        public const int NumberOfVanillaBagSlots = 4;

        public int BagSlotLimit { get; set; } = 1;

        public bool ForbidBagsFromSlots { get; set; } = false;

        public bool ForbidNonBagsFromBagSlots { get; set; } = true;
    }
}
