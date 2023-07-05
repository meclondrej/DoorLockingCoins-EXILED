using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
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
                    Log.Debug("Door interaction with no coin in hand triggered");
                    return;
                }
                bool perms = false;
                if ((door.IsKeycardDoor || door.IsGate || door.IsCheckpoint) && config.checkInventoryForKeycards)
                {
                    Inventory inv = player.Inventory;
                    for (int i = 0; i < inv.UserInventory.Items.Count; i++)
                    {
                        Item x = Item.Get(inv.UserInventory.Items.Values.ElementAt(i));
                        if (x.Category != ItemCategory.Keycard)
                            continue;
                        Keycard keycard = (Keycard) x;
                        bool hasAll = true;
                        foreach (Interactables.Interobjects.DoorUtils.KeycardPermissions p in Enum.GetValues(typeof(Interactables.Interobjects.DoorUtils.KeycardPermissions)))
                        {
                            if (door.RequiredPermissions.RequiredPermissions.HasFlag(p))
                            {
                                if (!keycard.Base.Permissions.HasFlag(p))
                                {
                                    hasAll = false;
                                    break;
                                }
                            }
                        }
                        if (hasAll)
                        {
                            perms = true;
                            break;
                        }
                    }
                }
                Item item = player.CurrentItem;
                if (
                    (
                       e.IsAllowed
                       || perms
                    )
                    && !door.IsLocked
                    && door.IsFullyOpen
                    && !door.IsBroken
                    && !door.IsElevator
                )
                {
                    if (!e.IsAllowed)
                        e.IsAllowed = true;
                    door.Lock(config.lockSeconds, DoorLockType.NoPower);
                    player.RemoveItem(item);
                    Log.Debug("Door interaction with coin triggered and successful");
                }
                else
                    Log.Debug("Door interaction with coin triggered and unsuccessful");
            } catch (Exception ex)
            {
                Log.Debug($"An exception occured: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}
