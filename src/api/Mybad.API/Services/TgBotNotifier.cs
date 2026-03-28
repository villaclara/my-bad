using Mybad.Core.Services;
using Mybad.Storage.DB.Services;
using Telegram.Bot;

namespace Mybad.API.Services;

public class TgBotNotifier : INotifier
{
    private readonly string _env = string.Empty;
    private readonly TelegramBotClient _bot;
    private readonly TgBotSubscriberService _tgService;

    public TgBotNotifier(TelegramBotClient bot, TgBotSubscriberService tgService, IWebHostEnvironment webHost)
    {
        _bot = bot;
        _tgService = tgService;
        _env = webHost.IsProduction() ? string.Empty : "DEV!";
    }

    /// <summary>
    /// Sends message in Tg to all chats that are subscribed (got from TgService).
    /// </summary>
    /// <remarks>It does not handle exceptions for now. We dont actually care what happens after we try to send so whatever.</remarks>
    public async Task NotifyAsync(NotifyMessage message)
    {
        var msg = $"{_env}\n{message}";
        var chats = await _tgService.GetSubsAsync();
        foreach (var id in chats)
        {
            try
            {
                await _bot.SendMessage(id, msg, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
            }
            catch
            {
            }
        }
    }
}
