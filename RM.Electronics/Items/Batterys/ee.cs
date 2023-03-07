using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.RM.Core.Components;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;

namespace Eco.RM.Electronics.Items.Batterys
{
    [Serialized, LocDisplayName("Battery")]
    public partial class BatteryItem : WorldObjectItem<BatteryObject> { }
    [Serialized]
    [RequireComponent(typeof(BatteryDischargingComponent))]
    public partial class BatteryObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(SmallBatteryItem);
        public override LocString DisplayName => Localizer.DoStr("Battery");
        public void Initalize()
        {
            this.GetComponent<BatteryDischargingComponent>().Initalize(10);
        }
    }
}
