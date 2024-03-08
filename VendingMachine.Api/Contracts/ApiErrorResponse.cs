using VendingMachine.Domain.Core;

namespace VendingMachine.Api.Contracts;

public class ApiErrorResponse
{

    public ApiErrorResponse(IReadOnlyCollection<Error> errors) => Errors = errors;

    public IReadOnlyCollection<Error> Errors { get; }
}
