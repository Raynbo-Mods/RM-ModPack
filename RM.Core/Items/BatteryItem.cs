﻿using Eco.Core.Items;
using Eco.Core.Utils;
using Eco.Gameplay.Items;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using System.ComponentModel;

namespace Eco.RM.Core.Items
{
    /// <summary>Base battery item used to make new battery types</summary>
    [Serialized, LocDisplayName("BaseBatteryItem"), Category("Hidden"), Tag("Batteries")]
    public partial class BatteryItem : Item
    {
        public bool OverrideThis { get; set; }
        /// <summary>The max the battery can hold in watt hours</summary>
        public int MaxCharge { get; set; }
        /// <summary>The max power input in watts the battery can take in</summary>
        public int MaxChargeRate { get; set; }
        /// <summary>The batterys max output in watts</summary>
        public int MaxDischargeRate { get; set; }
        [Serialized] private float currentCharge = 0;
        /// <summary>The current charge in watt hours the batter is holding</summary>
        public float CurrentCharge
        {
            get => this.currentCharge;
            set
            {
                if (value == this.currentCharge) return;
                this.currentCharge = value;
                this.ChargeChangedEvent.Invoke(this);
            }
        }
        /// <summary>Event called whenever the battery's charge is changed. used for updating the battery's tooltip</summary>
        public ThreadSafeAction<BatteryItem> ChargeChangedEvent = new ThreadSafeAction<BatteryItem>();
        public BatteryItem() { }
    }
}