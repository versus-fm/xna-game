using System;
using System.Collections.Generic;
using System.IO;
using Game5.Data.Attributes.DependencyInjection;
using Game5.Data.Attributes.Network;
using Game5.Data.Attributes.Service;
using Game5.Data.Helper;
using Game5.Env;
using Game5.Env.World;
using Game5.Network;
using Game5.Service.Services.Interfaces;
using Microsoft.Xna.Framework;
using Steamworks;

namespace Game5.Service.Services.Implementations
{
    [Service(typeof(IClient))]
    public class SteamWorksClient : IClient
    {
        [FactoryConstructor]
        public SteamWorksClient(IWorldManager worldManager)
        {
            WorldManager = worldManager;
            Lobby = new CSteamID();
            RemoteUsers = new List<CSteamID>();
            User = SteamUser.GetSteamID();
        }

        public IWorldManager WorldManager { get; set; }
        public CSteamID Lobby { get; set; }
        public CSteamID User { get; }
        public CSteamID Host { get; set; }
        public List<CSteamID> RemoteUsers { get; }

        public void CreateLobby(ELobbyType lobbyType, int maxUsers)
        {
            var call = SteamMatchmaking.CreateLobby(lobbyType, maxUsers);
            SteamCallback.lobbyCreated.Set(call);
        }

        public void LeaveLobby()
        {
            SteamMatchmaking.LeaveLobby(Lobby);
            Lobby = new CSteamID();
        }

        public void JoinLobby(CSteamID id)
        {
            SteamMatchmaking.JoinLobby(id);
        }


        public void Update(GameTime gameTime)
        {
            ReadMessages();
        }

        public bool IsHost()
        {
            return User.m_SteamID == Host.m_SteamID;
        }

        public void ReadMessages()
        {
            while (SteamNetworking.IsP2PPacketAvailable(out var size))
            {
                var data = new byte[size];
                var packet =
                    SteamNetworking.ReadP2PPacket(data, (uint) data.Length, out var msgSize, out var steamUser);
                ReadData(data);
            }
        }

        public void UpdateLobbyInformation()
        {
            RemoteUsers.Clear();
            var num = SteamMatchmaking.GetNumLobbyMembers(Lobby);
            for (var i = 0; i < num; i++)
            {
                var id = SteamMatchmaking.GetLobbyMemberByIndex(Lobby, i);
                RemoteUsers.Add(id);
            }
        }

        public void SendEntityData<T>(short packetId, T data, Entity sender, EP2PSend m = EP2PSend.k_EP2PSendReliable,
            bool sendToHost = false) where T : new()
        {
            var msg = new byte[1024];
            var pos = 0;
            msg = msg.Write(ref pos, packetId);
            msg = msg.Write(ref pos, sender.World.GetID());
            msg = msg.Write(ref pos, sender.Id);
            msg = msg.Write(ref pos, data.GetType().Name.ToSnakeCase());
            //var b = TypeHelper.WriteProperties(data);
            //msg = msg.Append(pos, b);
            //pos += b.Length;
            if (sendToHost)
                SteamNetworking.SendP2PPacket(Host, msg, (uint) msg.Length, m);
            else
                foreach (var steamUser in RemoteUsers)
                    if (steamUser != User)
                        SteamNetworking.SendP2PPacket(steamUser, msg, (uint) msg.Length, m);
        }

        public void SendWorldData<T>(short packetId, T data, IWorld sender, EP2PSend m = EP2PSend.k_EP2PSendReliable,
            bool sendToHost = false) where T : new()
        {
            var msg = new byte[1024];
            var pos = 0;
            msg = msg.Write(ref pos, packetId);
            msg = msg.Write(ref pos, sender.GetID());
            msg = msg.Write(ref pos, data.GetType().Name.ToSnakeCase());
            //var b = TypeHelper.WriteProperties(data);
            //msg = msg.Append(pos, b);
            //pos += b.Length;
            if (sendToHost)
                SteamNetworking.SendP2PPacket(Host, msg, (uint) msg.Length, m);
            else
                foreach (var steamUser in RemoteUsers)
                    if (steamUser != User)
                        SteamNetworking.SendP2PPacket(steamUser, msg, (uint) msg.Length, m);
        }

        public void SendCustomData<T>(short packetId, T data, EP2PSend m = EP2PSend.k_EP2PSendReliable,
            bool sendToHost = false) where T : new()
        {
            var msg = new byte[1024];
            var pos = 0;
            msg = msg.Write(ref pos, packetId);
            msg = msg.Write(ref pos, data.GetType().Name.ToSnakeCase());
            //var b = TypeHelper.WriteProperties(data);
            //msg = msg.Append(pos, b);
            //pos += b.Length;
            if (sendToHost)
                SteamNetworking.SendP2PPacket(Host, msg, (uint) msg.Length, m);
            else
                foreach (var steamUser in RemoteUsers)
                    if (steamUser != User)
                        SteamNetworking.SendP2PPacket(steamUser, msg, (uint) msg.Length, m);
        }

        public void ReadData(byte[] msg)
        {
            using (var reader = new BinaryReader(new MemoryStream(msg)))
            {
                var packetId = reader.ReadInt16();
                var headerType = (HeaderType) (packetId / 10000);
                switch (headerType)
                {
                    case HeaderType.Entity:
                    {
                        var worldId = reader.ReadByte();
                        var guid = reader.ReadInt16();
                        var dataType = NetworkMethodCache.GetSendable(reader.ReadNullTerminatedString());
                        var dataTarget = Activator.CreateInstance(dataType);
                        var entity = WorldManager.GetWorld(worldId).GetEntity(guid);
                        //TypeHelper.ReadProperties(reader, dataTarget);
                        var methods = NetworkMethodCache.GetEntity(packetId);
                        foreach (var method in methods) method(entity, dataTarget);
                    }
                        break;
                    case HeaderType.World:
                    {
                        var worldId = reader.ReadByte();
                        var dataType = NetworkMethodCache.GetSendable(reader.ReadNullTerminatedString());
                        var dataTarget = Activator.CreateInstance(dataType);
                        var world = WorldManager.GetWorld(worldId);
                        //TypeHelper.ReadProperties(reader, dataTarget);
                        var methods = NetworkMethodCache.GetWorld(packetId);
                        foreach (var method in methods) method(world, dataTarget);
                    }
                        break;
                    case HeaderType.Custom:
                    {
                        var dataType = NetworkMethodCache.GetSendable(reader.ReadNullTerminatedString());
                        var dataTarget = Activator.CreateInstance(dataType);
                        //TypeHelper.ReadProperties(reader, dataTarget);
                        var methods = NetworkMethodCache.GetCustom(packetId);
                        foreach (var method in methods) method(dataTarget);
                    }
                        break;
                }
            }
        }

        public IWorldManager GetWorldManager()
        {
            return WorldManager;
        }

        public void SetWorldManager(IWorldManager manager)
        {
            WorldManager = manager;
        }
    }
}