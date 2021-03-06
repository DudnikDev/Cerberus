using System;
using System.Threading;
using System.Threading.Tasks;
using CRepublic.Magic.Logic;
using CRepublic.Magic.Core.Networking;
using CRepublic.Magic.Extensions;
using CRepublic.Magic.Extensions.Binary;
using CRepublic.Magic.Files;
using CRepublic.Magic.Logic.Enums;
using CRepublic.Magic.Packets.Messages.Server.Authentication;

namespace CRepublic.Magic.Packets.Messages.Client.Authentication
{
    internal class Pre_Authentication : Message
    {
        internal int AppStore;
        internal int DeviceSO;
        internal int KeyVersion;
        internal int Protocol;
        internal int Major;
        internal int Minor;
        internal int Revision;

        internal string Hash;

        public Pre_Authentication(Device Device) : base(Device)
        {
            this.Device.State = State.SESSION;
        }

        internal override void Decode()
        {
            this.Protocol = this.Reader.ReadInt32();
            this.KeyVersion = this.Reader.ReadInt32();
            this.Major = this.Reader.ReadInt32();
            this.Revision = this.Reader.ReadInt32();
            this.Minor = this.Reader.ReadInt32();
            this.Hash = this.Reader.ReadString();
            this.DeviceSO = this.Reader.ReadInt32();
            this.AppStore = this.Reader.ReadInt32();
        }

        internal override void Process()
        {
            if (this.Major == Convert.ToInt32(Constants.ClientVersion[0]) &&  this.Minor == Convert.ToInt32(Constants.ClientVersion[1]))
            {
                if (Constants.Maintenance == null)
                {
                    //if (string.Equals(this.Hash, Fingerprint.Sha))
                    {
                        new Pre_Authentication_OK(this.Device).Send();
                    }
                   // else
                    {
                     //   new Authentication_Failed(this.Device, Reason.Patch).Send();
                    }
                }
                else
                    new Authentication_Failed(this.Device, Reason.Maintenance).Send();
            }
            else
                new Authentication_Failed(this.Device, Reason.Update).Send();
        }
    }
}