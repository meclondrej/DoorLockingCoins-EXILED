using Exiled.API.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorLockingCoins_EXILED
{
    public class Config : IConfig
    {
        [Description("Whether or not this plugin is enabled.")]
        public bool IsEnabled { get; set; } = true;
        [Description("Whether or not debug messages should be shown in the console.")]
        public bool Debug { get; set; } = false;
        public int lockSeconds { get; set; } = 60;
    }
}
