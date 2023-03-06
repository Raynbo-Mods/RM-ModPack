using Eco.Core.Plugins;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Shared.Localization;
using Eco.Shared.Utils;
using Eco.RM.Core.Configs;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Components;

namespace Eco.RM.Core.Plugins
{
    [LocDisplayName("RM Decay Plugin")]
    public class DecayPlugin : Singleton<DecayPlugin>, IInitializablePlugin, IModKitPlugin, IConfigurablePlugin
    {

        private readonly PluginConfig<DecayConfig> config;
        public DecayPlugin()
        {
            this.config = new PluginConfig<DecayConfig>("RM-Decay");
        }

        public IPluginConfig PluginConfig => this.config;
        public DecayConfig Config => this.config.Config;
        public ThreadSafeAction<object, string> ParamChanged { get; set; } = new ThreadSafeAction<object, string>();

        public object GetEditObject() => this.config.Config;
        public void OnEditObjectChanged(object o, string param) => this.SaveConfig();
        public string GetStatus() => "Settings Loaded";

        public void Initialize(TimedTask timer) { }
        public override string ToString() => Localizer.DoStr("RM Settings");

        public string GetCategory() => "Raynbo Mods";

        public void Tick()
        {
            if (Config.BatteryItemDecayEnabled) TickBatteryItems();
            if (Config.BatteryObjectDecayEnabled) TickBatteryObjects();
        }

        private void TickBatteryItems()
        {
            WorldObjectManager.ForEach(x =>
            {
                x.GetComponent<StorageComponent>()?.Inventory.NonEmptyStacks.Where(x => x.TagNames().Contains("Batteries"));
            });
        }

        private void TickBatteryObjects()
        {

        }
    }
}
