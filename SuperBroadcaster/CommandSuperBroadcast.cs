using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using Steamworks;
using UnityEngine;
using SDG.Unturned;
using System;

namespace ExtraConcentratedJuice.SuperBroadcaster
{
    public class CommandSuperBroadcast : IRocketCommand
    {
        #region Properties

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "superbroadcast";

        public string Help => "Broadcasts via a UI effect.";

        public string Syntax => "/superbroadcast \"your text here\" <time>";

        public List<string> Aliases => new List<string> { "sbroadcast", "sb" };

        public List<string> Permissions => new List<string> { "superbroadcast.broadcast" };

        #endregion

        public void Execute(IRocketPlayer caller, string[] args)
        {
            SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(ulong.Parse(caller.Id));
            if (args.Length < 1 || args.Length > 2)
            {
                ChatManager.serverSendMessage(Syntax, Color.red, toPlayer: steamPlayer);
                return;
            }

            if (SuperBroadcaster.instance.isActive)
            {
                ChatManager.serverSendMessage(SuperBroadcaster.instance.Translate("is_active"), Color.red, toPlayer: steamPlayer);
                return;
            }

            switch (args.Length)
            {
                case 1:
                    SuperBroadcaster.instance.StartBroadcast(SuperBroadcaster.instance.Configuration.Instance.defaultBroadcastDuration, args[0]);
                    break;
                case 2:
                {
                    if (!float.TryParse(args[1], out float time))
                    {
                        ChatManager.serverSendMessage(Syntax, Color.red, toPlayer: steamPlayer);
                        return;
                    }
                    float limit = SuperBroadcaster.instance.Configuration.Instance.broadcastTimeLimit;

                    if (limit > 0 && time > limit)
                    {
                        ChatManager.serverSendMessage(SuperBroadcaster.instance.Translate("too_long"), Color.red, toPlayer: steamPlayer);
                        return;
                    }

                    ChatManager.serverSendMessage(SuperBroadcaster.instance.Translate("success"), Palette.SERVER, toPlayer: steamPlayer);
                    SuperBroadcaster.instance.StartBroadcast(time, args[0]);
                    break;
                }
            }
        }
    }
}