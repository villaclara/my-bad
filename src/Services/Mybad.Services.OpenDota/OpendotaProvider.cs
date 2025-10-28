using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Mybad.Core;
using Mybad.Core.Requests;
using Mybad.Services.OpenDota.ApiResponseModels;
using Mybad.Services.OpenDota.ApiResponseReaders;

[assembly: InternalsVisibleTo("OpenDotaService.Tests")]

namespace Mybad.Services.OpenDota;

public class OpendotaProvider : IInfoProvider
{
	private static string _urlPath = "https://api.opendota.com/api/";

	public async Task<BaseResponse> GetData(BaseRequest request)
	{
		var task = request switch
		{
			WardLogSingleMatchRequest req => GetWardsInfoForMatch(req),
			WardLogRequest req => GetWardsLogInfo(req),
			WardMapRequest req => GetWardsPlacementMap(req),
			_ => throw new NotImplementedException($"{nameof(OpendotaProvider)} does not have implementations with this request type.")
		};

		return await task;
	}

	private async Task<BaseResponse> GetWardsPlacementMap(WardMapRequest request)
	{
		using var http = new HttpClient();
		//var response = await http.GetFromJsonAsync<WardsInfo>(_urlPath + $"players/136996088/matches?limit={request.MatchesCount}");

		try
		{
			var apiResponse = await http.GetFromJsonAsync<WardPlacementMap>(_urlPath + $"players/136996088/wardmap?having=100");

			if (apiResponse == null)
			{
				throw new InvalidOperationException();
			}

			var reader = new WardsPlacementMapReader();

			return reader.ConvertWardsPlacementMap(apiResponse);
		}
		catch (Exception)
		{
			throw;
		}
	}

	private async Task<BaseResponse> GetWardsInfoForMatch(WardLogSingleMatchRequest request)
	{
		var accountId = request.AccountId ?? 0;
		try
		{
			using var http = new HttpClient();
			var response = await http.GetFromJsonAsync<MatchWardLogInfo>(_urlPath + $"matches/{request.MatchId}");

			if (response == null)
			{
				throw new InvalidOperationException();
			}

			var reader = new WardsPlacementMapReader();

			return reader.ConvertWardsLogMatch(response, accountId);

		}
		catch (Exception)
		{
			throw;
		}

		throw new NotImplementedException();
	}

	/*
	 * Flow
	 * 1. Get player Id 
	 * 2. Get last {amount} matches ID for the player
	 * 3. On every match check if the match is parsed
	 * 4. If yes - then get wards info for match
	 * 4.1. if no - ask to parse and save the state somewhere
	 * 5. Wait for all matches to be checked 
	 * 6. return
	 */
	private async Task<BaseResponse> GetWardsLogInfo(WardLogRequest request)
	{
		throw new NotImplementedException();
	}

	private async Task<BaseResponse> GetHeroesInfo(string url)
	{
		throw new NotImplementedException();
	}
}
