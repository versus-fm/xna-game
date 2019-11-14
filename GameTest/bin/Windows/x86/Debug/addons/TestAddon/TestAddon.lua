function main()
    game:subscribeEvent("STATE_ENTER", enterState);
end

function enterState(stateName)
    if(stateName == "menu") then
        game:loadUserInterface(ADDON_PATH .. "/ui/menu", "style1");
        panel = game:getControl("mainPanel");
        print(panel.getWidth());
        print(panel.getHeight());
    end
end