using System.Diagnostics;
using Mybad.Core.Services;
using Mybad.Services.OpenDota.Cachers;

namespace Mybad.API.Services;

public class HeroMatchupCacherHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<HeroMatchupCacherHostedService> _logger;
    private readonly HeroMatchupCacherStatus _status;
    private const int _timeoutS = 900;

    public HeroMatchupCacherHostedService(
        IServiceScopeFactory scopeFactory,
        ILogger<HeroMatchupCacherHostedService> logger,
        HeroMatchupCacherStatus status)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _status = status;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(_timeoutS));
        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            if (_status.IsEnabled)
            {
                await DoWork();
            }
        }
    }

    /// <inheritdoc />
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        await base.StopAsync(stoppingToken);
    }

    /// <summary>
    /// Creates scope, and calls service to update db with new info.
    /// </summary>
    private async Task DoWork()
    {
        using var scope = _scopeFactory.CreateScope();
        var cacher = scope.ServiceProvider.GetRequiredService<ODotaHeroMatchupCacher>();
        var notifier = scope.ServiceProvider.GetService<INotifier>();
        var sw = Stopwatch.StartNew();
        var opResult = false;

        try
        {
            await cacher.UpdateHeroMatchupsDatabase(minRank: 75);
            opResult = true;
        }
        catch (Exception ex)
        {
            if (notifier is not null)
            {
                await notifier.NotifyAsync(new NotifyMessage($"<b>[{DateTime.UtcNow} UTC]</b> - UpdateHeroMatchup failed. Exception:\n{ex.Message}."));
            }
        }
        finally
        {
            sw.Stop();
            if (notifier is not null)
            {
                await notifier.NotifyAsync(new NotifyMessage(
                    $"<b>[{DateTime.UtcNow} UTC]</b> - UpdateHeroMatchup finished with success - <b>{opResult.ToString().ToUpperInvariant()}</b>.\nMatches in Db - {cacher.CachedMatchesCount}.\nTime elapsed - {sw.Elapsed.TotalSeconds} s."));
            }
        }
    }
}
