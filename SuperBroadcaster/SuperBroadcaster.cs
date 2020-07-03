using Rocket.Core.Plugins;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using Rocket.API.Collections;

namespace ExtraConcentratedJuice.SuperBroadcaster
{
    public class SuperBroadcaster : RocketPlugin<SuperBroadcasterConfiguration>
    {
        public static SuperBroadcaster instance;
        public bool isActive;
        private int m_broadcastIndex;

        protected override void Load()
        {
            instance = this;
            isActive = false;
            m_broadcastIndex = 0;

            if (Configuration.Instance.repeatingBroadcastInterval > 0)
                StartCoroutine(BroadcastCoroutine(Configuration.Instance.repeatingBroadcastInterval));
        }

        protected override void Unload()
        {
            StopAllCoroutines();

            foreach (var player in Provider.clients.Where(x => x != null))
                EffectManager.askEffectClearByID(Configuration.Instance.effectId, player.playerID.steamID);

            isActive = false;
        }

        private IEnumerator BroadcastCoroutine(float time)
        {
            while (true)
            {
                yield return new WaitForSeconds(time);

                if (isActive)
                    yield return new WaitForSeconds(time);

                string message = Configuration.Instance.broadcastMessages[m_broadcastIndex];

                StartBroadcast(Configuration.Instance.repeatingBroadcastStayTime, message);

                if (++m_broadcastIndex >= Configuration.Instance.broadcastMessages.Count)
                    m_broadcastIndex = 0;
            }
        }

        private IEnumerator ClearEffectCoroutine(float time)
        {
            yield return new WaitForSeconds(time);

            foreach (SteamPlayer player in Provider.clients.Where(x => x != null))
                EffectManager.askEffectClearByID(Configuration.Instance.effectId, player.playerID.steamID);

            isActive = false;
        }

        public void StartBroadcast(float time, string message)
        {
            foreach (var player in Provider.clients.Where(x => x != null))
                EffectManager.sendUIEffect(Configuration.Instance.effectId, 4205, player.playerID.steamID, true, message);

            isActive = true;

            StartCoroutine(ClearEffectCoroutine(time));
        }

        public override TranslationList DefaultTranslations =>
            new TranslationList
            {
                { "is_active", "There is an active broadcast. Please wait for it to end before creating another one." },
                { "broadcast_sent", "You have successfully sent your broadcast!" },
                { "too_long", "The duration that you specified on your broadcast is above this server's max broadcast time limit." },
                { "success", "You have successfully created a superbroadcast." }
            };
    }
}
