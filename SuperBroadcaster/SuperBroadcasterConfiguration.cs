using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExtraConcentratedJuice.SuperBroadcaster
{
    public class SuperBroadcasterConfiguration : IRocketPluginConfiguration
    {
        public int defaultBroadcastDuration;
        public int broadcastTimeLimit;
        public ushort effectId;
        public float repeatingBroadcastInterval;
        public float repeatingBroadcastStayTime;

        [XmlArrayItem(ElementName = "message")]
        public List<string> broadcastMessages;

        public void LoadDefaults()
        {
            effectId = 42069;
            defaultBroadcastDuration = 5;
            broadcastTimeLimit = 25;
            repeatingBroadcastInterval = 300;
            repeatingBroadcastStayTime = 6;
            broadcastMessages = new List<string> { "SupahBroadcasto!!!!", "This is a SUPER broadcast!", "set repeatingBroadcastInterval to -1 to disable this!"};
        }
    }
}
