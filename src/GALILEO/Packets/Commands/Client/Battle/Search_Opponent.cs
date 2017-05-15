﻿using System;
using BL.Servers.CoC.Core.Networking;
using BL.Servers.CoC.Extensions;
using BL.Servers.CoC.Extensions.Binary;
using BL.Servers.CoC.Logic;
using BL.Servers.CoC.Packets.Messages.Server.Battle;

namespace BL.Servers.CoC.Packets.Commands.Client.Battle
{
    internal class Search_Opponent : Command
    {
        internal long Enemy_ID;
        internal bool Max_Seed_Achieved;
        internal Level Enemy_Player;
        public Search_Opponent(Reader _Reader, Device _Client, int _ID) : base(_Reader, _Client, _ID)
        {
        }

        internal override void Decode()
        {
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
            this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            while (this.Enemy_Player == null)
            {
                if (this.Enemy_ID != this.Device.Player.Avatar.UserId && this.Enemy_ID > 0)
                {
                    if (this.Device.Player.Avatar.Last_Attack_Enemy_ID.FindIndex(P => P == this.Enemy_ID) < 0)
                    {
                        this.Enemy_Player = Core.Resources.Players.Get(this.Enemy_ID, Constants.Database, false);

                        if (this.Enemy_Player != null)
                        {
                            this.Device.Player.Avatar.Last_Attack_Enemy_ID.Add(
                                (int) this.Enemy_Player.Avatar.UserId);

                            if (this.Device.Player.Avatar.Last_Attack_Enemy_ID.Count > 20 ||
                                this.Device.Player.Avatar.Last_Attack_Enemy_ID.Count ==
                                Core.Resources.Players.Seed - 1)
                                this.Device.Player.Avatar.Last_Attack_Enemy_ID.RemoveAt(0);
                        }
                    }
                    else
                    {
                        this.Enemy_ID = Core.Resources.Random.Next(1, Convert.ToInt32(Core.Resources.Players.Seed - 1));
                    }
                }
                else
                {
                    this.Enemy_ID = Core.Resources.Random.Next(1, Convert.ToInt32(Core.Resources.Players.Seed - 1));
                }
            }

            if (this.Enemy_Player != null)
                new Pc_Battle_Data(this.Device) { Enemy = this.Enemy_Player }.Send();
            else
                new Npc_Data(this.Device) { Npc_ID = 17000020 }.Send();
        }
    }
}