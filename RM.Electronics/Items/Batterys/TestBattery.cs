
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.RM.Core.Components;
using Eco.Shared;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;

namespace Eco.RM.Electronics.Items.Batterys
{
    [Serialized, LocDisplayName("Test Battery")]
    public partial class TestBatteryItem : WorldObjectItem<TestBatteryObject> { }

    [Serialized, RequireComponent(typeof(BatteryDischargingComponent))]
    public partial class TestBatteryObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(TestBatteryItem);
        public override LocString DisplayName => Localizer.DoStr("Test Battery");
        public void Initalize()
        {
            this.GetComponent<BatteryDischargingComponent>().Initalize(10);
        }
        public TestBatteryObject()
        {
            WorldObject.AddOccupancy<TestBatteryObject>(new List<BlockOccupancy> { new BlockOccupancy(new Shared.Math.Vector3i(0, 0, 0)) });
        }
    }
}
