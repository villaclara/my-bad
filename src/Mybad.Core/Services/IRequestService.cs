using Mybad.Core.Models;
using Mybad.Core.Models.Responses;

namespace Mybad.Core.Services;

public interface IRequestService
{
	Task<BaseResponse> GetData(BaseRequest request);
}
