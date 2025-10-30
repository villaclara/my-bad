//IRequestService service = new OpendotaProvider();
//BaseRequest req = new WardLogRequest(136996088);

//var a = await service.GetData(req);
//WardsLogMatchResponse b = (WardsLogMatchResponse)a;
//foreach (var item in b.ObserverWardsLog)
//{
//	Console.WriteLine(item.X + " " + item.Y + "-" + item.TimeLived + " " + item.Amount);
//}
//foreach (var item in b.SentryWardsLog)
//{
//	Console.WriteLine(item.X + " " + item.Y + "-" + item.TimeLived + " " + item.Amount);
//}
//Console.WriteLine("END");


using Mybad.Core;
using Mybad.Core.Requests;
using Mybad.Core.Responses;
using Mybad.Services.OpenDota.Providers;

var req = new WardMapRequest(136996088);

IInfoProvider<WardMapRequest, WardsMapPlacementResponse> provider = new ODotaWardPlacementMapProvider();
var responce = await provider.GetInfo(req);

