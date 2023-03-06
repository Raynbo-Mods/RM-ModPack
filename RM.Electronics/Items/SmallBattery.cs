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
    [Serialized, Category("Batteries"), LocDisplayName("Small Battery"), Weight(300)]
    public class SmallBatteryItem : BatteryItem, IConfigurableCustoms
    {
        public int MaxChargeRate => (int)EMCustomsResolver.GetCustom(typeof(SmallBatteryItem), "MaxChargeRate");
        public int MaxDischargeRate => (int)EMCustomsResolver.GetCustom(typeof(SmallBatteryItem), "MaxDischargeRate");
        public int MaxCharge => (int)EMCustomsResolver.GetCustom(typeof(SmallBatteryItem), "MaxCharge");
        static SmallBatteryItem()
        {
            Dictionary<string, object> defaults = new Dictionary<string, object>();
            defaults.Add("MaxCharge", 60);
            defaults.Add("MaxChargeRate", 30);
            defaults.Add("MaxDischargeRate", 20);
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
            BaseLabor = 300,
            LaborIsStatic = false,
            BaseCraftTime = 20,
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
            this.Recipes = EMRecipeResolver.Obj.ResolveRecipe(this);
            this.LaborInCalories = EMRecipeResolver.Obj.ResolveLabor(this);
            this.CraftMinutes = EMRecipeResolver.Obj.ResolveCraftMinutes(this);
            this.ExperienceOnCraft = EMRecipeResolver.Obj.ResolveExperience(this);
            this.Initialize(defaults.LocalizableName, GetType());
            CraftingComponent.AddRecipe(EMRecipeResolver.Obj.ResolveStation(this), this);
        }
    }
}