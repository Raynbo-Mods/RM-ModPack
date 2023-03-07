using Eco.EM.Framework.Resolvers;
using Eco.Gameplay.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.ComponentModel;

namespace Eco.RM.Core.Items
{
    [DisplayName("Scrap")]
    [MaxStackSize(1000)]
    [Serialized]
    public class ScrapItem : Item
    {
        public override LocString DisplayDescription => Localizer.DoStr("Recycle Me!");
        public ScrapItem() { }
    }
}
