using System.Collections.Concurrent;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Mybad.Core;
using Mybad.Core.Requests;
using Mybad.Services.OpenDota.ApiResponseModels;
using Mybad.Services.OpenDota.ApiResponseModels.Player;
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

	private async Task<T?> ReadData<T>(BaseRequest request)
	{
		using var http = new HttpClient();
		var response = await http.GetFromJsonAsync<T>(_urlPath);
		return response;
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
		ArgumentNullException.ThrowIfNull(request.AccountId);

		using var http = new HttpClient();
		var response = await http.GetFromJsonAsync<List<RecentMatch>>(_urlPath + $"players/136996088/recentMatches");

		if (response == null)
		{
			throw new InvalidOperationException();
		}
		var reader = new WardsPlacementMapReader();

		// TODO - Check for database entries of matches

		var bag = new ConcurrentBag<MatchWardLogInfo>();

		//Parallel.ForEach(response, async (match) =>
		//{
		//	if (match.Lane != null)
		//	{
		//		var r = await http.GetFromJsonAsync<MatchWardLogInfo>(_urlPath + $"matches/{match.MatchId}");
		//		if (r != null)
		//		{
		//			File.WriteAllText($"Data/{match.MatchId}.txt", JsonSerializer.Serialize(r));
		//			bag.Add(r);
		//		}
		//	}
		//});

		Directory.CreateDirectory("Data");
		var tasks = response.Where(m => m.Lane != null)
			.Select(async match =>
			{
				var r = await http.GetFromJsonAsync<MatchWardLogInfo>(_urlPath + $"matches/{match.MatchId}");

				if (r != null)
				{
					var json = JsonSerializer.Serialize(r, new JsonSerializerOptions { WriteIndented = true });
					var filePath = Path.Combine("Data", $"{match.MatchId}.txt");
					await File.WriteAllTextAsync(filePath, json);
					return r;
				}

				return null;
			})
			.ToList();

		var result = await Task.WhenAll(tasks);

		//var tasks = new List<Task>();
		//foreach (var match in response.Matches)
		//{
		//	if (match.Lane != null)
		//	{
		//		var task = Task.Run(async () =>
		//		{
		//			var r = await http.GetFromJsonAsync<MatchWardLogInfo>(_urlPath + $"matches/{match.MatchId}");
		//		}).ContinueWith(t =>
		//		{
		//			if (response == null)
		//			{
		//				throw new InvalidOperationException();
		//			}
		//		});
		//		tasks.Add(task);
		//	}
		//}

		return reader.ConvertWardsLogManyMathes(result.ToList()!, (long)request.AccountId);
	}

	private async Task<BaseResponse> GetHeroesInfo(string url)
	{
		throw new NotImplementedException();
	}


	private async Task<MatchWardLogInfo> DownloadMatchLogInfo(string url)
	{
		throw new NotImplementedException();

	}
}
