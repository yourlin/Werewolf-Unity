# Werewolf Game Client Documentation

## Overview

The Werewolf game client is crafted on the Unity engine.

## Primary Contents of the Game Assets Directory

Animations: Encompassing Animation Clips and Animation Controllers.
Art: A myriad of visual elements, including character portraits, character animation sprites, graphical user interfaces, and tiled maps.
Audio: Background melodies and sound effects.
Characters: Prefabricated character placements.
Scenes: Scene placements, currently comprising two scenes - one for the opening sequence and another for the main gameplay.
Scripts: Various scripts governing the game's mechanics.

## Build Process

The build process is facilitated through the Unity Editor, with the post-build frontend directory structure as follows:

```
├── Werewolf.app
├── config.json
└── history.txt
```

`Werewolf.app` represents the compiled application; on Windows platforms, the filename would be `Werewolf.exe`.

`config.json` houses the application's configuration, dynamically loaded upon startup to modify in-game configuration parameters. This file can be located within the `StreamingAssets` directory during development mode, loading `StreamingAssets\config.json`. Post-build, if the file is absent, the default configuration embedded within the code is loaded.

`history.txt` chronicles the in-game content and chat logs from the current game session. The generation path for this file can be modified within `config.json`.

The contents of `config.json` are as follows:

```
{
    "gameLoopInterval": 3000.0,
    "requestTimeout": 1,
    "historyFilePath": "./history.txt",
    "APIUrl": "http://werewolf-demo.us-east-1.elb.amazonaws.com/"
}
```

* gameLoopInterval: The frequency of game loop polling, measured in milliseconds.
* requestTimeout: Network request timeout, measured in seconds.
* historyFilePath: Storage path for in-game chat logs.
* APIUrl: The endpoint for the server.

## Supported Platforms

* MacOS
* Windows