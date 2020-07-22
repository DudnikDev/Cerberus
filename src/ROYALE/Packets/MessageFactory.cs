using System;
using System.Collections.Generic;
using CRepublic.Royale.Packets.Messages.Client;
using CRepublic.Royale.Packets.Messages.Client.Alliance;
using CRepublic.Royale.Packets.Messages.Client.API;
using CRepublic.Royale.Packets.Messages.Client.Authentication;
using CRepublic.Royale.Packets.Messages.Client.Battle;

namespace CRepublic.Royale.Packets
{
    internal class MessageFactory
    {
        public static Dictionary<int, Type> Messages;

        public MessageFactory()
        {
            Messages = new Dictionary<int, Type>
            {
                {10100, typeof(Pre_Authentification)},
                {10101, typeof(Authentification)},
                {10107, typeof(Client_Capabilities)},
                {10108, typeof(Keep_Alive)},
                {10113, typeof(Set_Device_Token)},
                {10121, typeof(Unlock_Account)},
                {10212, typeof(Change_Name)},
                //{10513, typeof(Bind_Google)},
                {10905, typeof(Open_Inbox)},
                {12904, typeof(Sector_Command)},
                {12951, typeof(Battle_Commands)},
                {14358, typeof(Request_Create_Alliance)},//crash game for ever
                {14101, typeof(Go_Home)},
                {14102, typeof(Execute_Commands)},
                {14104, typeof(Battle_NPC)},
                {14107, typeof(Cancel_Battle)},
                //{14111, typeof(Cancel_Tournament_Battle)},
               // {14113, typeof(Ask_Profile)},//crash game for ever
                {14201, typeof(Bind_Facebook)},
                {14301, typeof(Request_Create_Alliance)},
                {14302, typeof(Request_Alliance_Data)},
                {14303, typeof(Request_Joinable)},
                {14315, typeof(Send_Clan_Message)},
                {14405, typeof(Avatar_Stream)},
                {14406, typeof(Battle_Stream)},
                {14600, typeof(Request_Name_Change)},
                {16103, typeof(Joinable_Tournaments)}
            };
        }
    }
}