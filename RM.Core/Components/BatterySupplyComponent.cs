using Eco.Core.Controller;
using Eco.Core.Utils;
using Eco.Gameplay.Components;
using Eco.Gameplay.Economy;
using Eco.Gameplay.Interactions;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Tutorial.Internal;
using Eco.RM.Core.Items;
using Eco.Shared.IoC;
using Eco.Shared.Localization;
using Eco.Shared.Networking;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;
using System.ComponentModel;
using static Eco.Gameplay.Items.AuthorizationInventory;

namespace Eco.RM.Core.Components
{
    /// <summary>holds the battery storage for use in varius other components</summary>
    [LocDisplayName("Battery Supply"), Priority(PriorityAttribute.High), LocDescription("View Battery Status")]
    public class BatterySupplyComponent : StorageComponent, IController, INotifyPropertyChanged
    {
        #region IController
        public new event PropertyChangedEventHandler PropertyChanged;
        int controllerID;
        public new ref int ControllerID => ref this.controllerID;
        #endregion
        public override WorldObjectComponentClientAvailability Availability => WorldObjectComponentClientAvailability.UI;
        /// <summary>The inventory to hold batteries</summary>
        [Serialized, SyncToView] public override Inventory Inventory { get; } = new AuthorizationInventory(1, AuthorizationFlags.AuthedMayAdd | AuthorizationFlags.AuthedMayRemove);
        public StatusElement Status { get; private set; }
        public bool Enabled => Inventory.NonEmptyStacks.Any();
        /// <summary>The battery currently inserted. returns null if none.
        [SyncToView, Notify] public BatteryItem? Battery => (BatteryItem?)Inventory.NonEmptyStacks.FirstOrDefault(x => true)?.Item ?? null;
        [SyncToView, Notify] public float CurrentCharge => Battery?.CurrentCharge ?? 0;
        public BatterySupplyComponent()
        {
            this.Status = this.Parent.GetOrCreateComponent<StatusComponent>().CreateStatusElement();
            this.Status.SetStatusMessage(this.Enabled, new LocString("Battery Inserted"), new LocString("Missing Battery"));
            this.Inventory.AddInvRestriction(new TagRestriction(new string[] { "Batteries" }));
        }
    }
}
