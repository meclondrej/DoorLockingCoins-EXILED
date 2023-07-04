using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;

namespace DoorLockingCoins_EXILED
{
    public class Handler
    {
        private readonly Config config;
        public Handler(Config config)
        {
            this.config = config;
        }
        public void OnInteractingDoor(InteractingDoorEventArgs e)
        {
            Log.Debug("Door interaction registered");
            Door door = e.Door;
            Player player = e.Player;
            if (player.CurrentItem == null || player.CurrentItem.Type != ItemType.Coin) {
                Log.Debug("Door interaction with no coin in hand triggered");
                return;
            }
            Item item = player.CurrentItem;
            if (
                e.IsAllowed
                && !door.IsLocked
                && door.IsFullyOpen
                && !door.IsBroken
                && !door.IsElevator
            )
            {
                door.Lock(config.lockSeconds, DoorLockType.NoPower);
                player.RemoveItem(item);
                Log.Debug("Door interaction with coin triggered and successful");
            }
            else
                Log.Debug("Door interaction with coin triggered and unsuccessful");
        }
    }
}
