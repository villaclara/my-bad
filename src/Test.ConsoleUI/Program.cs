using Mybad.Core;
using Mybad.Core.Requests;
using Mybad.Core.Responses;
using Mybad.Services.OpenDota;


IInfoProvider service = new OpendotaProvider();

//BaseRequest request = new WardsRequest
//{
//	URL = "/matches/8519566987",
//	RequestType = RequestType.Wards,
//	MatchesCount = 10,
//};
//var aa = await service.GetData(request);
//var bb = (WardsInfo)aa;
//Console.WriteLine(bb.Players.Count);

//BaseRequest request1 = new WardMapRequest
//{
//	RequestId = 1,
//	AccountId = 136996088
//};
//var aa1 = await service.GetData(request1);
//var bb1 = (WardsMapPlacementResponse)aa1;
//Console.WriteLine(bb1.ObserverWards.Count);
//foreach (var kvp in bb1.ObserverWards)
//{
//	Console.WriteLine(kvp.X + " " + kvp.Y + " " + kvp.Amount);
//}

BaseRequest req = new WardLogRequest(136996088);

var a = await service.GetData(req);
WardsLogMatchResponse b = (WardsLogMatchResponse)a;
foreach (var item in b.ObserverWardsLog)
{
	Console.WriteLine(item.X + " " + item.Y + "-" + item.TimeLived + " " + item.Amount);
}
foreach (var item in b.SentryWardsLog)
{
	Console.WriteLine(item.X + " " + item.Y + "-" + item.TimeLived + " " + item.Amount);
}
Console.WriteLine("END");
