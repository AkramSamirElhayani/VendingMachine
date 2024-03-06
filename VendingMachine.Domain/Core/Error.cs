using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Domain.Core;


public class Error : ValueObject<Error>
{
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }
    public string Message { get; }
    public static implicit operator string(Error error) => error?.Code ?? string.Empty;
    [ExcludeFromCodeCoverage]
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
        yield return Message;
    }

    public static Error CreateFormExeption(Exception ex)
    {
        return new Error($"{ex.GetType().Name}", ex.Message);
    }
}
