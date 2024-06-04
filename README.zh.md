# 狼人杀客户端文档

## Overview

狼人杀客户端基于Unity编写。

## 游戏Assets目录主要内容

Animations：Animation Clips和Animation Controller
Art：各种图片元素，包括人物头像、人物动画图片，GUI，瓦片地图等
Audio：背景音乐和音效
Characters：放置角色的prefab
Scenes：放置场景，现在就2个场景，1个opening，一个main
Script：游戏中的各种脚本

## Build

使用Unity Editor进行构建，前端构建后目录如下：

```
├── Werewolf.app
├── config.json
└── history.txt
```

`Werewolf.app`为构建完成后的应用程序，如果是windows平台文件名应该为`Werewolf.exe`

`config.json`是应用程序的配置，这个配置会在应用启动后动态加载，并修改游戏内的配置参数。这个文件可以在`StreamingAssets`中找到，开发模式在加载`StreamingAssets\config.json`。构建后，如果文件不存在，则加载代码中的默认配置。

`history.txt`是运行后会客户端记录的本局游戏的游戏内容。该文件生成路径可在`config.json`中修改。

其中`config.json`内容如下：

```
{
    "gameLoopInterval": 3000.0,
    "requestTimeout": 1,
    "historyFilePath": "./history.txt",
    "APIUrl": "http://werewolf-demo.us-east-1.elb.amazonaws.com/"
}
```

* gameLoopInterval：游戏内效率轮询的频率，单位`毫秒`
* requestTimeout：网络请求超时，单位`秒`
* historyFilePath：游戏内容聊天记录存储路径
* APIUrl：服务器的endpoint

## Supported Platforms

* MacOS
* Windows

