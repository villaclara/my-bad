using System.Text.Json;
using Mybad.Core.Models.Entries;
using Mybad.Services.OpenDota.ApiResponseModels;
using Mybad.Services.OpenDota.ApiResponseReaders;

namespace OpenDotaService.Tests;

public class ApiResponseConverterTests
{
	[Fact]
	public void GetWardsPlacementMap_ReturnsWardsPlacementResponse()
	{
		// Arrange 
		var apiResponseFilePath = "Data/wardmap.txt";
		var json = File.ReadAllText(apiResponseFilePath);
		var obsCount = 22;
		var distinctObsCount = 19;
		var senCount = 50;
		var distinctSenCount = 40;

		// Act 
		var data = JsonSerializer.Deserialize<WardPlacementMap>(json);
		var reader = new WardsPlacementMapReader();
		var response = reader.ConvertWardsPlacementMap(data!);
		int oCount = 0, sCount = 0;
		foreach (var kvp in response.ObserverWards)
		{
			oCount += kvp.Amount;
		}

		foreach (var kvp in response.SentryWards)
		{
			sCount += kvp.Amount;
		}

		// Assert
		Assert.NotNull(response);
		Assert.Equal(distinctObsCount, response.ObserverWards.Count);
		Assert.Equal(distinctSenCount, response.SentryWards.Count);
		Assert.Equal(obsCount, oCount);
		Assert.Equal(senCount, sCount);
	}

	[Fact]
	public void GetWardsForPlayerPerMatch_ReturnsWarsLogMatchResponse()
	{
		// Arrange
		var apiResponseFilePath = "Data/matchWards.txt";
		var json = File.ReadAllText(apiResponseFilePath);
		var accountId = 136996088;
		var obsCount = 3;
		var senCount = 12;
		var firstObs = new WardLog
		{
			Amount = 1,
			TimeLived = 360,
			X = 174,
			Y = 102,
			WasDestroyed = false
		};
		var secondObs = new WardLog
		{
			Amount = 1,
			TimeLived = 360,
			X = 132,
			Y = 183,
			WasDestroyed = false
		};
		var thirdObs = new WardLog
		{
			Amount = 1,
			TimeLived = 360,
			X = 163,
			Y = 156,
			WasDestroyed = false
		};
		var firstSentry = new WardLog
		{
			Amount = 1,
			X = 162,
			Y = 95,
			WasDestroyed = true,
			TimeLived = 32,
		};
		var secondSentry = new WardLog
		{
			Amount = 1,
			X = 162,
			Y = 95,
			WasDestroyed = true,
			TimeLived = 272,
		};
		var thirdSentry = new WardLog
		{
			Amount = 1,
			X = 110,
			Y = 161,
			WasDestroyed = false,
			TimeLived = 420,
		};

		// Act 
		var data = JsonSerializer.Deserialize<MatchWardLogInfo>(json);
		var reader = new WardsPlacementMapReader();
		var response = reader.ConvertWardsLogMatch(data!, accountId);

		// Assert
		Assert.NotNull(response);
		Assert.Equal(obsCount, response.ObserverWardsLog.Count);
		Assert.Equal(senCount, response.SentryWardsLog.Count);
		Assert.Equivalent(firstObs, response.ObserverWardsLog[0]);
		Assert.Equivalent(secondObs, response.ObserverWardsLog[1]);
		Assert.Equivalent(thirdObs, response.ObserverWardsLog[2]);
		Assert.Equivalent(firstSentry, response.SentryWardsLog[0]);
		Assert.Equivalent(secondSentry, response.SentryWardsLog[1]);
		Assert.Equivalent(thirdSentry, response.SentryWardsLog[2]);
	}
}
