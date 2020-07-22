﻿using System;
using System.Net.Sockets;
using CRepublic.Royale.Core;
using CRepublic.Royale.Core.Network;
using CRepublic.Royale.Extensions;
using CRepublic.Royale.Extensions.Binary;
using CRepublic.Royale.Logic.Enums;
using CRepublic.Royale.Packets;
using CRepublic.Royale.Packets.Cryptography.RC4;
using System.Diagnostics;
using CRepublic.Royale.Packets.Cryptography;

namespace CRepublic.Royale.Logic
{
    internal class Device
    {
        internal Socket Socket;
        internal Player Player;
        internal Token Token;
        internal Crypto Crypto;
        internal RC4_Core RC4;

        public Device(Socket Socket)
        {
            this.Socket = Socket;
            this.Crypto = new Crypto();
            this.RC4 = new RC4_Core();
            this.SocketHandle = Socket.Handle;
        }
        public Device(Socket Socket, Token token)
        {
            this.Socket = Socket;
            this.Crypto = new Crypto();
            this.RC4 = new RC4_Core();
            this.Token = token;
            this.SocketHandle = Socket.Handle;
        }

        internal Client_State PlayerState = Client_State.DISCONNECTED;

        internal IntPtr SocketHandle;

        internal bool Android;

        internal int Ping;
        internal int Tick;
        internal int Major;
        internal int Revision;
        internal int Minor;
        internal int LastChecksum;

        internal volatile int Dropped;

        internal string Interface;
        internal string AndroidID;
        internal string OpenUDID;
        internal string Model;
        internal string OSVersion;
        internal string MACAddress;
        internal string AdvertiseID;
        internal string VendorID;
        internal string IPAddress;

        public bool Connected()
        {
            try
            {
                return !(Socket.Poll(1000, SelectMode.SelectRead) && Socket.Available == 0 || !Socket.Connected);
            }
            catch
            {
                return false;
            }
        }

        internal void Process(byte[] Buffer)
        {
            const int HEADER_LEN = 7;
            if (Buffer.Length >= 5)
            {
                int length = (Buffer[2] << 16) | (Buffer[3] << 8) | Buffer[4];
                ushort type = (ushort)((Buffer[0] << 8) | Buffer[1]);
                if (Buffer.Length - HEADER_LEN >= length)
                {
                    var packet = new byte[length];
                    for (int i = 0; i < packet.Length; i++)
                        packet[i] = Buffer[i + HEADER_LEN];

                    if (MessageFactory.Messages.ContainsKey(type))
                    {
                        var Reader = new Reader(packet);
                        Message _Message =
                            Activator.CreateInstance(MessageFactory.Messages[type], this, Reader) as Message;
                        _Message.Identifier = type;
                        _Message.Length = (ushort)length;
                        _Message.Reader = Reader;

                        try
                        {
                            try
                            {
                                if (Constants.Encryption == Enums.Server_Crypto.SODIUM)
                                    _Message.DecryptRC4();
                                else
                                    _Message.DecryptSodium();
                            }
                            catch (Exception ex)
                            {
                                Server_Resources.Exceptions.Catch(ex,
                                    $"Unable to decrypt message with ID: {type}" + Environment.NewLine +
                                    ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine +
                                    ex.Data, this.Model, this.OSVersion, this.Player.Token,
                                    Player?.UserId ?? 0);
                            }

#if DEBUG
                            Loggers.Log(
                                Utils.Padding(_Message.Device.Socket.RemoteEndPoint.ToString(), 15) + " --> " +
                                _Message.GetType().Name, true);
                            Loggers.Log(_Message,
                                Utils.Padding(_Message.Device.Socket.RemoteEndPoint.ToString(), 15));
#endif

                            try
                            {
                                _Message.Decode();
                            }
                            catch (Exception ex)
                            {
                                Server_Resources.Exceptions.Catch(ex,
                                    $"Unable to decode message with ID: {type}" + Environment.NewLine + ex.Message +
                                    Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.Data, this.Model,
                                    this.OSVersion, this.Player.Token, Player?.UserId ?? 0);
                            }
                            try
                            {
                                _Message.Process();
                            }
                            catch (Exception ex)
                            {
                                Server_Resources.Exceptions.Catch(ex,
                                    $"Unable to process message with ID: {type}" + Environment.NewLine +
                                    ex.Message + Environment.NewLine + ex.StackTrace + Environment.NewLine +
                                    ex.Data, this.Model, this.OSVersion, this.Player.Token,
                                    Player?.UserId ?? 0);
                            }
                        }
                        catch (Exception Exception)
                        {
                            Server_Resources.Exceptions.Catch(Exception,
                                Exception.Message + Environment.NewLine + Exception.StackTrace +
                                Environment.NewLine + Exception.Data, this.Model, this.OSVersion,
                                this.Player.Token, Player?.UserId ?? 0);
                            Loggers.Log(Utils.Padding(Exception.GetType().Name, 15) + " : " + Exception.Message +
                                        ". [" + (this.Player != null
                                            ? this.Player.UserId + ":" +
                                              GameUtils.GetHashtag(this.Player.UserId)
                                            : "---") + ']' + Environment.NewLine + Exception.StackTrace, true,
                                Defcon.ERROR);
                        }
                    }
                    else
                    {
#if DEBUG
                        Loggers.Log(Utils.Padding(this.GetType().Name, 15) + " : Aborting, we can't handle the following message : ID " + type + ", Length " + length + ".", true, Defcon.WARN);
#endif
                        if (Constants.Encryption == Enums.Server_Crypto.SODIUM)
                        {
                            this.RC4.Decrypt(ref packet);
                        }
                        else
                            this.Crypto.SNonce.Increment();
                    }
                    this.Token.Packet.RemoveRange(0, length + HEADER_LEN);
                }
            }
        }
    }
}