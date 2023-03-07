using Eco.Shared.Localization;

namespace Eco.RM.Core.Configs
{
    public class DecayConfig
    {
        [LocDescription("Items Decay Enabled")]
        public bool ItemDecayEnabled { get; set; } = true;

        [LocDescription("Items Decay Multiplyer")]
        public float ItemDecayMultiplyer { get; set; } = 1;

        [LocDescription("Object Decay Enabled")]
        public bool ObjectDecayEnabled { get; set; } = true;

        [LocDescription("Object Decay Multiplyer")]
        public float ObjectDecayMultiplyer { get; set; } = 1;

        [LocDescription("Battery Item Power Decay Enabled")]
        public bool BatteryItemDecayEnabled { get; set; } = true;

        [LocDescription("Battery Item Power Decay Multiplyer")]
        public float BatteryItemDecayMultiplyer { get; set; } = 1;

        [LocDescription("Battery Object Power Decay Enabled")]
        public bool BatteryObjectDecayEnabled { get; set; } = true;

        [LocDescription("Battery Object Power Decay Multiplyer")]
        public float BatteryObjectDecayMultiplyer { get; set; } = 1;
    }
}
