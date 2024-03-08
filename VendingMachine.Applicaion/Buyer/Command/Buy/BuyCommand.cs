using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VendingMachine.Applicaion.Core.Abstraction;
using VendingMachine.Domain.Models;

namespace VendingMachine.Applicaion.Buyers.Command.Buy;

public sealed record BuyCommand(Guid BuyerId , Guid ProductId ,int Amount):ICommand<BuyCommandResponse>;

public sealed record BuyCommandResponse(int TotalSpent,Product Product,Dictionary<int,int> Change);
