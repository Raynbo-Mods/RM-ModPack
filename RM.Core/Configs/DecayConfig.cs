using Eco.Shared.Localization;

namespace Eco.RM.Core.Configs
{
    public class DecayConfig
    {
        [LocDescription("Battery Items Decay Enabled")]
        public bool BatteryItemDecayEnabled { get; set; } = true;

        [LocDescription("Battery Items Decay Multiplyer")]
        public float BatteryItemDecayMultiplyer { get; set; } = 1;

        [LocDescription("Battery Objects Decay Enabled")]
        public bool BatteryObjectDecayEnabled { get; set; } = true;

        [LocDescription("Battery Objects Decay Multiplyer")]
        public float BatteryObjectDecayMultiplyer { get; set; } = 1;
    }
}
