namespace Mybad.Core.Admin.Dashboard;

internal class Dtos
{
}

public record TableMetadata(string Name, List<string> Columns);

public record TableData<T>(string Name, List<T> Data)
    where T : class;
