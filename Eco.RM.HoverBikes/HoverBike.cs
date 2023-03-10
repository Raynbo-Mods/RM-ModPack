using Eco.Core.Controller;
using Eco.Core.Items;
using Eco.EM.Framework.Resolvers;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Auth;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Skills;
using Eco.Gameplay.Systems.NewTooltip;
using Eco.Gameplay.Systems.Tooltip;
using Eco.Mods.TechTree;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;

namespace Eco.RM.HoverBikes
{
    [Serialized]
    [LocDisplayName("Hover Bike")]
    [Weight(10000)]
    [AirPollution(0.1f)]
    [Ecopedia("Crafted Objects", "Vehicles", true, true, null)]
    public class HoverBikeItem : WorldObjectItem<HoverBikeObject>, IPersistentData
    {
        public override LocString DisplayDescription => Localizer.DoStr("A hover bike.");

        [Serialized]
        [SyncToView(null, true)]
        [TooltipChildren(new Type[] { })]
        [NewTooltipChildren(CacheAs.Instance)]
        public object PersistentData { get; set; }
    }
    [Serialized]
    [RequireComponent(typeof(StandaloneAuthComponent), null)]
    [RequireComponent(typeof(FuelSupplyComponent), null)]
    [RequireComponent(typeof(FuelConsumptionComponent), null)]
    [RequireComponent(typeof(PublicStorageComponent), null)]
    [RequireComponent(typeof(TailingsReportComponent), null)]
    [RequireComponent(typeof(MovableLinkComponent), null)]
    [RequireComponent(typeof(AirPollutionComponent), null)]
    [RequireComponent(typeof(VehicleComponent), null)]
    [RequireComponent(typeof(CustomTextComponent), null)]
    [RequireComponent(typeof(MinimapComponent), null)]
    [Ecopedia("Crafted Objects", "Vehicles", false, true, "HoverBike Item")]
    public class HoverBikeObject : PhysicsWorldObject, IRepresentsItem, IConfigurableVehicle
    {
        public static VehicleModel defaults;
        private static readonly string[] fuelTagList;
        public override TableTextureMode TableTexture => TableTextureMode.Metal;
        public override bool PlacesBlocks => false;
        public override LocString DisplayName => Localizer.DoStr("Hover Bike");
        public Type RepresentedItemType => typeof(HoverBikeItem);

        static HoverBikeObject()
        {
            defaults = new VehicleModel(typeof(HoverBikeObject), "Hover Bike", fuelTagList, 2, 40f, 0.1f, 50f, 1f, 5f, 1000000f);
            fuelTagList = new string[1] { "Burnable Fuel" };
            WorldObject.AddOccupancy<HoverBikeObject>(new List<BlockOccupancy>(0));
            EMVehicleResolver.AddDefaults(defaults);
        }
        private HoverBikeObject()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            GetComponent<PublicStorageComponent>().Initialize(AutoSingleton<EMVehicleResolver>.Obj.ResolveStorageSlots(this), AutoSingleton<EMVehicleResolver>.Obj.ResolveMaxWeight(this));
            GetComponent<FuelSupplyComponent>().Initialize(AutoSingleton<EMVehicleResolver>.Obj.ResolveFuelSlots(this), AutoSingleton<EMVehicleResolver>.Obj.ResolveFuelTagList(this));
            GetComponent<FuelConsumptionComponent>().Initialize(AutoSingleton<EMVehicleResolver>.Obj.ResolveFuelConsumption(this));
            GetComponent<AirPollutionComponent>().Initialize(AutoSingleton<EMVehicleResolver>.Obj.ResolveAirPollution(this));
            GetComponent<VehicleComponent>().Initialize(AutoSingleton<EMVehicleResolver>.Obj.ResolveMaxSpeed(this), AutoSingleton<EMVehicleResolver>.Obj.ResolveEfficiencyMultiplier(this), AutoSingleton<EMVehicleResolver>.Obj.ResolveSeats(this));
            GetComponent<CustomTextComponent>().Initialize(20);
            GetComponent<MinimapComponent>().InitAsMovable();
            GetComponent<MinimapComponent>().SetCategory(Localizer.DoStr("Vehicles"));
        }
    }
    [RequiresSkill(typeof(MechanicsSkill), 2)]
    [Ecopedia("Crafted Objects", "Vehicles", false, true, "HoverBike Item")]
    public class HoverBikeRecipe : RecipeFamily, IConfigurableRecipe
    {
        private static RecipeDefaultModel defaults
        {
            get
            {
                RecipeDefaultModel recipeDefaultModel = new RecipeDefaultModel();
                recipeDefaultModel.ModelType = typeof(HoverBikeRecipe)!.Name;
                recipeDefaultModel.Assembly = typeof(HoverBikeRecipe)!.AssemblyQualifiedName;
                recipeDefaultModel.HiddenName = "Hover Bike Recipe";
                recipeDefaultModel.LocalizableName = Localizer.DoStr("Hover Bike Recipe");
                recipeDefaultModel.IngredientList = new List<EMIngredient>
                {
                    new EMIngredient(typeof(SteamTruckItem)!.Name, isTag: false, 1f, isStatic: true)
                };
                recipeDefaultModel.ProductList = new List<EMCraftable>
                {
                    new EMCraftable(typeof(HoverBikeItem)!.Name)
                };
                recipeDefaultModel.BaseExperienceOnCraft = 0f;
                recipeDefaultModel.BaseLabor = 100f;
                recipeDefaultModel.LaborIsStatic = false;
                recipeDefaultModel.BaseCraftTime = 10f;
                recipeDefaultModel.CraftTimeIsStatic = false;
                recipeDefaultModel.CraftingStation = typeof(AssemblyLineItem)!.Name;
                recipeDefaultModel.RequiredSkillType = typeof(MechanicsSkill);
                recipeDefaultModel.RequiredSkillLevel = 2;
                recipeDefaultModel.IngredientImprovementTalents = typeof(MechanicsLavishResourcesTalent);
                recipeDefaultModel.SpeedImprovementTalents = new Type[2]
                {
                    typeof(MechanicsParallelSpeedTalent),
                    typeof(MechanicsFocusedSpeedTalent)
                };
                return recipeDefaultModel;
            }
        }

        static HoverBikeRecipe()
        {
            EMRecipeResolver.AddDefaults(defaults);
        }

        public HoverBikeRecipe()
        {
            base.Recipes = AutoSingleton<EMRecipeResolver>.Obj.ResolveRecipe(this);
            base.LaborInCalories = AutoSingleton<EMRecipeResolver>.Obj.ResolveLabor(this);
            base.CraftMinutes = AutoSingleton<EMRecipeResolver>.Obj.ResolveCraftMinutes(this);
            ExperienceOnCraft = AutoSingleton<EMRecipeResolver>.Obj.ResolveExperience(this);
            Initialize(defaults.LocalizableName, GetType());
            CraftingComponent.AddRecipe(AutoSingleton<EMRecipeResolver>.Obj.ResolveStation(this), this);
        }
    }