using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Validation;

internal  static class DomainValidators
{
    internal static Validator<Actor> ActorValidator => new Validator<Actor>()
     .AddRule(a => !string.IsNullOrEmpty(a.Name), Error.CreateFormExeption(new InvalidNameExeption()))
     .AddRule(a => a.Name?.Length < 200, Error.CreateFormExeption(new InvalidNameExeption()));
     



    internal static Validator<Buyer> BuyerValidator => ActorValidator.CreateCopy<Buyer>();
    internal static Validator<Seller> SellerValidator => ActorValidator.CreateCopy<Seller>();


}
