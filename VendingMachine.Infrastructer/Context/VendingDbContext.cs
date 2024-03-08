using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;
using VendingMachine.Domain.Interfaces;
using VendingMachine.Infrastructer.Interfaces;

namespace VendingMachine.Infrastructer.Context
{
    public class VendingDbContext: DbContext, IVendingDbContext, IUnitOfWork
    {
        public VendingDbContext(DbContextOptions  options)
         : base(options)
        {
              
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        public new DbSet<TEntity> Set<TEntity>()
   where TEntity : Entity
   => base.Set<TEntity>();

        public void Insert<TEntity>(TEntity entity)
       where TEntity : Entity
       => Set<TEntity>().Add(entity);

        /// <inheritdoc />
        public void InsertRange<TEntity>(IReadOnlyCollection<TEntity> entities)
            where TEntity : Entity
            => Set<TEntity>().AddRange(entities);

        /// <inheritdoc />
        public new void Remove<TEntity>(TEntity entity)
            where TEntity : Entity
            => Set<TEntity>().Remove(entity);


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            return await base.SaveChangesAsync(cancellationToken);
        }


        /// <inheritdoc />



        private IDbContextTransaction? tranasction;
        public async Task  BeginTransactionAsync(CancellationToken cancellationToken)
        {
            tranasction = await Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await tranasction!.CommitAsync(cancellationToken);
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            await tranasction!.RollbackAsync(cancellationToken);
        }
    }
}
