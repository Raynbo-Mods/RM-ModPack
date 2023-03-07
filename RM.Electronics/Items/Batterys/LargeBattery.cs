using Eco.Core.Items;
using Eco.EM.Framework.Resolvers;
using Eco.Gameplay.Components;
using Eco.Gameplay.Items;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.RM.Core.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;
using System.ComponentModel;

namespace Eco.RM.Electronics.Items
{
    [Serialized, Category("Batteries"), LocDisplayName("Large Battery"), Weight(300)]
    public class LargeBatteryItem : BatteryItem, IConfigurableCustoms
    {
        public override int ScrapOutput => 4;
        protected override int BaseLifespan => (int)TimeUtil.HoursToSeconds(168);   //one week
        public int MaxChargeRate => (int)EMCustomsResolver.GetCustom(typeof(LargeBatteryItem), "Max Charge Rate");
        public int MaxDischargeRate => (int)EMCustomsResolver.GetCustom(typeof(LargeBatteryItem), "Max Discharge Rate");
        public int MaxCharge => (int)EMCustomsResolver.GetCustom(typeof(LargeBatteryItem), "Max Charge");
        public float CurrentCharge => (float)EMCustomsResolver.GetCustom(typeof(LargeBatteryItem), "Staring Charge");
        static LargeBatteryItem()
        {
            Dictionary<string, object> defaults = new Dictionary<string, object>();
            defaults.Add("Max Charge", 60);
            defaults.Add("Max Charge Rate", 30);
            defaults.Add("Max Discharge Rate", 20);
            defaults.Add("Staring Charge", 0);
            EMCustomsResolver.AddDefaults(new CustomsModel(typeof(LargeBatteryItem), defaults));
        }
        public LargeBatteryItem() { }
    }
    [RequiresSkill(typeof(IndustrySkill), 2)]
    public class LargeBatteryRecipe : RecipeFamily, IConfigurableRecipe
    {
        static RecipeDefaultModel defaults => new()
        {
            ModelType = typeof(LargeBatteryRecipe).Name,
            Assembly = typeof(LargeBatteryRecipe).AssemblyQualifiedName,
            HiddenName = "Large Battery Recipe",
            LocalizableName = Localizer.DoStr("Large Battery Recipe"),
            IngredientList = new()
            {
                new EMIngredient(typeof(CopperPlateItem).Name, false, 30, true),
                new EMIngredient(typeof(SteelPlateItem).Name, false, 20, true),
            },
            ProductList = new()
            {
                new EMCraftable(typeof(LargeBatteryItem).Name)
            },
            BaseExperienceOnCraft = 4,
            BaseLabor = 800,
            LaborIsStatic = false,
            BaseCraftTime = 20,
            CraftTimeIsStatic = false,
            CraftingStation = typeof(ElectricMachinistTableItem).Name,
            RequiredSkillType = typeof(IndustrySkill),
            RequiredSkillLevel = 2,
            IngredientImprovementTalents = typeof(IndustryLavishResourcesTalent),
            SpeedImprovementTalents = new Type[] { typeof(IndustryParallelSpeedTalent), typeof(IndustryFocusedSpeedTalent) },
        };
        static LargeBatteryRecipe() { EMRecipeResolver.AddDefaults(defaults); }
        public LargeBatteryRecipe()
        {
            Recipes = EMRecipeResolver.Obj.ResolveRecipe(this);
            LaborInCalories = EMRecipeResolver.Obj.ResolveLabor(this);
            CraftMinutes = EMRecipeResolver.Obj.ResolveCraftMinutes(this);
            ExperienceOnCraft = EMRecipeResolver.Obj.ResolveExperience(this);
            this.Initialize(defaults.LocalizableName, GetType());
            CraftingComponent.AddRecipe(EMRecipeResolver.Obj.ResolveStation(this), this);
        }
    }
}