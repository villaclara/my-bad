using Mybad.Core.Models.Entries;
using Mybad.Core.Responses;
using Mybad.Services.OpenDota.ApiResponseModels;

namespace Mybad.Services.OpenDota.ApiResponseReaders;

internal class WardsPlacementMapReader
{
	public WardsPlacementMapResponse ConvertWardsPlacementMap(WardPlacementMap apiReponse)
	{
		var response = new WardsPlacementMapResponse();
		foreach (var (x, innerDic) in apiReponse.Obs)
		{
			foreach (var (y, amount) in innerDic)
			{
				response.ObserverWards.Add(new Core.Models.Entries.Ward
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
				response.SentryWards.Add(new Core.Models.Entries.Ward
				{
					X = int.Parse(x),
					Y = int.Parse(y),
					Amount = amount,
				});
			}
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

		// Checking observers
		foreach (var obs in playerInfo.ObsLog)
		{
			var timeLived = 360;    // Default duration - 6mins.
			if (playerInfo.ObsLeftLog.FirstOrDefault(x => x.EHandle == obs.EHandle) is WardLogEntry leftLogEntry)
			{
				/* 
				 * Calculating time depending on whether ward was placed and destroyed before/after horn.
				 */
				// put = -50, destroy = -20 => lived = 30s
				if (leftLogEntry.Time < 0 && obs.Time < 0)
				{
					timeLived = Math.Abs(obs.Time) - Math.Abs(leftLogEntry.Time);
				}

				// put = -20, destroy = 20 => lived = 40s
				if (leftLogEntry.Time < 0 && obs.Time > 0)
				{
					timeLived = leftLogEntry.Time + Math.Abs(obs.Time);
				}

				// put = 20, destroy = 40 => lived = 20s
				timeLived = leftLogEntry.Time - obs.Time;
			}
			var ward = new WardLog
			{
				X = (int)Math.Round(obs.X, MidpointRounding.AwayFromZero),
				Y = (int)Math.Round(obs.Y, MidpointRounding.AwayFromZero),
				TimeLived = timeLived,
				WasDestroyed = timeLived != 360,
				Amount = 1 // TODO - amount of ward when creating WardsLogMatchResponse.
						   // Dont know if i should increase count for all wards in same place.
			};
			response.ObserverWardsLog.Add(ward);
		}

		foreach (var sen in playerInfo.SenLog)
		{
			var timeLived = 420;    // Default duration - 7 mins
			if (playerInfo.SenLeftLog.FirstOrDefault(x => x.EHandle == sen.EHandle) is WardLogEntry leftLogEntry)
			{
				/* 
				 * Calculating time depending on whether ward was placed and destroyed before/after horn.
				 */
				// put = -50, destroy = -20 => lived = 30s
				if (leftLogEntry.Time < 0 && sen.Time < 0)
				{
					timeLived = Math.Abs(sen.Time) - Math.Abs(leftLogEntry.Time);
				}

				// put = -20, destroy = 20 => lived = 40s
				if (leftLogEntry.Time < 0 && sen.Time > 0)
				{
					timeLived = leftLogEntry.Time + Math.Abs(sen.Time);
				}

				// put = 20, destroy = 40 => lived = 20s
				timeLived = leftLogEntry.Time - sen.Time;
			}
			var ward = new WardLog
			{
				X = (int)Math.Round(sen.X, MidpointRounding.AwayFromZero),
				Y = (int)Math.Round(sen.Y, MidpointRounding.AwayFromZero),
				WasDestroyed = timeLived != 420,
				TimeLived = timeLived,
				Amount = 1 // TODO - amount of ward when creating WardsLogMatchResponse.
						   // Dont know if i should increase count for all wards in same place.
			};
			response.SentryWardsLog.Add(ward);
		}

		return response;
	}
}
