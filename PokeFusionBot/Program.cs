using TelegramBotBase.Builder;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("PokeFusionBot, I choose you!");

var token = await ConfigUtility.GetToken();

var bot = BotBaseBuilder
    .Create()
    .QuickStart<StartForm>(token)
    .Build();

// Start your Bot
await bot.Start();
Console.ReadLine();