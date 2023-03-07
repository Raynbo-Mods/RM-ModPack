/**************************************************************************************\
| *  THIS CODE IS BASED OFF OF ItemStackSpoilageTicker.cs IN Eco.Gameplay.Garbage     *|
| *  CREDIT: Strange Loop Games with minor edits by RainbowIris323                    *|
\**************************************************************************************/

using C5;
using Eco.Gameplay.Items;
using Eco.RM.Core.Plugins;
using Eco.Simulation.Agents;
using Eco.Simulation.Time;

namespace Eco.RM.Core.Managers
{
    public class ItemStackDecayTicker : ITickable
    {
        public ItemStack TargetStack { get; set; }
        public ItemStackDecayTicker(ItemStack stack) => this.TargetStack = stack;
        public double NextTick { get; set; } = double.MaxValue;
        public IPriorityQueueHandle<ITickable> QueueHandle { get; set; }
        public void Destroy() { }
        public bool IsReady() => this.NextTick <= WorldTime.Seconds;
        public int CompareTo(object obj) => this.NextTick.CompareTo(((ItemStackDecayTicker)obj).NextTick);

        public void Tick()
        {
            if (!DecayPlugin.Obj.Config.ItemDecayEnabled) return;
            DecayManager.Obj?.TickItemStack(this.TargetStack);
        }
    }
}
