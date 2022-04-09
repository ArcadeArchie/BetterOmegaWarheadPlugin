﻿using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using System;
using System.Linq;
using UnityEngine;

namespace OmegaWarheadPlugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    public class Activate : ICommand
    {
        public string Command { get; } = "activateomega";

        public string[] Aliases { get; } = null;

        public string Description { get; } = "Activates the Omega Warhead.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(sender.CheckPermission(PlayerPermissions.WarheadEvents))
            {
                foreach (Room room in Room.List)
                    room.Color = Color.cyan;
                Cassie.Message(Plugin.Instance.Config.Cassie, false, false);
                Map.Broadcast(10, Plugin.Instance.Config.ActivatedMessage);
                foreach (Room room in Room.List)
                    room.Color = Color.cyan;
                Timing.CallDelayed(180, () =>
                {
                    foreach (Player Sobrevivientes in Player.List.Where(plr => plr.CurrentRoom.Name == "EZ_Shelter"))
                    {
                        Sobrevivientes.IsGodModeEnabled = true;
                        Timing.CallDelayed(1f, () =>
                        {
                            Sobrevivientes.IsGodModeEnabled = false;
                            Sobrevivientes.EnableEffect(EffectType.Flashed, 2);
                            Sobrevivientes.Position = new Vector3(-53, 988, -50);
                            Sobrevivientes.EnableEffect(EffectType.Visuals939, 5);
                        });
                        Timing.CallDelayed(1.5f, () =>
                        {
                            Timing.CallDelayed(0.2f, Warhead.Detonate);
                        });
                    }
                    foreach (Player Muertos in Player.List)
                        if (Muertos.CurrentRoom.Type != RoomType.EzShelter)
                        {
                            Muertos.Kill("Omega Warhead");
                        }
                    foreach (Room room in Room.List)
                        room.Color = Color.blue;
                });
                Timing.CallDelayed(155, () =>
                {
                    //Map.Broadcast(10, "<color=blue> RESCUE HELICOPTER COMING TO SURFACE </color>");
                    //RespawnEffectsController.ExecuteAllEffects(RespawnEffectsController.EffectType.Selection, SpawnableTeamType.NineTailedFox);
                });
                response = string.Empty;
                return false;
            }
            else
            {
                response = "You need Warhead Events permissions to use this commands";
                return true;
            }
        }
    }
}
