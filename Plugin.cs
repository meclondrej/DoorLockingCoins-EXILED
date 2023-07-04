using Exiled;
using Exiled.API.Features;
using System;
using Player = Exiled.Events.Handlers.Player;

namespace DoorLockingCoins_EXILED
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance;
        public Handler EventHandler;
        public override string Name => "DoorLockingCoins-EXILED";
        public override string Prefix => "doorlockingcoinsexiled";
        public override string Author => "meclondrej";
        public override Version Version => new Version(1, 0, 0);
        public Plugin()
        {
            Instance = this;
        }
        public override void OnEnabled()
        {
            EventHandler = new Handler(Config);
            Player.InteractingDoor += EventHandler.OnInteractingDoor;
            Log.Debug("Registered events");
        }
        public override void OnDisabled()
        {
            Player.InteractingDoor -= EventHandler.OnInteractingDoor;
            EventHandler = null;
            Log.Debug("Unregistered events");
        }
    }
}
