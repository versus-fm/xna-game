function main()
    resources:loadTiles(ADDON_PATH .. "/res/tiles.json");
    resources:loadSpritesheet(ADDON_PATH .. "/res/images/desertTileset.png", 16, 16, "desertTileset");
    resources:loadFont(ADDON_PATH .. "/res/fonts/roboto.ttf", 36, "", " ", "~", "roboto");
    resources:loadImage(ADDON_PATH .. "/res/images/gui/check.png", "guiCheck");
    resources:loadImage(ADDON_PATH .. "/res/images/gui/uncheck.png", "guiUnCheck");
    resources:loadImage(ADDON_PATH .. "/res/images/gui/icon.png", "guiIconFrame");
    resources:loadImage(ADDON_PATH .. "/res/images/gui/button.png", "guiButton");
    resources:loadImage(ADDON_PATH .. "/res/images/gui/buttonPressed.png", "guiButtonPressed");
end