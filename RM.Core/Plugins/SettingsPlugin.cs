using Eco.Core.Plugins;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Shared.Localization;
using Eco.Shared.Utils;
using Eco.RM.Core.Configs;

namespace Eco.RM.Core.Plugins
{
    [LocDisplayName("RM Settings Plugin")]
    public class SettingsPlugin : Singleton<SettingsPlugin>, IInitializablePlugin, IModKitPlugin, IConfigurablePlugin
    {
        public Timer Timer;

        private readonly PluginConfig<SettingsConfig> config;
        public SettingsPlugin()
        {
            this.config = new PluginConfig<SettingsConfig>("RM-Settings");
        }

        public IPluginConfig PluginConfig => this.config;
        public SettingsConfig Config => this.config.Config;
        public ThreadSafeAction<object, string> ParamChanged { get; set; } = new ThreadSafeAction<object, string>();

        public object GetEditObject() => this.config.Config;
        public void OnEditObjectChanged(object o, string param) => this.SaveConfig();
        public string GetStatus() => "Settings Loaded";

        public void Initialize(TimedTask timer)
        {
            config.SaveAsync();
        }
        public override string ToString()
        {
            return Localizer.DoStr("RM Settings");
        }

        public string GetCategory() => "Raynbo Mods";
    }
}
