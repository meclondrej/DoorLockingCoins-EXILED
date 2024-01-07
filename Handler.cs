using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using Exiled.API.Features.Doors;
using InventorySystem;
using System.Linq;
using System;

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
            try
            {
                Log.Debug("Door interaction registered");
                Door door = e.Door;
                Player player = e.Player;
                if (player.CurrentItem == null || player.CurrentItem.Type != ItemType.Coin)
                {
                    Log.Debug("Interaction with no coin in hand triggered");
                    return;
                }
                if (!(
                    !(!config.affectGates && door.IsGate)
                    && !door.IsElevator
                    && !door.IsPartOfCheckpoint
                    && !door.IsLocked
                    && !(door is BreakableDoor breakableDoor && breakableDoor.IsDestroyed)
                    && door.IsFullyOpen
                ))
                {
                    Log.Debug("Interaction on incompatible door triggered");
                    return;
                }
                bool perms = e.IsAllowed;
                if (config.checkInventoryForKeycards && !perms)
                {
                    Inventory inv = player.Inventory;
                    for (int i = 0; i < inv.UserInventory.Items.Count; i++)
                    {
                        Item x = Item.Get(inv.UserInventory.Items.Values.ElementAt(i));
                        if (x.Category != ItemCategory.Keycard)
                            continue;
                        Keycard keycard = (Keycard)x;
                        bool hasAll = true;
                        foreach (Interactables.Interobjects.DoorUtils.KeycardPermissions p in Enum.GetValues(typeof(Interactables.Interobjects.DoorUtils.KeycardPermissions)))
                        {
                            if (door.RequiredPermissions.RequiredPermissions.HasFlag(p) && !keycard.Base.Permissions.HasFlag(p))
                            {
                                hasAll = false;
                                break;
                            }
                        }
                        if (hasAll)
                        {
                            perms = true;
                            break;
                        }
                    }
                }
                if (!perms)
                {
                    Log.Debug("Door interaction with invalid permissions triggered");
                    return;
                }
                if (!e.IsAllowed)
                    e.IsAllowed = true;
                door.Lock(config.lockSeconds, DoorLockType.NoPower);
                player.RemoveItem(player.CurrentItem);
                Log.Debug("Door interaction successful");
            }
            catch (Exception ex)
            {
                Log.Debug($"An exception occured: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}
