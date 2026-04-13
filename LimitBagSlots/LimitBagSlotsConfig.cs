namespace LimitBagSlots
{
    public class LimitBagSlotsConfig
    {
        public const int NumberOfVanillaBagSlots = 4;

        public int BagSlotLimit { get; set; } = 1;
        public bool ForbidBagsFromSlots { get; set; }
        public bool ForbidNonBagsFromBagSlots { get; set; } = true;
        public bool ForceForbidenItemsDrop { get; set; } = true;
        public bool ForceInvetoryDrop { get; set; }
    }
}
