﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Models;

namespace VendingMachine.Domain.Interfaces;

public interface ISellerRepository:IGenericRepository<Seller>
{
    Task<Seller?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> IsNameUniqueAsync(Guid execludedId, string name, CancellationToken cancellationToken = default);

}
