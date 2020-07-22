﻿using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Logic.Structure;

namespace CRepublic.Magic.Extensions
{
    internal class Constants
    {
        internal const int ID = 0;
        internal const int MaxCommand  = 0;
        internal const int Buffer = 4096;
        internal const int PRE_ALLOC_SEA = 128;
        internal const bool Local = false;
        internal const bool PacketCompression = true;
        internal const bool UseSentry = false;
        internal const bool RC4 = true;
        //internal const DBMS Database = DBMS.Mysql;

        internal static int MaintenanceDuration = 0;
        internal static Maintenance_Timer Maintenance = null;
        internal static readonly string PatchServer = Utils.ParseConfigString("PatchUrl");
        internal static readonly string UpdateServer = Utils.ParseConfigString("UpdateUrl");
        internal static readonly string[] ClientVersion = Utils.ParseConfigString("ClientVersion").Split('.');

      /*  internal static string[] AuthorizedIP =
        {
            "192.168.0.5",
            "192.168.0.144",
            "115.133.41.158"
        };*/

    }
}
