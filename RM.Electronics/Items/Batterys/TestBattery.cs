
using Eco.Core.Items;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.RM.Core.Components;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.ComponentModel;

namespace Eco.RM.Electronics.Items
{
    [Serialized, Category("Batteries"), LocDisplayName("Stationary Battery"), Tag("Battery")]
    public partial class StationaryBatteryItem : WorldObjectItem<StationaryBatteryObject>
    {
        public StationaryBatteryItem() { }
    }

    [Serialized, RequireComponent(typeof(BatteryDischargingComponent))]
    public partial class StationaryBatteryObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(StationaryBatteryItem);
        public override LocString DisplayName => Localizer.DoStr("Stationary Battery");
        public void Initalize()
        {
            this.GetComponent<BatteryDischargingComponent>().Initalize(10);
        }
        public StationaryBatteryObject() { }
    }
}
