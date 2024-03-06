using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;

namespace VendingMachine.Applicaion.Core.Abstraction;
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
      where TCommand : ICommand<TResponse>

{
}

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
   where TCommand : ICommand

{
}
