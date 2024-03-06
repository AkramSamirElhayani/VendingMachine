using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;

namespace VendingMachine.Applicaion.Core.Abstraction;
public interface ICommand<TRespons> : IRequest<Result<TRespons>>
{
}

public interface ICommand : IRequest<Result>
{
}
