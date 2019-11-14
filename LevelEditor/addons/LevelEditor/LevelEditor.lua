function main()
    registerAssets();
    game:subscribeEvent("STATE_ENTER", enteredState);
    game:enterState("editor", false);
end

function enteredState(name)
    game:loadUserInterface(ADDON_PATH .. "/res/ui/editor", "editorStyle");
    tileToolControl = game:getControl("tileTool");
    colliderToolControl = game:getControl("colliderTool");
    tileToolControl.onCheckChanged = function(sender)
        toggleContextMenu(sender, "tileContextPanel");
        toggleContextMenu(sender, "tileList");
    end
    colliderToolControl.onCheckChanged = function(sender)
        toggleContextMenu(sender, "colliderContextPanel");
    end
end

function registerAssets()
    resources:loadStyle(ADDON_PATH .. "/res/styles/editor", "editorStyle");
end

function toggleContextMenu(control, targetContextName)
    target = game:getControl(targetContextName);
    target.setVisible(control.isChecked());
    target.dirty();
end