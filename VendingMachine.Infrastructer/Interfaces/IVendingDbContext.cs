using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;

namespace VendingMachine.Infrastructer.Interfaces
{
    public interface IVendingDbContext
    {
        DbSet<TEntity> Set<TEntity>()
     where TEntity : Entity;



        Task<TEntity?> GetByIdAsync<TEntity>(Guid id, CancellationToken cancellationToken = default)
            where TEntity : Entity;


        void Insert<TEntity>(TEntity entity)
          where TEntity : Entity;


        void InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities)
        where TEntity : Entity;


        void Remove<TEntity>(TEntity entity)
         where TEntity : Entity;

    }
}
