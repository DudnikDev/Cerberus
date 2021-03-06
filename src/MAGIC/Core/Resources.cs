using System;
using System.Threading;
using CRepublic.Magic.Core.API;
using CRepublic.Magic.Core.Events;
using CRepublic.Magic.Core.Networking;

namespace CRepublic.Magic.Core
{
    internal class Resources
    {
        internal static Clans Clans;
        internal static Battles Battles;
        internal static Battles_V2 Battles_V2;
        internal static Random Random;
        internal static Classes Classes;
        internal static Global_Chat GChat;
        internal static Region Region;
        internal static Player_Region PRegion;
        internal static Parser Parser;
        internal static WebApi Api;

        internal static void Initialize()
        {
            Exceptions.Initialize();
            Devices.Initialize();
            Players.Initialize();
            Resources.Clans = new Clans();
            Resources.GChat = new Global_Chat();
            Resources.Battles = new Battles();
            Resources.Battles_V2 = new Battles_V2();
            Resources.Classes = new Classes();
            Resources.Random = new Random(DateTime.Now.ToString().GetHashCode());
            Resources.Region = new Region();
            Resources.PRegion = new Player_Region();
            Gateway.Initialize();
            Gateway.Listen();
            Resources.Api = new WebApi();
            Resources.Parser = new Parser();
        }
    }
}
