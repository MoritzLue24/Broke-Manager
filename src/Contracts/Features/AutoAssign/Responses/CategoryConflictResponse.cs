namespace Contracts.Features.AutoAssign.Responses;

public record CategoryConflictResponse(
    Guid CategoryId,
    double Score
);