# PokeFusion Bot

This is a dotnet core console app that hosts the core functionality for a Telegram bot.  

This bot polls groups and messages it has access to.  If the message contains two Pokemon names, it will post an image to that chat or group with the fused version of those Pokemon, if available.

Credit for the Image Source: https://kaboom242.github.io/PokemonInfiniteFusionTool/



## Tech Stack

C# Console App, Dotnet core.

Hosted on a Ubuntu Linux Server using GCM.

Deployed using Github Actions.

### How To Use

Message https://t.me/BotFather on Telegram to create a new bot and generate an api key.  Generate your token and put it in a txt file that matches your app.config file location.

Run the console application. As long as the bot is running, it will poll any chats and groups it has access to.

--Fusions
In Telegram, type any two Pokemon names. 
- Example: "Charmander Pikachu"
After a few moments, the bot will post the fusion image as a sticker into the same channel.

Some combinations have no fusion image. Use the tool above to evaluate valid combos.

--Battles

You will need to generate a ChatGPT Api key to generate battles.  Configure another txt file with that key.

Type in "battle" followed by two ids of PokeFusions.  A PokeFusion id is the id of each half, connected by a period. Battle responses often take >5 seconds.
- Example: Pikachu (id:25) fused with Bulbasaur (id:1) create a fusion with id:25.1
- Example Battle: "battle 1.25 4.55"



