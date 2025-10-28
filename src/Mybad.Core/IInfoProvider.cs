namespace Mybad.Core;

public interface IInfoProvider
{
	Task<BaseResponse> GetData(BaseRequest request);
}
