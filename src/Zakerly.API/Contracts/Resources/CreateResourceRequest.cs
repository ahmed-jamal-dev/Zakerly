namespace Zakerly.API.Contracts.Resources;

public sealed record CreateResourceRequest(
    string Name,
    string FilePath);