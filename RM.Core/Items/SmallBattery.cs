using Eco.EM.Framework;
using Eco.EM.Framework.Resolvers;
using Eco.Gameplay.Items;
using System.ComponentModel;

namespace Eco.RM.Core.Items
{
    [Category("Batteries")]
    public class SmallBatteryItem : BatteryItem, IConfigurableCustoms
    {
        public int MaxChargeRate => (int)EMCustomsResolver.GetCustom(typeof(SmallBatteryItem), "MaxChargeRate");
        public int MaxDischargeRate => (int)EMCustomsResolver.GetCustom(typeof(SmallBatteryItem), "MaxDischargeRate");
        public int MaxCharge => (int)EMCustomsResolver.GetCustom(typeof(SmallBatteryItem), "MaxCharge");
        public SmallBatteryItem()
        {
            Dictionary<string, object> defaults = new Dictionary<string, object>();
            defaults.Add("MaxCharge", 60);
            defaults.Add("MaxChargeRate", 30);
            defaults.Add("MaxDischargeRate", 20);
            EMCustomsResolver.AddDefaults(new CustomsModel(typeof(SmallBatteryItem), defaults));
        }
    }
}