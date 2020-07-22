using System;
using CRepublic.Royale.Core.Network;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Logic;
using CRepublic.Royale.Packets.Messages.Server;

namespace CRepublic.Royale.Packets.Messages.Client
{
    internal class Open_Inbox : Message
    {
        public Open_Inbox(Device Device, Reader Reader) : base(Device, Reader)
        {
            // Open_Inbox
        }


        internal override void Process()
        {
            new Inbox_Data(Device).Send();
        }
    }
}
