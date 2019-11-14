function main()
    print(ADDON_NAME);
    game:subscribeEvent("STATE_ENTER", onEnterState);
    game:subscribeEvent("STEAM_LOBBY_ENTERED", onLobbyEntered);
    game:subscribeEvent("STEAM_LOBBY_UPDATE", onLobbyUpdate);
    game:subscribeEvent("STEAM_LOBBY_CREATED", onLobbyCreated);
end

function onEnterState(name)
    if(name == "menu") then
        game:loadUserInterface(ADDON_PATH .. "/uidef/ui", "style1");
        quit = game:getControl("quitButton");
        start = game:getControl("startButton");
        quit.onClick = function(sender)
            game:exit();
        end
        start.onClick = function(sender)
            steam:createLobby(4);
        end
    end
    if(name == "lobby") then
        game:loadUserInterface(ADDON_PATH .. "/uidef/lobby", "style1");
    end
end

function onLobbyEntered()
    game:enterState("lobby", false);
    members = steam:getLobbyUsers();
    for i=1, #members do
        if(members[i]) then
            label = game:getControl("member" .. i);
            label.setContent(members[i]);
        end
    end
end

function onLobbyUpdate()
    members = steam:getLobbyUsers();
    for i=1, #members do
        if(members[i]) then
            label = game:getControl("member" .. i);
            label.setContent(members[i]);
        end
    end
end

function onLobbyCreated()
end