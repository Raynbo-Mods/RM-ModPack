using Eco.Core.Controller;
using Eco.EM.Framework.ChatBase;
using Eco.EM.Framework.Extentsions.Items;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Shared.IoC;
using Eco.Shared.Localization;
using Eco.Shared.Networking;
using Eco.Shared.Utils;
using System.ComponentModel;

namespace Eco.RM.Core.Components
{
    /// <summary>Discharges from a battery in a battery supply component that is made automaticly.</summary>
    public partial class BatteryDischargingComponent : WorldObjectComponent, IController, INotifyPropertyChanged
    {
        #region IController
        public new event PropertyChangedEventHandler PropertyChanged;
        int controllerID;
        public new ref int ControllerID => ref this.controllerID;
        #endregion

        public StatusElement Status { get; private set; }
        public bool Enabled => Supply.Battery.CurrentCharge > 0 && Supply.Battery.MaxDischargeRate <= watts;
        public int watts { get;private set; }
        public float wh => watts / ServiceHolder<IWorldObjectManager>.Obj.TickDeltaTime / 3600;
        /// <summary>the battery supply component to pull from.</summary>
        public BatterySupplyComponent Supply { get; private set; }
        BatteryDischargingComponent() { }
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
        public override void Tick()
        {
            base.Tick();
            if (Supply.Battery == null) return;
            if (Supply.Battery.MaxDischargeRate > watts) return;
            if (Supply.Battery.CurrentCharge > 0) Supply.Battery.CurrentCharge -= wh;
            if (Supply.Battery.CurrentCharge < 0) Supply.Battery.CurrentCharge = 0;
        }
        [Eco, ClientInterfaceProperty, GuestHidden]
        public Eco.Gameplay.Items.Inventory inventory { get => this.Supply.Inventory; }

        [RPC, Autogen, GuestHidden]
        public void Ping(Player player)
        {
            ChatBaseExtended.CBAnnouncement("Pong!   "+Supply.Battery.Name, true, true);
        }
    }
}
