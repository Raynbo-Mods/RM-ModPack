using Eco.Core.Items;
using Eco.EM.Framework.Resolvers;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.RM.Core.Components;
using Eco.RM.Core.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;
using System.ComponentModel;

namespace Eco.RM.Electronics.Items
{
    [Serialized, Category("Batteries"), LocDisplayName("Small Battery"), Weight(300)]
    public class SmallBatteryItem : BatteryItem, IConfigurableCustoms
    {
        public override int ScrapOutput => 2;
        //protected override int BaseLifespan => (int)TimeUtil.HoursToSeconds(120); //five days
        protected override int BaseLifespan => 30;
        public int MaxChargeRate => (int)EMCustomsResolver.GetCustom(typeof(SmallBatteryItem), "MaxChargeRate");
        public int MaxDischargeRate => (int)EMCustomsResolver.GetCustom(typeof(SmallBatteryItem), "MaxDischargeRate");
        public int MaxCharge => (int)EMCustomsResolver.GetCustom(typeof(SmallBatteryItem), "MaxCharge");
        public float CurrentCharge => (float)EMCustomsResolver.GetCustom(typeof(SmallBatteryItem), "Staring Charge");
        static SmallBatteryItem()
        {
            Dictionary<string, object> defaults = new Dictionary<string, object>();
            defaults.Add("Max Charge", 60);
            defaults.Add("Max Charge Rate", 30);
            defaults.Add("Max Discharge Rate", 20);
            defaults.Add("Starting Charge", 0);
            EMCustomsResolver.AddDefaults(new CustomsModel(typeof(SmallBatteryItem), defaults));
        }
        public SmallBatteryItem() { }
    }
    [RequiresSkill(typeof(MechanicsSkill), 4)]
    public class SmallBatteryRecipe : RecipeFamily, IConfigurableRecipe
    {
        static RecipeDefaultModel defaults => new()
        {
            ModelType = typeof(SmallBatteryRecipe).Name,
            Assembly = typeof(SmallBatteryRecipe).AssemblyQualifiedName,
            HiddenName = "Small Battery Recipe",
            LocalizableName = Localizer.DoStr("Small Battery Recipe"),
            IngredientList = new()
            {
                new EMIngredient(typeof(CopperPlateItem).Name, false, 10, true),
                new EMIngredient(typeof(IronPlateItem).Name, false, 10, true),
                new EMIngredient(typeof(CoalItem).Name, false, 60)
            },
            ProductList = new()
            {
                new EMCraftable(typeof(SmallBatteryItem).Name)
            },
            BaseExperienceOnCraft = 3,
            BaseLabor = 500,
            LaborIsStatic = false,
            BaseCraftTime = 10,
            CraftTimeIsStatic = false,
            CraftingStation = typeof(MachinistTableItem).Name,
            RequiredSkillType = typeof(MechanicsSkill),
            RequiredSkillLevel = 4,
            IngredientImprovementTalents = typeof(MechanicsLavishResourcesTalent),
            SpeedImprovementTalents = new Type[] { typeof(MechanicsParallelSpeedTalent), typeof(MechanicsFocusedSpeedTalent) },
        };
        static SmallBatteryRecipe() { EMRecipeResolver.AddDefaults(defaults); }
        public SmallBatteryRecipe()
        {
            Recipes = EMRecipeResolver.Obj.ResolveRecipe(this);
            LaborInCalories = EMRecipeResolver.Obj.ResolveLabor(this);
            CraftMinutes = EMRecipeResolver.Obj.ResolveCraftMinutes(this);
            ExperienceOnCraft = EMRecipeResolver.Obj.ResolveExperience(this);
            Initialize(defaults.LocalizableName, GetType());
            CraftingComponent.AddRecipe(EMRecipeResolver.Obj.ResolveStation(this), this);
        }
    }
}