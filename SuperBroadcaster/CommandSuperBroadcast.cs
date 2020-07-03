using System.Collections.Generic;
using Rocket.API;
using UnityEngine;
using SDG.Unturned;
using Logger = Rocket.Core.Logging.Logger;

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
                TellUser(caller, Syntax, Color.red);
                return;
            }

            if (SuperBroadcaster.instance.isActive)
            {
                TellUser(caller, SuperBroadcaster.instance.Translate("is_active"), Color.red);
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
                        TellUser(caller, Syntax, Color.red);
                        return;
                    }
                    float limit = SuperBroadcaster.instance.Configuration.Instance.broadcastTimeLimit;

                    if (limit > 0 && time > limit)
                    {
                        TellUser(caller, SuperBroadcaster.instance.Translate("too_long"), Color.red);
                        return;
                    }

                    TellUser(caller, SuperBroadcaster.instance.Translate("success"), Palette.SERVER);
                    SuperBroadcaster.instance.StartBroadcast(time, args[0]);
                    break;
                }
            }
        }

        private void TellUser(IRocketPlayer caller, string message, Color color)
        {
            if (caller.Id == "Console")
            {
                Logger.Log(message);
                return;
            }
            
            SteamPlayer steamPlayer = PlayerTool.getSteamPlayer(ulong.Parse(caller.Id));
            ChatManager.serverSendMessage(message, color, toPlayer: steamPlayer);
        }
    }
}