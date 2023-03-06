using Eco.Gameplay.Components;
using Eco.Gameplay.Objects;
using Eco.Shared.IoC;
using Eco.Shared.Localization;

namespace Eco.RM.Core.Components
{
    /// <summary>Discharges from a battery in a battery supply component that is made automaticly.</summary>
    public partial class BatteryChargingComponent : WorldObjectComponent
    {
        public StatusElement Status { get; private set; }
        public bool Enabled => Supply.Battery.CurrentCharge > 0 && Supply.Battery.MaxDischargeRate <= watts;
        public int watts
        {
            get => watts;
            private set
            {
                if (value == watts) return;
                watts = value;
                OverrideWatts(watts, BaseWatts);
            }
        }
        public int BaseWatts { get; private set; }
        public float wh => watts / ServiceHolder<IWorldObjectManager>.Obj.TickDeltaTime / 3600;
        /// <summary>the battery supply component to pull from.</summary>
        public BatterySupplyComponent Supply { get; private set; }
        BatteryChargingComponent(int watts, int BasePower)
        {
            this.watts = watts;
            this.BaseWatts = BasePower;
            this.Supply = this.Parent.GetOrCreateComponent<BatterySupplyComponent>();
            this.Parent.GetOrCreateComponent<PowerConsumptionComponent>().Initialize(watts + BasePower);
            this.Parent.GetOrCreateComponent<PowerGridComponent>().Initialize(20, new ElectricPower());
            this.Status = this.Parent.GetOrCreateComponent<StatusComponent>().CreateStatusElement();
            this.Status.SetStatusMessage(this.Enabled, new LocString($"Discharging Battery"), new LocString("No Charge In Battery"));
        }
        /// <summary>override the power consumption</summary>
        public void OverrideWatts(int watts, int BasePower)
        {
            this.watts = watts;
            this.BaseWatts = BasePower;
            this.Parent.GetOrCreateComponent<PowerConsumptionComponent>().OverridePowerConsumption(watts + BasePower);
        }
        public override void Tick()
        {
            base.Tick();
            if (Supply.Battery == null) return;
            if (Supply.Battery.MaxChargeRate > watts) watts = Supply.Battery.MaxChargeRate;
            if (Supply.Battery.CurrentCharge > 0) Supply.Battery.CurrentCharge -= wh;
            if (Supply.Battery.CurrentCharge < 0) Supply.Battery.CurrentCharge = 0;
        }
    }
}