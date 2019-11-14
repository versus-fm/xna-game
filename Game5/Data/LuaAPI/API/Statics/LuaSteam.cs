using System;
using Game5.Data.Attributes.Lua;
using Game5.Service;
using Game5.Service.Services.Interfaces;
using Steamworks;

namespace Game5.Data.LuaAPI.API.Statics
{
    [LuaStaticClass("steam")]
    public class LuaSteam
    {
        public static void CreateLobby(int maxUsers, int type = 1)
        {
            var client = ServiceLocator.Get<IClient>();
            client.CreateLobby((ELobbyType) type, maxUsers);
        }

        public static void LeaveLobby()
        {
            var client = ServiceLocator.Get<IClient>();
            client.LeaveLobby();
        }

        public static void RefreshLobby()
        {
            var client = ServiceLocator.Get<IClient>();
            client.UpdateLobbyInformation();
        }

        public static string[] GetLobbyUsers()
        {
            var client = ServiceLocator.Get<IClient>();
            var names = new string[client.RemoteUsers.Count];
            for (var i = 0; i < names.Length; i++)
            {
                names[i] = SteamFriends.GetFriendPersonaName(client.RemoteUsers[i]);
                Console.WriteLine(names[i]);
            }

            return names;
        }
    }
}