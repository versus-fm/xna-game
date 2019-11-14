using System;
using Game5.Data.LuaAPI;
using Game5.Data.LuaAPI.AddonAPI;
using Game5.Service;
using Game5.Service.Services.Interfaces;
using Steamworks;

namespace Game5
{
    public static class SteamCallback
    {
        public static CallResult<LobbyCreated_t> lobbyCreated;

        private static Callback<P2PSessionRequest_t> sessionRequest;
        private static Callback<LobbyDataUpdate_t> lobbyUpdated;
        private static Callback<LobbyEnter_t> lobbyEnter;
        private static Callback<GameLobbyJoinRequested_t> lobbyRequestToJoin;

        public static void Create()
        {
            sessionRequest = Callback<P2PSessionRequest_t>.Create(OnSessionRequest);
            lobbyUpdated = Callback<LobbyDataUpdate_t>.Create(OnLobbyUpdated);
            lobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
            lobbyRequestToJoin = Callback<GameLobbyJoinRequested_t>.Create(OnLobbyRequestToJoin);


            lobbyCreated = CallResult<LobbyCreated_t>.Create(OnLobbyCreated);
        }

        public static void OnSessionRequest(P2PSessionRequest_t request)
        {
            SteamNetworking.AcceptP2PSessionWithUser(request.m_steamIDRemote);
        }

        public static void OnLobbyUpdated(LobbyDataUpdate_t lobby)
        {
            var client = ServiceLocator.Get<IClient>();
            client.UpdateLobbyInformation();
            LuaContext.FireEvent(LuaConstants.STEAM_LOBBY_UPDATE);
        }

        public static void OnLobbyEntered(LobbyEnter_t lobby)
        {
            var client = ServiceLocator.Get<IClient>();
            client.Lobby = new CSteamID(lobby.m_ulSteamIDLobby);
            client.Host = SteamMatchmaking.GetLobbyOwner(new CSteamID(lobby.m_ulSteamIDLobby));
            client.UpdateLobbyInformation();
            LuaContext.FireEvent(LuaConstants.STEAM_LOBBY_ENTERED);
        }

        public static void OnLobbyRequestToJoin(GameLobbyJoinRequested_t lobby)
        {
            var client = ServiceLocator.Get<IClient>();
            client.JoinLobby(lobby.m_steamIDLobby);
        }

        //Callresults
        public static void OnLobbyCreated(LobbyCreated_t lobby, bool failure)
        {
            Console.WriteLine("Onlobbyenter: " + failure);
            Console.WriteLine(lobby.m_ulSteamIDLobby);
            var client = ServiceLocator.Get<IClient>();
            client.Lobby = new CSteamID(lobby.m_ulSteamIDLobby);
            LuaContext.FireEvent(LuaConstants.STEAM_LOBBY_CREATED);
        }
    }
}