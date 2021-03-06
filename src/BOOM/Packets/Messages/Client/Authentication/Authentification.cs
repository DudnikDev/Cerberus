using System;
using CRepublic.Boom.Core;
using CRepublic.Boom.Packets.Messages.Server;
using CRepublic.Boom.Packets.Messages.Server.Stream;

namespace CRepublic.Boom.Packets.Messages.Client.Authentication
{
    using System.Linq;
    using System.Text;
    using CRepublic.Boom.Core.Network;
    using CRepublic.Boom.Library.Blake2B;
    using CRepublic.Boom.Library.Sodium;
    using CRepublic.Boom.Logic;
    using CRepublic.Boom.Logic.Enums;
    using CRepublic.Boom.Packets.Messages.Server.Authentication;
    using CRepublic.Boom.Extensions.Binary;

    internal class Authentification : Message
    {
        public Authentification(Device device, Reader reader) : base(device, reader)
        {
            this.Device.PlayerState = State.LOGIN;
        }


        internal StringBuilder Reason;
        internal long UserId;

        internal string Token;
        internal string MasterHash;
        internal string Language;

        internal int Major;
        internal int Minor;
        internal int Revision;

        internal override void Decrypt()
        {
            byte[] Buffer = this.Reader.ReadBytes(this.Length);
            this.Device.Keys.PublicKey = Buffer.Take(32).ToArray();

            Blake2BHasher Blake = new Blake2BHasher();

            Blake.Update(this.Device.Keys.PublicKey);
            Blake.Update(Key.PublicKey);

            this.Device.Keys.RNonce = Blake.Finish();

            Buffer = Sodium.Decrypt(Buffer.Skip(32).ToArray(), this.Device.Keys.RNonce, Key.PrivateKey, this.Device.Keys.PublicKey);
            this.Device.Keys.SNonce = Buffer.Skip(24).Take(24).ToArray();
            this.Reader = new Reader(Buffer.Skip(48).ToArray());

            this.Length = (ushort) Buffer.Length;
        }

        internal override void Decode()
        {
            this.UserId = this.Reader.ReadInt64();

            this.Token = this.Reader.ReadString();

            this.Device.Major = this.Reader.ReadInt32();
            this.Device.Minor = this.Reader.ReadInt32();
            this.Device.Revision = this.Reader.ReadInt32();

            this.MasterHash = this.Reader.ReadString();

            this.Reader.ReadString();

            this.Device.AndroidID = this.Reader.ReadString();
            this.Device.Model = this.Reader.ReadString();

            this.Reader.Seek(4); // 2000001

            this.Language = this.Reader.ReadString();
            this.Device.OpenUDID = this.Reader.ReadString();
            this.Device.OSVersion = this.Reader.ReadString();

            this.Reader.Seek(4);

            this.Device.Android = this.Reader.ReadBoolean();

            this.Device.AndroidID = this.Reader.ReadString();
        }

        internal override void Process()
        {
            if (this.UserId == 0)
            {
                this.Device.Player = Resources.Players.New();

                if (this.Device.Player != null)
                {
                    this.Login();
                }
                else
                {
                    new Authentification_Failed(this.Device, Logic.Enums.Reason.Pause).Send();
                }
            }
            else if (this.UserId > 0)
            {
                this.Device.Player = Resources.Players.Get(this.UserId, Constants.Database, false);

                if (this.Device.Player != null)
                {
                    if (string.Equals(this.Token, this.Device.Player.Avatar.Token))
                    {
                        if (this.Device.Player.Avatar.Locked)
                        {
                            new Authentification_Failed(this.Device, Logic.Enums.Reason.Locked).Send();
                        }
                        else if (this.Device.Player.Avatar.Banned)
                        {
                            this.Reason = new StringBuilder();
                            this.Reason.AppendLine("Your Player have been banned on our servers, please contact one of the TeamCrayCray staff with these following informations if you are not satisfied with the ban :");
                            this.Reason.AppendLine();
                            this.Reason.AppendLine("Your Player Name         : " + this.Device.Player.Avatar.Username + ".");
                            this.Reason.AppendLine("Your Player ID           : " + this.Device.Player.Avatar.UserHighId + "-" + this.Device.Player.Avatar.UserLowId + ".");
                            this.Reason.AppendLine("Your Player Ban Duration : " + Math.Round((this.Device.Player.Avatar.BanTime - DateTime.UtcNow).TotalDays, 3) + " Day.");
                            this.Reason.AppendLine("Your Player Unlock Date  : " + this.Device.Player.Avatar.BanTime);
                            this.Reason.AppendLine();

                            new Authentification_Failed(this.Device, Logic.Enums.Reason.Banned)
                            {
                                Message = Reason.ToString()
                            }.Send();
                        }
                        else
                        {
                            this.Login();
                        }
                    }
                    else
                    {
                        new Authentification_Failed(this.Device, Logic.Enums.Reason.Locked).Send();
                    }
                }
                else
                {
                    this.Reason = new StringBuilder();
                    this.Reason.AppendLine("Your Device have been block from accessing our servers due to invalid Id, please  clear your game data or contact one of the BarbarianLand staff with these following informations if you are not able to clear you game data :");
                    this.Reason.AppendLine();
                    this.Reason.AppendLine("Your Device IP         : " + this.Device.IPAddress +  ".");
                    this.Reason.AppendLine("Your Requested ID       : " + this.UserId  + ".");
                    this.Reason.AppendLine();

                    new Authentification_Failed(this.Device, Logic.Enums.Reason.Banned)
                    {
                        Message = Reason.ToString()
                    }.Send();
                }
            }
        }

        internal void Login()
        {
            this.Device.Player.Client = this.Device;

            bool Admin = false;
            this.Device.Player.Home = Admin  ? new Home("layout/enemybase.level")  : new Home();

            new Authentification_OK(this.Device).Send();
            new Own_Home_Data(this.Device).Send();
            new Maintenance_Inbound(this.Device, (int)TimeSpan.FromMinutes(100).TotalSeconds).Send();
            //new Avatar_Stream_Entry(this.Device).Send();
        }
    }
}
