using Eco.Core.Controller;
using Eco.Core.Serialization;
using Eco.EM.Framework.ChatBase;
using Eco.Gameplay.Components;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems;
using Eco.Shared.IoC;
using Eco.Shared.Localization;
using Eco.Shared.Networking;
using Eco.Shared.Serialization;
using Eco.Shared.View;
using System.ComponentModel;

namespace Eco.RM.Core.Components
{
    /// <summary>Discharges from a battery in a battery supply component that is made automaticly.</summary>
    [LocDisplayName("Battery Discharge"), Serialized]
    public class BatteryDischargingComponent : WorldObjectComponent
    {
        public StatusElement Status { get; private set; }
        public bool Enabled => Supply.Battery.CurrentCharge > 0 && Supply.Battery.MaxDischargeRate <= watts;
        public int watts { get;private set; }
        public float wh => watts / ServiceHolder<IWorldObjectManager>.Obj.TickDeltaTime / 3600;
        /// <summary>the battery supply component to pull from.</summary>
        public BatterySupplyComponent Supply { get; private set; }
        public BatteryDischargingComponent() { }
        public void Initalize(int watts)
        {
            this.watts = watts;
            this.Supply = this.Parent.GetOrCreateComponent<BatterySupplyComponent>();
            this.Status = this.Parent.GetOrCreateComponent<StatusComponent>().CreateStatusElement();
            this.Status.SetStatusMessage(this.Enabled, new LocString($"Discharging Battery"), new LocString("No Charge In Battery"));
        }
        /// <summary>override the power consumption</summary>
        public void OverrideWatts(int watts)
        {
            this.watts = watts;
        }
        public void Tick()
        {
            if (Supply.Battery == null) return;
            if (Supply.Battery.MaxDischargeRate > watts) return;
            if (Supply.Battery.CurrentCharge > 0) Supply.Battery.CurrentCharge -= wh;
            if (Supply.Battery.CurrentCharge < 0) Supply.Battery.CurrentCharge = 0;
        }
        
    }
}
