﻿using Microsoft.EntityFrameworkCore;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Exceptions;
using ProjectHermes.ShoppingList.Api.Infrastructure.Entities;
using ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Entities;
using ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Adapters
{
    public class StoreRepository : IStoreRepository
    {
        private readonly ShoppingContext dbContext;

        public StoreRepository(ShoppingContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<IStore>> FindActiveStoresAsync(CancellationToken cancellationToken)
        {
            var storeEntities = await dbContext.Stores.AsNoTracking()
                .Where(store => !store.Deleted)
                .ToListAsync();

            cancellationToken.ThrowIfCancellationRequested();

            return storeEntities.Select(store => store.ToDomain());
        }

        public async Task<IStore> FindByAsync(StoreId id, CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            cancellationToken.ThrowIfCancellationRequested();

            var entity = await dbContext.Stores.AsNoTracking()
                .FirstOrDefaultAsync(store => store.Id == id.Value);

            if (entity == null)
                throw new StoreNotFoundException(id);

            cancellationToken.ThrowIfCancellationRequested();

            return entity.ToDomain();
        }

        public async Task<bool> IsValidIdAsync(StoreId id, CancellationToken cancellationToken)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            cancellationToken.ThrowIfCancellationRequested();

            var entity = await dbContext.Stores.AsNoTracking()
                .FirstOrDefaultAsync(store => store.Id == id.Value);

            cancellationToken.ThrowIfCancellationRequested();

            return entity != null;
        }

        public async Task<StoreId> StoreAsync(IStore store, CancellationToken cancellationToken)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));

            var storeEntity = store.ToEntity();

            cancellationToken.ThrowIfCancellationRequested();

            if (store.Id.Value == 0)
            {
                dbContext.Entry(storeEntity).State = EntityState.Added;
            }
            else
            {
                dbContext.Entry(storeEntity).State = EntityState.Modified;
            }

            cancellationToken.ThrowIfCancellationRequested();

            await dbContext.SaveChangesAsync();
            return new StoreId(storeEntity.Id);
        }
    }
}