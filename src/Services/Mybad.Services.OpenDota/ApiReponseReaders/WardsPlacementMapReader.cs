using Mybad.Core.Responses;
using Mybad.Core.Responses.Entries;
using Mybad.Services.OpenDota.ApiResponseModels;

namespace Mybad.Services.OpenDota.ApiResponseReaders;

internal class WardsPlacementMapReader
{
	public WardsMapPlacementResponse ConvertWardsPlacementMap(WardPlacementMap apiReponse)
	{
		var response = new WardsMapPlacementResponse();
		foreach (var (x, innerDic) in apiReponse.Obs)
		{
			foreach (var (y, amount) in innerDic)
			{
				response.ObserverWards.Add(new Ward
				{
					X = int.Parse(x),
					Y = int.Parse(y),
					Amount = amount
				});
			}
		}

		foreach (var (x, innerDic) in apiReponse.Sen)
		{
			foreach (var (y, amount) in innerDic)
			{
				response.SentryWards.Add(new Ward
				{
					X = int.Parse(x),
					Y = int.Parse(y),
					Amount = amount,
				});
			}
		}

		return response;
	}

	public List<WardLog> ConvertWards(List<WardLogEntry> wardLog, List<WardLeftLogEntry> wardLeftLog, bool isObs = true)
	{
		var response = new List<WardLog>();
		var defaultTime = isObs ? 360 : 420;

		foreach (var ward in wardLog)
		{
			var timeLived = defaultTime;    // Default duration - 6mins.
			if (wardLeftLog.FirstOrDefault(x => x.EHandle == ward.EHandle) is WardLogEntry leftLogEntry)
			{
				/* 
				 * Calculating time depending on whether ward was placed and destroyed before/after horn.
				 */
				// put = -50, destroy = -20 => lived = 30s
				if (leftLogEntry.Time < 0 && ward.Time < 0)
				{
					timeLived = Math.Abs(ward.Time) - Math.Abs(leftLogEntry.Time);
				}

				// put = -20, destroy = 20 => lived = 40s
				if (leftLogEntry.Time < 0 && ward.Time > 0)
				{
					timeLived = leftLogEntry.Time + Math.Abs(ward.Time);
				}

				// put = 20, destroy = 40 => lived = 20s
				timeLived = leftLogEntry.Time - ward.Time;
			}
			var wardL = new WardLog
			{
				X = (int)Math.Round(ward.X, MidpointRounding.AwayFromZero),
				Y = (int)Math.Round(ward.Y, MidpointRounding.AwayFromZero),
				TimeLived = timeLived,
				WasDestroyed = timeLived != defaultTime,
				Amount = 1 // TODO - amount of ward when creating WardsLogMatchResponse.
						   // Dont know if i should increase count for all wards in same place.
			};
			response.Add(wardL);
		}

		return response;
	}

	public WardsLogMatchResponse ConvertWardsLogMatch(MatchWardLogInfo apiReponse, long accountId)
	{
		var response = new WardsLogMatchResponse();

		var playerInfo = apiReponse.Players.FirstOrDefault(x => x.AccountId == accountId);
		if (playerInfo == null)
		{
			throw new InvalidOperationException();
		}

		response.ObserverWardsLog = ConvertWards(playerInfo.ObsLog, playerInfo.ObsLeftLog, isObs: true);
		response.SentryWardsLog = ConvertWards(playerInfo.SenLog, playerInfo.SenLeftLog, isObs: false);

		return response;
	}

	public WardsLogMatchResponse ConvertWardsLogManyMathes(List<MatchWardLogInfo> mathesLogs, long accountId)
	{
		var response = new WardsLogMatchResponse();

		var obses = new List<WardLog>();
		var sens = new List<WardLog>();

		Dictionary<(int X, int Y), WardLog> dicObs = [];
		Dictionary<(int X, int Y), WardLog> dicSen = [];

		foreach (var log in mathesLogs)
		{
			var playerInfo = log.Players.FirstOrDefault(x => x.AccountId == accountId);
			if (playerInfo == null)
			{
				throw new InvalidOperationException();
			}

			obses.AddRange(ConvertWards(playerInfo.ObsLog, playerInfo.ObsLeftLog, true));
			sens.AddRange(ConvertWards(playerInfo.SenLog, playerInfo.SenLeftLog, false));
		}

		foreach (var obs in obses)
		{
			(int x, int y) key = (obs.X, obs.Y);
			if (!dicObs.TryGetValue(key, out WardLog? value))
			{
				dicObs.Add(key, obs);
			}
			else
			{
				value.Amount++;
				value.TimeLived = (value.TimeLived + obs.TimeLived) / value.Amount;
			}
		}

		foreach (var sen in sens)
		{
			(int x, int y) key = (sen.X, sen.Y);
			if (!dicSen.TryGetValue(key, out WardLog? value))
			{
				dicSen.Add(key, sen);
			}
			else
			{
				value.Amount++;
			}
		}

		response.ObserverWardsLog = [.. dicObs.Select(x => x.Value)];
		response.SentryWardsLog = [.. dicSen.Select(x => x.Value)];
		return response;
	}
}
