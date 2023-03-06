using Eco.EM.Framework.Resolvers;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.RM.Core.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.ComponentModel;

namespace Eco.RM.Electronics.Items
{
    [Serialized, Category("Batteries"), LocDisplayName("Carbon Battery"), Weight(300)]
    public class CarbonBatteryItem : BatteryItem, IConfigurableCustoms
    {
        public int MaxChargeRate => (int)EMCustomsResolver.GetCustom(typeof(CarbonBatteryItem), "Max Charge Rate");
        public int MaxDischargeRate => (int)EMCustomsResolver.GetCustom(typeof(CarbonBatteryItem), "Max Discharge Rate");
        public int MaxCharge => (int)EMCustomsResolver.GetCustom(typeof(CarbonBatteryItem), "Max Charge");
        public float CurrentCharge => (float)EMCustomsResolver.GetCustom(typeof(CarbonBatteryItem), "Staring Charge");
        static CarbonBatteryItem()
        {
            Dictionary<string, object> defaults = new Dictionary<string, object>();
            defaults.Add("Max Charge", 60);
            defaults.Add("Max Charge Rate", 0);
            defaults.Add("Max Discharge Rate", 10);
            defaults.Add("Starting Charge", 60);
            EMCustomsResolver.AddDefaults(new CustomsModel(typeof(CarbonBatteryItem), defaults));
        }
        public CarbonBatteryItem() { }
    }
    [RequiresSkill(typeof(MechanicsSkill), 2)]
    public class CarbonBatteryRecipe : RecipeFamily, IConfigurableRecipe
    {
        static RecipeDefaultModel defaults => new()
        {
            ModelType = typeof(CarbonBatteryRecipe).Name,
            Assembly = typeof(CarbonBatteryRecipe).AssemblyQualifiedName,
            HiddenName = "Carbon Battery",
            LocalizableName = Localizer.DoStr("Carbon Battery"),
            IngredientList = new()
            {
                new EMIngredient(typeof(CopperPlateItem).Name, false, 10, true),
                new EMIngredient(typeof(IronPlateItem).Name, false, 10, true),
                new EMIngredient(typeof(CoalItem).Name, false, 120)
            },
            ProductList = new()
            {
                new EMCraftable(typeof(SmallBatteryItem).Name)
            },
            BaseExperienceOnCraft = 1,
            BaseLabor = 300,
            LaborIsStatic = false,
            BaseCraftTime = 5,
            CraftTimeIsStatic = false,
            CraftingStation = typeof(MachinistTableItem).Name,
            RequiredSkillType = typeof(MechanicsSkill),
            RequiredSkillLevel = 2,
            IngredientImprovementTalents = typeof(MechanicsLavishResourcesTalent),
            SpeedImprovementTalents = new Type[] { typeof(MechanicsParallelSpeedTalent), typeof(MechanicsFocusedSpeedTalent) },
        };
        static CarbonBatteryRecipe() { EMRecipeResolver.AddDefaults(defaults); }
        public CarbonBatteryRecipe()
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