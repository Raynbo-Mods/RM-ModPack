/****************************************************************************\
| *  THIS CODE IS BASED OFF OF SpoilageManager.cs IN Eco.Gameplay.Garbage   *|
| *  CREDIT: Strange Loop Games with minor edits by RainbowIris323          *|
\****************************************************************************/

using Eco.Core.Controller;
using Eco.Gameplay.Items;
using Eco.Shared.Localization;
using Eco.Shared.Utils;
using Eco.Gameplay.Systems;
using Eco.Shared.IoC;
using Eco.Gameplay.Items.InventoryRelated;
using System.Collections.Concurrent;
using Eco.Gameplay.Garbage;
using Eco.RM.Core.Components;
using Eco.RM.Core.Items;

namespace Eco.RM.Core.Managers
{
    public class DecayManager : Singleton<DecayManager>
    {
        public const string ScrapName = "ScrapItem";
        public static AutoResetEvent Reset = new AutoResetEvent(true);
        public static BasicSimulation Simulation { get; private set; } = new BasicSimulation(Localizer.DoStr("Decay"));
        public static Type DecayingItemType = typeof(DecayingItem);
        readonly ConcurrentDictionary<ItemStack, ItemStackDecayTicker> stackTickersMap = new ConcurrentDictionary<ItemStack, ItemStackDecayTicker>();
        readonly HashSet<ItemStack> stacksToDecay = new HashSet<ItemStack>();

        Item scrapItem;

        public void Initialize()
        {
            SleepManager.Obj.Subscribe(nameof(SleepManager.AcceleratingTime), this.FireTick);
            foreach (var inventory in ServiceHolder<IInventoryManager>.Obj.AllLeafInventories) this.CheckInventoryForDecay(inventory);
            Inventory.StacksChanged.Add(this.CheckStacksForDecayThenReset);
            Inventory.InventoryEffectsChanged.Add(this.CheckInventoryForDecayThenReset);
            Inventory.InventoryDestroyed.Add(this.OnInventoryDestroyed);
            this.scrapItem = Item.Get(ScrapName);
            if (this.scrapItem == null)
            {
                Log.WriteErrorLineLoc($"{ScrapName} isn't found make sure it exists, this is needed for Decay Plugin");
                throw new Exception($"{ScrapName} isn't found make sure it exists.");
            }
        }

        public int Tick()
        {
            lock (this.stacksToDecay)
            {
                Simulation.TickAll();
                this.DecayStacks();
                return (int)Simulation.NextTickTimeMs;
            }
        }

        void DecayStacks()
        {
            if (this.stacksToDecay.Count == 0)
                return;

            var inventories = this.stacksToDecay.Select(i => i.Parent).Distinct();
            using (var changeSet = InventoryChangeSet.New(inventories))
            {
                foreach (var stack in this.stacksToDecay)
                {
                    DecayingItem item = (DecayingItem)stack.Item;
                    Console.WriteLine("scrap check");
                    if (!item.ScrapOnDeath) return;
                    Console.WriteLine("scraping");
                    int quantity = stack.Quantity * item.ScrapOutput;
                    changeSet.ClearStack(stack);
                    changeSet.AddItems(this.scrapItem.Type, quantity, stack.Parent);
                }
                var res = changeSet.TryApply();
                DebugUtils.Assert(res.Success, AssertionFlags.WarnOnly, Localizer.DoStr("Failed to decay some items"));
            }

            this.stacksToDecay.Clear();
        }

        public void TickItemStack(ItemStack stack)
        {
            if (stack == null || stack.Item == null || !stack.Item.Type.IsSubclassOf(DecayManager.DecayingItemType)) this.RemoveTicker(stack);
            else if (this.ShouldItemDecay(stack.Item))
            {
                this.stacksToDecay.Add(stack);
                this.RemoveTicker(stack);
            }
        }

        public void OnCreate() { }
        public string GetStatus() => Simulation.GetStatus();
        public string GetDisplayText() => string.Empty;
        public override string ToString() => Localizer.DoStr("Decay");
        public void OnLoaded() { }
        void ResetOnSmallestTickChange(Action method)
        {
            var cachedNextTickTime = Simulation.GetNextSmallestTickSeconds();
            method?.Invoke();
            if (cachedNextTickTime != Simulation.GetNextSmallestTickSeconds()) this.FireTick();
        }

        void LifespanMultiplierUpdateThenReset() => this.ResetOnSmallestTickChange(() =>
        {
            foreach (var ticker in this.stackTickersMap.Values)
                this.UpdateNextTickAndQueueElementIfNeeded(ticker);
        });
        void CheckInventoryForDecayThenReset(Inventory inventory) => this.ResetOnSmallestTickChange(() =>
        {
            this.CheckInventoryForDecay(inventory);
        });

        void CheckStacksForDecayThenReset(IEnumerable<ItemStack> stacks)
        {
            foreach (var stack in stacks)
                this.CheckStackForDecay(stack);
        }

        void CheckInventoryForDecay(Inventory inventory)
        {
            if (!inventory.IsLeafInventory) return;
            foreach (var stack in inventory.ManipulatableStacks)
            {
                this.CheckStackForDecay(stack);
            }
        }

        void CheckStackForDecay(ItemStack stack)
        {
            if (this.stackTickersMap.TryGetValue(stack, out var itemStackDecayTicker)) this.UpdateNextTickAndQueueElementIfNeeded(itemStackDecayTicker);
            else this.CreateTickerForItemStackAndAddItIfItemIsDecaying(stack);
        }

        void OnInventoryDestroyed(Inventory inventory)
        {
            if (!inventory.IsLeafInventory) return;
            foreach (var stack in inventory.ManipulatableStacks) this.RemoveTicker(stack);
        }

        void CreateTickerForItemStackAndAddItIfItemIsDecaying(ItemStack stack)
        {
            if (stack.Item is not DecayingItem decayingItem) return;
            decayingItem.UpdateDecayTime();
            var ticker = new ItemStackDecayTicker(stack);
            ticker.NextTick = decayingItem.DecayTime.ExpirationTime();
            if (this.stackTickersMap.TryAdd(stack, ticker)) this.ResetOnSmallestTickChange(() => Simulation.AddTickable(ticker));
        }

        void FireTick() => Reset.Set();

        bool ShouldItemDecay(Item item) => item switch
        {
            DecayingItem decayingItem => decayingItem.DecayTime.TimeLeft() <= 0,
            _ => false
        };

        void RemoveTicker(ItemStack stack)
        {
            if (this.stackTickersMap.TryRemove(stack, out var ticker))
            {
                Simulation.RemoveTickable(ticker);
                ticker.Destroy();
            }
        }

        void UpdateNextTickAndQueueElementIfNeeded(ItemStackDecayTicker stackTicker)
        {
            if (stackTicker.TargetStack.Item is DecayingItem decayingItem)
            {
                decayingItem.UpdateDecayTime();
                if (stackTicker.NextTick == decayingItem.DecayTime.ExpirationTime()) return;
                stackTicker.NextTick = decayingItem.DecayTime.ExpirationTime();
            }
            else this.RemoveTicker(stackTicker.TargetStack);
        }
    }
}
