namespace Mybad.Core;

public interface IInfoProvider<TRequest, TResponse>
	where TRequest : BaseRequest
	where TResponse : BaseResponse
{
	Task<TResponse> GetInfo(TRequest request);
}
