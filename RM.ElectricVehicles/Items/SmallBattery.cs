using Eco.EM.Framework.Console;
using Eco.EM.Framework.Resolvers;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.RM.Core.Items;
using Eco.Shared.Localization;
using System.ComponentModel;

namespace Eco.RM.Electronics.Items
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
    [RequiresSkill(typeof(MechanicsSkill), 4)]
    public class SmallBatteryRecipe : RecipeFamily, IConfigurableRecipe
    {
        static RecipeDefaultModel defaults => new()
        {
            ModelType = typeof(SmallBatteryRecipe).Name,
            Assembly = typeof(SmallBatteryRecipe).AssemblyQualifiedName,
            HiddenName = "Small Battery",
            LocalizableName = Localizer.DoStr("Small Battery"),
            IngredientList = new()
            {
                new EMIngredient("CopperPlateItem", false, 10, true),
                new EMIngredient("IronPlateItem", false, 10, true),
                new EMIngredient("CoalItem", false, 60)
            },
            ProductList = new()
            {
                new EMCraftable("SmallBatteryItem")
            },
            BaseExperienceOnCraft = 3,
            BaseLabor = 300,
            LaborIsStatic = false,
            BaseCraftTime = 20,
            CraftTimeIsStatic = false,
            CraftingStation = "MachinistTableObject",
            RequiredSkillType = typeof(MechanicsSkill),
            RequiredSkillLevel = 4,
            IngredientImprovementTalents = typeof(MechanicsLavishResourcesTalent),
            SpeedImprovementTalents = new Type[] { typeof(MechanicsParallelSpeedTalent), typeof(MechanicsFocusedSpeedTalent) },
        };
        static SmallBatteryRecipe() { EMRecipeResolver.AddDefaults(defaults); ConsoleWriter.TextWriter(ConsoleColor.Cyan, "loading"); }
        public SmallBatteryRecipe()
        {
            this.Recipes = EMRecipeResolver.Obj.ResolveRecipe(this);
            this.LaborInCalories = EMRecipeResolver.Obj.ResolveLabor(this);
            this.CraftMinutes = EMRecipeResolver.Obj.ResolveCraftMinutes(this);
            this.ExperienceOnCraft = EMRecipeResolver.Obj.ResolveExperience(this);
            this.Initialize(defaults.LocalizableName, GetType());
            CraftingComponent.AddRecipe(EMRecipeResolver.Obj.ResolveStation(this), this);
            ConsoleWriter.TextWriter(ConsoleColor.Cyan, "loaded");
        }
    }
}