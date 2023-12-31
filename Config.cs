﻿using Exiled.API.Interfaces;
using System.ComponentModel;

namespace DoorLockingCoins_EXILED
{
    public class Config : IConfig
    {
        [Description("Whether or not this plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;
        [Description("Whether or not debug messages should be shown in the console.")]
        public bool Debug { get; set; } = false;
        [Description("Amount of time a door should stay locked in seconds.")]
        public int lockSeconds { get; set; } = 60;
        [Description("Whether or not should the plugin work on keycard doors. Disable if you installed RemoteKeycard.")]
        public bool checkInventoryForKeycards { get; set; } = true;
        [Description("Whether or not coins should lock gates.")]
        public bool affectGates { get; set; } = true;
    }
}
