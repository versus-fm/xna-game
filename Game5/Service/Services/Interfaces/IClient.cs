using System.Collections.Generic;
using Game5.Env;
using Game5.Env.World;
using Microsoft.Xna.Framework;
using Steamworks;

namespace Game5.Service.Services.Interfaces
{
    public interface IClient
    {
        CSteamID Host { get; set; }
        CSteamID Lobby { get; set; }
        List<CSteamID> RemoteUsers { get; }
        CSteamID User { get; }
        IWorldManager WorldManager { get; set; }

        void CreateLobby(ELobbyType lobbyType, int maxUsers);
        IWorldManager GetWorldManager();
        bool IsHost();
        void JoinLobby(CSteamID id);
        void LeaveLobby();
        void ReadData(byte[] msg);
        void ReadMessages();

        void SendCustomData<T>(short packetId, T data, EP2PSend m = EP2PSend.k_EP2PSendReliable,
            bool sendToHost = false) where T : new();

        void SendEntityData<T>(short packetId, T data, Entity sender, EP2PSend m = EP2PSend.k_EP2PSendReliable,
            bool sendToHost = false) where T : new();

        void SendWorldData<T>(short packetId, T data, IWorld sender, EP2PSend m = EP2PSend.k_EP2PSendReliable,
            bool sendToHost = false) where T : new();

        void SetWorldManager(IWorldManager manager);
        void Update(GameTime gameTime);
        void UpdateLobbyInformation();
    }
}