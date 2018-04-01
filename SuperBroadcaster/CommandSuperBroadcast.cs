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
            if (args.Length < 1 || args.Length > 2)
            {
                UnturnedChat.Say(caller, Syntax, Color.red);
                return;
            }

            if (SuperBroadcaster.instance.isActive)
            {
                UnturnedChat.Say(caller, SuperBroadcaster.instance.Translate("is_active"), Color.red);
                return;
            }

            if (args.Length == 1)
            {
                SuperBroadcaster.instance.StartBroadcast(SuperBroadcaster.instance.Configuration.Instance.defaultBroadcastDuration, args[0]);
            }

            if (args.Length == 2)
            {
                if (!float.TryParse(args[1], out float time))
                {
                    UnturnedChat.Say(caller, Syntax, Color.red);
                    return;
                }
                float limit = SuperBroadcaster.instance.Configuration.Instance.broadcastTimeLimit;

                if (limit > 0 && time > limit)
                {
                    UnturnedChat.Say(caller, SuperBroadcaster.instance.Translate("too_long"), Color.red);
                    return;
                }

                UnturnedChat.Say(caller, SuperBroadcaster.instance.Translate("success"));
                SuperBroadcaster.instance.StartBroadcast(time, args[0]);
            }
        }
    }
}