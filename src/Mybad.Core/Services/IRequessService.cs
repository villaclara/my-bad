using Mybad.Core.Models;

namespace Mybad.Core.Services;

public interface IRequessService
{
	Task<BaseResponse> GetData();
}
