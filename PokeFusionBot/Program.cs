using TelegramBotBase.Builder;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("PokeFusionBot, I choose you!");

var token = await ConfigUtility.GetToken();

var bot = BotBaseBuilder
    .Create()
    .WithAPIKey(token)
    .DefaultMessageLoop()
    .WithStartForm<StartForm>()
    .NoProxy()
    .CustomCommands(a => {})
    .NoSerialization()
    .UseEnglish()
    .Build();

await bot.UploadBotCommands();

// Start your Bot
await bot.Start();
Console.ReadLine();