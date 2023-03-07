using Eco.Core.Plugins;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Core.Utils.Threading;
using Eco.RM.Core.Configs;
using Eco.Shared.Localization;
using Eco.RM.Core.Managers;
using Eco.Shared.Utils;

namespace Eco.RM.Core.Plugins
{
    [LocDisplayName(nameof(DecayPlugin))]
    public class DecayPlugin : Singleton<DecayPlugin>, IModKitPlugin, IThreadedPlugin, IInitializablePlugin, IShutdownablePlugin, IConfigurablePlugin
    {
        private readonly PluginConfig<DecayConfig> config;
        public IPluginConfig PluginConfig => this.config;
        public DecayConfig Config => this.config.Config;
        public ThreadSafeAction<object, string> ParamChanged { get; set; } = new ThreadSafeAction<object, string>();
        public object GetEditObject() => this.config.Config;
        public void OnEditObjectChanged(object o, string param) => this.SaveConfig();

        private readonly EventDrivenWorker tickWorker;

        public void Initialize(TimedTask timer)
        {
            config.SaveAsync();
            var decayManager = new DecayManager();
            decayManager.Initialize();
        }

        public DecayPlugin()
        {
            this.tickWorker = new EventDrivenWorker(DecayManager.Reset, this.Tick);
            this.config = new PluginConfig<DecayConfig>("RM-Decay");
        }
        public string GetCategory() => Localizer.DoStr("Raynbo Mods");
        public string GetStatus() => DecayManager.Obj.GetStatus();
        public override string ToString() => Localizer.DoStr("RM Decay");
        public void Run() => this.tickWorker.Start(ThreadPriorityTaskFactory.BelowNormal);
        public Task ShutdownAsync() => this.tickWorker.ShutdownAsync();
        public int Tick() => DecayManager.Obj.Tick();
    }
}
