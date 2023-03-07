/************************************************************************\
| *  THIS CODE IS BASED OFF OF FoodDurabilty.cs IN Eco.Gameplay.Items   *|
| *  CREDIT: Strange Loop Games with minor edits by RainbowIris323      *|
\************************************************************************/

using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;
using Eco.Core.Controller;
using Eco.Gameplay.Items;
using Eco.RM.Core.Plugins;

namespace Eco.RM.Core.Items
{
    public abstract partial class DecayingItem : DurabilityItem, IStackableMergable
    {
        ImmutableCountdown decayTime;
        [Serialized, SyncToView]
        public ImmutableCountdown DecayTime
        {
            get => this.decayTime;
            set => this.decayTime = value;
        }

        protected abstract int BaseLifespan { get; }
        public abstract bool ScrapOnDeath { get; }
        public abstract int ScrapOutput { get; }

        public override LocString TooltipMaximumDurability() { return LocString.Empty; }
        public override float MinDefaultDurability => 50;

        float AdjustedLifespan => this.BaseLifespan * DecayPlugin.Obj.Config.ItemDecayMultiplyer;

        bool HasInfiniteLifespan => !DecayPlugin.Obj.Config.ItemDecayEnabled;

        public int StackableQualityGroup() => (int)(this.GetDurability() / 34);
        public bool CanStack(Item item) => item.MaxStackSize != 1;

        public Item Merge(Item another, int first, int second)
        {
            if (second <= 0 || another == null) return this;
            if (another.Type != this.Type) throw new Exception("Trying to merge different kinds of food, this shouldn't be possible");

            var decayingItem = (DecayingItem)this.Clone;
            var firstDurability = this.GetDurability() * first;
            var secondDurability = ((DecayingItem)another).GetDurability() * second;
            var averageDurability = (firstDurability + secondDurability) / (first + second);
            decayingItem.DecayTime = this.GetDecayTimeBasedOnDurability(averageDurability);
            return decayingItem;
        }

        public override Item Clone
        {
            get
            {
                var copy = (DecayingItem)base.Clone;
                copy.decayTime = this.decayTime;
                return copy;
            }
        }

        public void SetDecayTimeBasedOnDurability(float durability) => this.DecayTime = this.GetDecayTimeBasedOnDurability(durability);

        public override float GetDurability()
        {
            if (this.DecayTime.Duration() == 0) this.DecayTime = this.GetDecayTimeBasedOnDurability(DurabilityMax);                       
            return this.GetDurabilityBasedOnDecayTime(this.DecayTime);
        }

        public ImmutableCountdown GetDecayTimeBasedOnDurability(float durability, bool paused = false)
        {
            var duration = this.HasInfiniteLifespan ? this.BaseLifespan : this.AdjustedLifespan;
            var timeLeft = duration * (durability / 100);
            return ImmutableCountdown.Create(duration, timeLeft, paused || this.HasInfiniteLifespan);
        }

        public void UpdateDecayTime()
        {
            float cachedDurability = this.GetDurability();
            if (this.DecayTime.Duration() != this.AdjustedLifespan) cachedDurability = this.GetDurabilityBasedOnDecayTime(this.DecayTime);
            this.DecayTime = this.GetDecayTimeBasedOnDurability(cachedDurability);
        }

        float GetDurabilityBasedOnDecayTime(ImmutableCountdown decayTime) => decayTime.PercentLeft() * 100f;
    }
}
