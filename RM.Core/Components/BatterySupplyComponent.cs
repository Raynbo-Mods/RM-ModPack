using Eco.Core.Controller;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.RM.Core.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;
using System.ComponentModel;
using static Eco.Gameplay.Items.AuthorizationInventory;

namespace Eco.RM.Core.Components
{
    /// <summary>holds the battery storage for use in varius other components</summary>
    [LocDisplayName("Battery Supply")]
    public class BatterySupplyComponent : StorageComponent
    {

        /// <summary>The inventory to hold batteries</summary>
        [Serialized] public override Inventory Inventory { get; } = new AuthorizationInventory(1, AuthorizationFlags.AuthedMayAdd | AuthorizationFlags.AuthedMayRemove);
        public StatusElement Status { get; private set; }
        public bool Enabled => Inventory.NonEmptyStacks.Any();
        /// <summary>The battery currently inserted. returns null if none.
        public BatteryItem? Battery => GetBattery();
        public BatterySupplyComponent()
        {
            this.Status = this.Parent.GetOrCreateComponent<StatusComponent>().CreateStatusElement();
            this.Status.SetStatusMessage(this.Enabled, new LocString("Battery Inserted"), new LocString("Missing Battery"));
            this.Inventory.AddInvRestriction(new TagRestriction(new string[] { "Batteries" }));
        }
        private BatteryItem? GetBattery() {
            BatteryItem? battery = null;
            this.Inventory.NonEmptyStacks.ForEach(itemStack =>
            {
                battery = (BatteryItem)itemStack.Item;
            });
            return battery;
        }
    }
}
