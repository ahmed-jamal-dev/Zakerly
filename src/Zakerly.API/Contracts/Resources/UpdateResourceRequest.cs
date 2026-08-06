namespace Zakerly.API.Contracts.Resources;

public sealed record UpdateResourceRequest(
    string Name,
    string FilePath);